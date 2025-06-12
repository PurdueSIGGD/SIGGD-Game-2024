using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using System.Xml.Serialization;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

/// <summary>
/// Enemy behavior when no player in sight
/// </summary>
public class IdleState : IEnemyStates
{

    // TODO: Make enemeies return to origin when they lose sight of the player

    private const string IDLE_SPEED = "Idle Speed"; // Speed of enemy
    private const string PAUSE_TIME_MIN = "Pause Timer Duration Min"; // How much time to pass in between patrols (MIN)
    private const string PAUSE_TIME_MAX = "Pause Timer Duration Max"; // How much time to pass in between patrols (MAX)
    private const string TARGET_RANGE_MIN = "Target Range Min"; // Target point can be at most this far from pivot point
    private const string TARGET_RANGE_MAX = "Target Range Max"; // Target point is at least this far from pivot
    
    private const float PATROL_TARGET_RADIUS = 0.1f; // How close enemy needs to be from the target point to stop/margin of error
    
    private float patrolPauseTimer = 0f; // Used for when enemy pauses near patrol endpoint
    private Vector2 currTarget; // Current Patrol endpoint
    private Vector2 pivotPoint; // The initial point of the enemy

    /// <summary>
    /// Enter state, start animation and set first patrol target
    /// </summary>
    /// <param name="enemy"></param>
    public void EnterState(EnemyStateManager enemy)
    {
        Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
        if (!enemy.isBeingKnockedBack && (enemy.isFlyer || enemy.isGrounded())) rb.velocity = new Vector2(0, rb.velocity.y);
        enemy.pool.move.Play(enemy.animator); // Play the move animation

        // Save initial point when entering the state

        pivotPoint = new Vector2(enemy.transform.position.x, enemy.transform.position.y);
        SetPatrolTarget(enemy);

    }

    /// <summary>
    /// Chooses the next point to walk to
    /// </summary>
    /// <param name="enemy"></param>
    private void SetPatrolTarget(EnemyStateManager enemy)

    {
        float distanceFromPivot = Random.Range(enemy.stats.ComputeValue(TARGET_RANGE_MIN), enemy.stats.ComputeValue(TARGET_RANGE_MAX));
        
        if (currTarget.x > pivotPoint.x)
        {
            currTarget = new Vector2(pivotPoint.x - distanceFromPivot, pivotPoint.y);
        }
        else {
            currTarget = new Vector2(pivotPoint.x + distanceFromPivot, pivotPoint.y);
        }
    }

    /// <summary>
    /// Controls patrolling behavior, timer, and animation
    /// </summary>
    /// <param name="enemy"></param>
    private void Patrol(EnemyStateManager enemy)
    {

        if (patrolPauseTimer > 0.0f)
        {
            patrolPauseTimer -= Time.deltaTime;

            // Timer is up

            if (patrolPauseTimer <= 0.0f)
            {
                patrolPauseTimer = 0.0f;

                // Set the new target

                SetPatrolTarget(enemy);

                // Play the move animation

                enemy.pool.move.Play(enemy.animator); // Play the move animation

            }

        }
        else
        {
            // Speed

            float patrolSpeed = enemy.stats.ComputeValue(IDLE_SPEED);
            Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();

            if (enemy.transform.position.x - currTarget.x < 0)
            {
                enemy.Flip(true);
                rb.velocity = new Vector2(patrolSpeed, rb.velocity.y);
            }
            else
            {
                enemy.Flip(false);
                rb.velocity = new Vector2(-patrolSpeed, rb.velocity.y);
            }

            // If we are close enough to endpoint, start timer and set idle animation

            if (Mathf.Abs(enemy.transform.position.x - currTarget.x) <= PATROL_TARGET_RADIUS)
            {
                StopPatrol(enemy);
                return;
            }

            // Check for wall hit
            // If we are close enough to endpoint or we hit a wall, start timer and set idle animation

            RaycastHit2D hitWall = Physics2D.Raycast(enemy.transform.position, 
                                                     rb.velocity,
                                                     1f, 
                                                     LayerMask.GetMask("Ground"));

            if (hitWall)
            {
                StopPatrol(enemy);
                return;
            }

            // For non-flying, check if we are at a cliff/edge

            if (!enemy.isFlyer)
            {
                RaycastHit2D hasGround = Physics2D.Raycast(new Vector2(enemy.transform.position.x + Mathf.Sign(rb.velocity.x), enemy.transform.position.y),
                                                           Vector2.down,
                                                           2f,
                                                           LayerMask.GetMask("Ground"));

                if (!hasGround)
                {
                    StopPatrol(enemy);
                    return;
                }
            }
        }


    }

    /// <summary>
    /// Run this code when the enemy stops at the patrol target
    /// </summary>
    /// <param name="enemy"></param>
    void StopPatrol(EnemyStateManager enemy)
    {
        patrolPauseTimer = Random.Range(enemy.stats.ComputeValue(PAUSE_TIME_MIN), enemy.stats.ComputeValue(PAUSE_TIME_MAX));

        enemy.pool.idle.Play(enemy.animator); // Play the idle animation

        Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0, rb.velocity.y);
    }

    /// <summary>
    /// Conditions to change state or keep patrolling
    /// </summary>
    /// <param name="enemy"></param>
    public void UpdateState(EnemyStateManager enemy)
    {
        if (enemy.pool.HasActionsReady()) // If has player in attack range, enter AggroState
        {
            enemy.SwitchState(enemy.AggroState);
            patrolPauseTimer = 0;
        }
        else if (enemy.HasLineOfSight(false)) // If player enters line of sight but not in range, enter MoveState
        {
            enemy.SwitchState(enemy.MoveState);
            patrolPauseTimer = 0;
        }
        else
        {
            // Patrolling behavior

            Patrol(enemy);           
        }
    }
}
