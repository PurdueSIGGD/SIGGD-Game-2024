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

    /// <summary>
    /// 
    /// </summary>
    private void HandleGroupAggro(EnemyStateManager enemy)
    {
        
        float aggroRadius = enemy.stats.ComputeValue(AGGRO_RADIUS);

        // TODO: get enemies array.

        foreach (EnemyStateManager e in enemies)
        {
            if (e.Equals(enemy)) { continue; }

            // Calculate distance to other enemy

            float d = Vector2.Distance(enemy.transform.position, e.transform.position);

            if (d <= aggroRadius)
            {
                e.SwitchState(enemy.AggroState);
            }


        }
    }

    public void EnterState(EnemyStateManager enemy)
    {
        rb = enemy.GetComponent<Rigidbody2D>();
        if (!enemy.isBeingKnockedBack && (enemy.isFlyer || enemy.isGrounded())) rb.velocity = new Vector2(0, rb.velocity.y); // Make sure Enemy stops moving
        enemy.pool.idle.Play(enemy.animator); // Play the idle animation when in between attacks
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

            HandleGroupAggro(enemy);
        }
        Action nextAction = enemy.pool.NextAction(); // If an action is ready, play it in BusyState
        if (nextAction != null)
        {
            nextAction.Play(enemy.animator);
            enemy.SwitchState(enemy.BusyState);
        }
    }


}
