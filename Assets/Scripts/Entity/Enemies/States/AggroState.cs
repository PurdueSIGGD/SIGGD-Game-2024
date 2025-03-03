using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

/// <summary>
/// Enemy behavior when aggroing on player
/// </summary>
public class AggroState : IEnemyStates
{
    private Rigidbody2D rb;

    public void EnterState(EnemyStateManager enemy)
    {
        rb = enemy.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0, rb.velocity.y); // Make sure Enemy stops moving
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
        Action nextAction = enemy.pool.NextAction(); // If an action is ready, play it in BusyState
        if (nextAction != null)
        {
            nextAction.Play(enemy.animator);
            enemy.SwitchState(enemy.BusyState);
        }
    }


}
