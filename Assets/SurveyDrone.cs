using UnityEngine;

public class SurveyDrone : EnemyStateManager
{
    [Header("Call Reinforcement")]
    [SerializeField] protected Transform alarmTrigger;
    [SerializeField] protected GameObject enemyToSummon;

    private GameObject[] spawnPoints; // all avaliable spawn points
    private float detectionRadius;

    void Start()
    {
        spawnPoints = PersistentData.Instance.GetComponent<EnemySpawning>().GetSpawnPoints();
        detectionRadius = stats.ComputeValue("DETECTION_RADIUS");
    }

    protected void ApproachSpawnPoint()
    {
        Vector2 closestPoint = new();
        float closestDistance = float.MaxValue;

        // find the closest spawn point
        foreach (GameObject spawnPoint in spawnPoints)
        {
            // ray cast to see if each point is reachable
            Vector2 targetLoc = spawnPoint.transform.position;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, (targetLoc - (Vector2)transform.position), LayerMask.GetMask("Ground"));
            if (!hit)
            {
                float dist = (targetLoc - (Vector2)transform.position).magnitude;
                if (dist < closestDistance)
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
        Vector2 dir = (closestPoint - (Vector2)transform.position).normalized;
        rb.velocity = stats.ComputeValue("Speed") * dir;
        if ((closestPoint - (Vector2)transform.position).magnitude <= 0.1f)
        {
            rb.velocity = Vector2.zero;
            pool.SetActionReady("Call Alarm");
            SwitchState(AggroState);
        }
    }


    /// <summary>
    /// Summons an enemy
    /// </summary>
    protected void OnCallAlarm()
    {
        Vector3 dest = transform.position; // + new Vector3(transform.right.x * transform.lossyScale.x, -transform.lossyScale.y, 0);
        GameObject nenemy = Instantiate(enemyToSummon, dest, transform.rotation);
        Destroy(nenemy.GetComponent<DropTable>());
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireCube(alarmTrigger.position, alarmTrigger.lossyScale);
    }
    public override bool HasLineOfSight(bool tracking)
    {
        bool hit_player = false;

        Vector2 dir = transform.TransformDirection(Vector2.right);
        float maxDistance = detectionRadius;


        // track player if player is being tracked
        if (tracking)
        {
            maxDistance = maxDistance * 1.2f;
            float maxTrackDistance = maxDistance * 2;

            dir = player.position - transform.position;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, maxTrackDistance, LayerMask.GetMask("Player", "Ground"));
            Debug.DrawRay(transform.position, dir);
            if (hit && hit.collider.gameObject.CompareTag("Player"))
            {
                hit_player = true;
            }
        }

        // if not tracking player
        // casts numRays rays in a circle to seek player
        int numRays = 16;
        for (float deg = 0; deg < (360 * Mathf.Deg2Rad); deg += 360 / numRays * Mathf.Deg2Rad)
        {
            // calculate unit vector direction based on angle
            dir = new Vector2(Mathf.Cos(deg), Mathf.Sin(deg));
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, maxDistance, LayerMask.GetMask("Player", "Ground"));
            Debug.DrawRay(transform.position, dir * maxDistance);
            if (hit && hit.collider.gameObject.CompareTag("Player"))
            {
                hit_player = true;
            }
        }
        return hit_player;
    }
}
