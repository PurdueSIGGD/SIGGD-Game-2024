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

    private float spawningTimer;

    protected override void Awake()
    {
        IdleState = new SurveyDroneIdleState();
        spawnPoints = PersistentData.Instance.GetComponent<EnemySpawning>().GetSpawnPoints();
        base.Awake();
    }

    void Start()
    {
        spawningTimer = stats.ComputeValue("Spawn Interval");
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        spawningTimer -= Time.deltaTime;
    }

    protected void ApproachSpawnPoint()
    {
        // find the closest spawn point
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
                if (dist <= closestDistance)
                {
                    closestPoint = targetLoc;
                    closestDistance = dist;
                }
            }
        }

        // if no a single spawn point is reachable
        // fugg it, I'm laying my egg right here
        // <insert code to enable OnCallAlarm>

        // if a viable point is found, start moving towards it
        if ((closestPoint - (Vector2)transform.position).magnitude <= 0.5f)
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
        if (spawningTimer < 0)
        {
            spawningTimer = stats.ComputeValue("Spawn Interval");
            Vector3 dest = transform.position; // + new Vector3(transform.right.x * transform.lossyScale.x, -transform.lossyScale.y, 0);
            GameObject nenemy = Instantiate(enemyToSummon, dest, transform.rotation);
            Destroy(nenemy.GetComponent<DropTable>());
        }

    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireCube(alarmTrigger.position, alarmTrigger.lossyScale);
    }
    //public override bool HasLineOfSight(bool tracking)
    //{
    //    bool hit_player = false;

    //    Vector2 dir = transform.TransformDirection(Vector2.right);
    //    float maxDistance = detectionRadius;


    //    // track player if player is being tracked
    //    if (tracking)
    //    {
    //        maxDistance = maxDistance * 1.2f;
    //        float maxTrackDistance = maxDistance * 2;

    //        dir = player.position - transform.position;

    //        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, maxTrackDistance, LayerMask.GetMask("Player", "Ground"));
    //        //Debug.DrawRay(transform.position, dir);
    //        if (hit && hit.collider.gameObject.CompareTag("Player"))
    //        {
    //            hit_player = true;
    //        }
    //    }

    //    // if not tracking player
    //    // casts numRays rays in a circle to seek player
    //    int numRays = 16;
    //    for (float deg = 0; deg < (360 * Mathf.Deg2Rad); deg += 360 / numRays * Mathf.Deg2Rad)
    //    {
    //        // calculate unit vector direction based on angle
    //        dir = new Vector2(Mathf.Cos(deg), Mathf.Sin(deg));
    //        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, maxDistance, LayerMask.GetMask("Player", "Ground"));
    //        Debug.DrawRay(transform.position, dir * maxDistance);
    //        if (hit && hit.collider.gameObject.CompareTag("Player"))
    //        {
    //            hit_player = true;
    //        }
    //    }
    //    return hit_player;
    //}
}
