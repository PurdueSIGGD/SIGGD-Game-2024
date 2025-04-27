using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDrone : EnemyStateManager
{
    [Header("Call Reinforcement")]
    [SerializeField] protected Transform alarmTrigger;
    [SerializeField] protected GameObject enemyToSummon;
    [SerializeField] float detectionRadius = 1;

    void Start()
    {
        MoveState = new CameraDroneMoveState();
        detectionRadius = stats.ComputeValue("DETECTION_RADIUS");
        print("START AH: " + detectionRadius);
    }
    /// <summary>
    /// Summons an enemy
    /// </summary>
    protected void OnCallAlarm()
    {
        Vector3 dest = transform.position + new Vector3(transform.right.x * transform.lossyScale.x, -transform.lossyScale.y, 0);
        Instantiate(enemyToSummon, dest, transform.rotation);
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
        print("AHHHHH: " + detectionRadius);
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

        print("AHHHHH: " + maxDistance);
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
