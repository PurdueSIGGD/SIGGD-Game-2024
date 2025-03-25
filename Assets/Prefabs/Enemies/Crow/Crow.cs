using System;
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
    [SerializeField] protected int damage;
    [SerializeField] GameObject poisonDebuff;
    protected bool diving = false;

    void Start()
    {
        MoveState = new CrowMoveState();
        diveDamage.damage = damage;
    }

    // Check for dive collision and do damage
    protected void OnDiveEvent()
    {
        if (diving)
        {
            return;
        }
        diving = true;
        print("DIVING AHHHHHH");

        // dive charge up wait time
        float waitTime = stats.ComputeValue("DIVE_WAIT");
        float diveSpeed = stats.ComputeValue("DIVE_SPEED");
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        StartCoroutine(DiveWaitCoroutine(rb, diveSpeed, waitTime));
    }

    public void EndDive()
    {
        transform.rotation = Quaternion.identity;
        diving = false;
        StopAllCoroutines();
        BusyState.ExitState(this);
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

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        // Gizmos.DrawWireCube(gunKick.position, gunKick.lossyScale);
        // Gizmos.DrawWireCube(gunShoot.position, gunShoot.lossyScale);
        // Gizmos.DrawWireSphere(rangeOrig.position, 0.1f);
    }

    IEnumerator DiveWaitCoroutine(Rigidbody2D rb, float diveSpeed, float waitTime)
    {

        print("WAIT............");
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(waitTime);
        Vector2 directionToPlayer = new Vector2(player.position.x - transform.position.x, player.position.y - transform.position.y);
        Vector2 directionX = (Vector2.right * directionToPlayer.x).normalized;
        // rotate crow to face player
        float angle = Vector2.Angle(Vector2.right * -1 * directionToPlayer.x, directionToPlayer.normalized);
        angle = angle - 180;
        int rot_flip = directionToPlayer.x > 0 ? 0 : 180;
        // angle = directionToPlayer.x > 0 ? angle - 180 : 360 - angle;
        transform.rotation = Quaternion.Euler(new Vector3(0, rot_flip, angle));

        // record whether the dive started on the left(<0) or right(>0) of the player
        StartCoroutine(DiveMoveCoroutine(rb, diveSpeed, directionToPlayer.x, directionToPlayer.y, directionToPlayer.normalized));
    }

    IEnumerator DiveMoveCoroutine(Rigidbody2D rb, float diveSpeed, float startX, float startY, Vector2 prevDir)
    {
        while (true)
        {
            Debug.DrawLine(crowDive.position, crowDive.position + Vector3.right * 0.65f);
            print("GRAHHHHHHHH");
            if (GenerateDamageFrame(crowDive.position, 0.65f, diveDamage, gameObject))
            {
                print("WE HIT EM LETS GOOOOOO");
                Instantiate(poisonDebuff, player.transform);
                EndDive();
                break;
            }
            if (Physics2D.OverlapCircle(crowDive.position, 0.65f, LayerMask.GetMask("Ground")))
            {
                print("CRASHED INTO DA GROUND");
                EndDive();
                break;
            }

            Vector2 directionToPlayer = player.position - transform.position;

            // stop dive if crow flies through / past player
            // crow on opposite side of start side always is negative (-x * y)
            // crow on same side of start side (hasn't reached player yet) is always positive (-x * -y or x * y)
            if ((directionToPlayer.x * startX <= 0) &&
                (directionToPlayer.y * startY <= 0))
            {
                break;
            }

            Vector2 newVelocity = diveSpeed * prevDir;
            rb.velocity = newVelocity;
            yield return null;
        }
        EndDive();
    }
}
