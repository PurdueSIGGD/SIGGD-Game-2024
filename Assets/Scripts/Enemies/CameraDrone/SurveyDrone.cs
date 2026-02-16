using Unity.VisualScripting;
using UnityEngine;

public class SurveyDrone : EnemyStateManager
{
    [Header("Call Reinforcement")]
    [SerializeField] protected Transform alarmTrigger;
    [SerializeField] protected GameObject enemyToSummon;

    private GameObject[] spawnPoints; // all avaliable spawn points
    private float detectionRadius;
    private Vector2 closestPoint;
    private float closestDistance = float.MaxValue;
    private float spawningTimer = 0;
    private bool hasTarget = false;
    private int enemiesSpawned = 0;
    private bool isWindingUp = false;

    [SerializeField] protected GameObject spawnExplosionRing;
    [SerializeField] protected Color spawnExplosionColor;

    private AudioSource windupSFX = null;

    private EnemySpawning enemySpawning;

    private void OnEnable()
    {
        GameplayEventHolder.OnEntityStunned += OnDroneStunned;
        GameplayEventHolder.OnDeathFilter.Add(OnDroneKilled);
    }

    private void OnDisable()
    {
        GameplayEventHolder.OnEntityStunned -= OnDroneStunned;
        GameplayEventHolder.OnDeathFilter.Remove(OnDroneKilled);
    }

    protected override void Awake()
    {
        IdleState = new SurveyDroneIdleState();
        enemySpawning = PersistentData.Instance.GetComponent<EnemySpawning>();
        spawnPoints = enemySpawning.GetSpawnPoints();
        base.Awake();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        spawningTimer -= Time.deltaTime;
    }

    protected void ApproachSpawnPoint()
    {
        if (!hasTarget)
        {
            // find the closest spawn point respecting min player distance and min travel distance
            foreach (GameObject spawnPoint in spawnPoints)
            {
                // ray cast to see if each point is reachable
                Vector2 targetLoc = spawnPoint.transform.position;
                Debug.DrawRay(transform.position, (targetLoc - (Vector2)transform.position), Color.red, 1f);
                Vector2 line = (targetLoc - (Vector2)transform.position);
                RaycastHit2D hit = Physics2D.Raycast(transform.position, line.normalized, line.magnitude, LayerMask.GetMask("Ground"));
                if (!hit)
                {
                    float dist = (targetLoc - (Vector2)transform.position).magnitude;
                    float playerDist = (targetLoc - (Vector2)(player.transform.position)).magnitude;
                    if (dist <= closestDistance && playerDist >= stats.ComputeValue("Min Call Player Distance") && dist >= stats.ComputeValue("Min Call Travel Distance"))
                    {
                        closestPoint = targetLoc;
                        closestDistance = dist;
                        hasTarget = true;
                    }
                }
            }
            if (closestDistance == float.MaxValue)
            {
                // find the closest spawn point respecting only min player distance
                foreach (GameObject spawnPoint in spawnPoints)
                {
                    // ray cast to see if each point is reachable
                    Vector2 targetLoc = spawnPoint.transform.position;
                    Debug.DrawRay(transform.position, (targetLoc - (Vector2)transform.position), Color.red, 1f);
                    Vector2 line = (targetLoc - (Vector2)transform.position);
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, line.normalized, line.magnitude, LayerMask.GetMask("Ground"));
                    if (!hit)
                    {
                        float dist = (targetLoc - (Vector2)transform.position).magnitude;
                        float playerDist = (targetLoc - (Vector2)(player.transform.position)).magnitude;
                        if (dist <= closestDistance && playerDist >= stats.ComputeValue("Min Call Player Distance"))
                        {
                            closestPoint = targetLoc;
                            closestDistance = dist;
                            hasTarget = true;
                        }
                    }
                }
            }
        }
        else
        {
            closestDistance = (closestPoint - (Vector2)transform.position).magnitude;
        }
        // find the closest spawn point
        /*
        foreach (GameObject spawnPoint in spawnPoints)
        {
            // ray cast to see if each point is reachable
            Vector2 targetLoc = spawnPoint.transform.position;
            Debug.DrawRay(transform.position, (targetLoc - (Vector2)transform.position), Color.red, 1f);
            Vector2 line = (targetLoc - (Vector2)transform.position);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, line.normalized, line.magnitude, LayerMask.GetMask("Ground"));
            if (!hit)
            {
                float dist = (targetLoc - (Vector2)transform.position).magnitude;
                if (dist <= closestDistance && dist >= stats.ComputeValue("Min Call Travel Distance"))
                {
                    closestPoint = targetLoc;
                    closestDistance = dist;
                    hasTarget = true;
                }
            }
        }
        */

