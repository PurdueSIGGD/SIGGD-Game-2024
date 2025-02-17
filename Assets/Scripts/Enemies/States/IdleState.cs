using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

/// <summary>
/// Enemy behavior when no player in sight
/// </summary>
public class IdleState : IEnemyStates
{
    // TODO merge in patroll behavior in IdleState
    // TODO implement Enemy returning to origin after loosing aggro

    public void EnterState(EnemyStateManager enemy)
    {
        Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;
        enemy.pool.idle.Play(enemy.animator); // Play the idle animation
    }

    public void UpdateState(EnemyStateManager enemy)
    {
        if (enemy.pool.HasActionsReady()) // If has player in attack range, enter AggroState
        {
            enemy.SwitchState(enemy.AggroState);
        }
        else if (enemy.HasLineOfSight(false)) // If player enters line of sight but not in range, enter MoveState
        {
            enemy.SwitchState(enemy.MoveState);
        }
    }
}
