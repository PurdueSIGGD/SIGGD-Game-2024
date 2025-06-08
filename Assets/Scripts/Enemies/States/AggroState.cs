//#define DEBUG_LOG

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

/// <summary>
/// Enemy behavior when aggroing on player
/// If aggroed enemy is within distance of an idle enemy, the other enemy will also be aggroed
/// </summary>
public class AggroState : IEnemyStates
{
    private Rigidbody2D rb;
    
    private const string AGGRO_RADIUS = "Enemy Group Aggro Radius"; // Max distance for enemy group aggro
    private const float DEFAULT_AGGRO_RADIUS = 3.0f; // If the radius is invalid, set a default value

    /// <summary>
    /// Checks other nearby enemies and changes their state to AggroState if not already aggroed
    /// </summary>
    private void EnemyGroupAggro(EnemyStateManager enemy)
    {
        
        float aggroRadius = enemy.stats.ComputeValue(AGGRO_RADIUS);
        if (aggroRadius < 0)
        {
            aggroRadius = DEFAULT_AGGRO_RADIUS;

#if DEBUG_LOG
            Debug.Log("Using default enemy group aggro radius of " + DEFAULT_AGGRO_RADIUS);
#endif
        }

        // Find enemies

        List<GameObject> enemies = PersistentData.Instance.GetComponent<EnemySpawning>().GetCurrentEnemies();

        foreach (GameObject e in enemies)
        {
            EnemyStateManager otherEnemy = e.GetComponent<EnemyStateManager>();

            // If enemy is self or already aggroed, continue

            if (otherEnemy.Equals(enemy)) { continue; }

            if (!otherEnemy.GetCurrentState().Equals(otherEnemy.IdleState)) {
                continue;
            }

            // Calculate distance to other enemy

            float d = Vector2.Distance(otherEnemy.transform.position, enemy.transform.position);

            if (d <= aggroRadius)
            {
                otherEnemy.SwitchState(otherEnemy.AggroState);
#if DEBUG
                Debug.Log("Enemy " + otherEnemy.name + " was aggroed by " + enemy.name);
#endif
            }


        }
    }

    public void EnterState(EnemyStateManager enemy)
    {
        rb = enemy.GetComponent<Rigidbody2D>();
        if (!enemy.isBeingKnockedBack && (enemy.isFlyer || enemy.isGrounded())) rb.velocity = new Vector2(0, rb.velocity.y); // Make sure Enemy stops moving
        enemy.pool.idle.Play(enemy); // Play the idle animation when in between attacks

        // Handle enemy group aggro

        EnemyGroupAggro(enemy);
    }

    public void UpdateState(EnemyStateManager enemy)
    {
        if (!enemy.HasLineOfSight(true)) // If line of sight is lost, return to IdleState
        {
            enemy.SwitchState(enemy.IdleState);
            return;
        }
        if (!enemy.pool.HasActionsReady()) // If player moves away, enter MoveState
        {
            enemy.SwitchState(enemy.MoveState);
            return;
        }
        else
        {
            // Handle enemy group aggro

            EnemyGroupAggro(enemy);
        }

        Action nextAction = enemy.pool.NextAction(); // If an action is ready, play it in BusyState
        if (nextAction != null)
        {
            nextAction.Play(enemy);
            enemy.SwitchState(enemy.BusyState);
        }
    }


}