        // if reached spawn point, start spawning
        // if not a single spawn point is reachable, fugg it, I'm laying my egg right here
        if (closestDistance == float.MaxValue ||
           closestDistance <= 0.5f)
        {
            rb.velocity = Vector2.zero;
            closestDistance = float.MaxValue;
            animator.SetBool("SpawnPoint Reached", true);
        }
        else
        {
            Vector2 dir = (closestPoint - (Vector2)transform.position).normalized;
            rb.velocity = stats.ComputeValue("Speed") * dir;
            animator.SetBool("SpawnPoint Reached", false);
        }
    }


    /// <summary>
    /// Summons an enemy
    /// </summary>
    protected void OnCallAlarm()
    {
        windupSFX = AudioManager.Instance.SFXBranch.PlaySFXTrack("DroneSpawnWindup");
        isWindingUp = true;
        if (spawningTimer < 0)
        {
            spawningTimer = stats.ComputeValue("Spawn Interval");
            /*
            hasTarget = false;
            Vector3 dest = transform.position; // + new Vector3(transform.right.x * transform.lossyScale.x, -transform.lossyScale.y, 0);
            GameObject nenemy = Instantiate(enemyToSummon, dest, transform.rotation);
            enemySpawning.RegisterNewEnemy(nenemy);
            Destroy(nenemy.GetComponent<DropTable>());
            */
        }

    }

    protected void OnAlerted()
    {
        AudioManager.Instance.SFXBranch.PlaySFXTrack("DroneAlert");
    }

    protected void SpawnEnemy()
    {
        if (windupSFX != null)
        {
            windupSFX.Stop();
            windupSFX = null;
        }
        isWindingUp = false;
        AudioManager.Instance.SFXBranch.PlaySFXTrack("DroneSpawn");
        GameObject spawnRing = Instantiate(spawnExplosionRing, transform.position, Quaternion.identity);
        spawnRing.GetComponent<RingExplosionHandler>().playRingExplosion(2f, spawnExplosionColor);

        hasTarget = false;
        Vector3 dest = transform.position; // + new Vector3(transform.right.x * transform.lossyScale.x, -transform.lossyScale.y, 0);
        GameObject nenemy = Instantiate(enemyToSummon, dest, transform.rotation);
        enemySpawning.RegisterNewEnemy(nenemy);
        if (enemiesSpawned > 3) Destroy(nenemy.GetComponent<DropTable>());
        enemiesSpawned++;
    }

    private void CancelWindup()
    {
        if (isWindingUp)
        {
            isWindingUp = false;
            if (windupSFX != null)
            {
                windupSFX.Stop();
                windupSFX = null;
            }
        }
    }

    public void OnDroneStunned(GameObject stunnedEntity)
    {
        if (stunnedEntity != gameObject) return;
        CancelWindup();
    }

    public void OnDroneKilled(ref DamageContext context)
    {
        if (context.victim != gameObject) return;
        CancelWindup();
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireCube(alarmTrigger.position, alarmTrigger.lossyScale);
    }
    //public override bool haslineofsight(bool tracking)
    //{
    //    bool hit_player = false;

    //    vector2 dir = transform.transformdirection(vector2.right);
    //    float maxdistance = detectionradius;


    //    // track player if player is being tracked
    //    if (tracking)
    //    {
    //        maxdistance = maxdistance * 1.2f;
    //        float maxtrackdistance = maxdistance * 2;

    //        dir = player.position - transform.position;

    //        raycasthit2d hit = physics2d.raycast(transform.position, dir, maxtrackdistance, layermask.getmask("player", "ground"));
    //        //debug.drawray(transform.position, dir);
    //        if (hit && hit.collider.gameobject.comparetag("player"))
    //        {
    //            hit_player = true;
    //        }
    //    }

    //    // if not tracking player
    //    // casts numrays rays in a circle to seek player
    //    int numrays = 16;
    //    for (float deg = 0; deg < (360 * mathf.deg2rad); deg += 360 / numrays * mathf.deg2rad)
    //    {
    //        // calculate unit vector direction based on angle
    //        dir = new vector2(mathf.cos(deg), mathf.sin(deg));
    //        raycasthit2d hit = physics2d.raycast(transform.position, dir, maxdistance, layermask.getmask("player", "ground"));
    //        debug.drawray(transform.position, dir * maxdistance);
    //        if (hit && hit.collider.gameobject.comparetag("player"))
    //        {
    //            hit_player = true;
    //        }
    //    }
    //    return hit_player;
    //}
}
