using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

/// <summary>
/// Enemy behavior when no player in sight
/// </summary>
public class IdleState : IEnemyStates
{
    // TODO merge in patrol behavior in IdleState

    // TODO implement Enemy returning to origin after loosing aggro

    private Vector2 currTarget; // Current Patrol endpoint
    public float pauseTimerDurationMin = 1f; // How much time to pass in between patrols (MIN)
    public float pauseTimerDurationMax = 4f; // How much time to pass in between patrols (MAX)
    public float targetRangeLeft = 5f; // Target point can be at most this far left from spawn
    public float targetRangeRight = 10f; // Target point can be at most this far right from spawn
    public float minSpeedPercent = 0.8f; // Min percent of speed that enemy can patrol 


    private float patrolPauseTimer = 0f; // Used for when enemy pauses near patrol endpoint
    public float patrolTargetRadius = 0.1f; // How close enemy needs to be from the target point to stop

    private Vector2 spawnPoint; // The initial point of the enemy

    /// <summary>
    /// 
    /// </summary>
    /// <param name="enemy"></param>
    public void EnterState(EnemyStateManager enemy)
    {
        Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
        if (!enemy.isBeingKnockedBack && (enemy.isFlyer || enemy.isGrounded())) rb.velocity = new Vector2(0, rb.velocity.y);
        enemy.pool.move.Play(enemy.animator); // Play the move animation

        spawnPoint = new Vector2(enemy.transform.position.x, enemy.transform.position.y);
        SetPatrolTarget(enemy);

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="enemy"></param>
    private void SetPatrolTarget(EnemyStateManager enemy)
    {
        if (currTarget.x > spawnPoint.x)
        {
            currTarget = new Vector2(spawnPoint.x - Random.Range(0, targetRangeLeft), spawnPoint.y);
        }
        else {
            currTarget = new Vector2(spawnPoint.x + Random.Range(0, targetRangeRight), spawnPoint.y);
        }
    }

    /// <summary>
    /// 
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

            float patrolSpeed = enemy.stats.ComputeValue("Speed") * Random.Range(minSpeedPercent, 1);
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

            if (Mathf.Abs(enemy.transform.position.x - currTarget.x) <= patrolTargetRadius)
            {
                patrolPauseTimer = Random.Range(pauseTimerDurationMin, pauseTimerDurationMax);
                enemy.pool.idle.Play(enemy.animator); // Play the idle animation
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
        }


    }

    /// <summary>
    /// 
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
