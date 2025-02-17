using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crow : EnemyStateManager
{
    [Header("Custom Crow Tracking")]

    [SerializeField] float detectionRadius;

    [Header("Dive")]
    [SerializeField] protected Transform crowDive;
    [SerializeField] protected DamageContext diveDamage;

    // Check for dive collision and do damage
    protected void OnDiveEvent()
    {
        // GenerateDamageFrame(gunKick.position, gunKick.lossyScale.x, gunKick.lossyScale.y, kickDamage, gameObject);
    }

    public override bool HasLineOfSight(bool tracking)
    {
        bool hit_player = false;

        print("Looking!");
        Vector2 dir = transform.TransformDirection(Vector2.right);
        float maxDistance = detectionRadius;

        // track player if player is being tracked
        if (tracking)
        {
            maxDistance = maxDistance * 1.5f;
            dir = player.position - transform.position;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, maxDistance, LayerMask.GetMask("Player", "Ground"));
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

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        // Gizmos.DrawWireCube(gunKick.position, gunKick.lossyScale);
        // Gizmos.DrawWireCube(gunShoot.position, gunShoot.lossyScale);
        // Gizmos.DrawWireSphere(rangeOrig.position, 0.1f);
    }
}
