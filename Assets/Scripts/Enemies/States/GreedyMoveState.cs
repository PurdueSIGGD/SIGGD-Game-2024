using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A Greedy Move state where enemy will appear more aggressive
/// Please use with GreedyAggroState
/// </summary>
public class GreedyMoveState : MoveState
{
    public override void UpdateState(EnemyStateManager enemy)
    {
        if (enemy.pool.HasActionsReady()) // If Enemy can attack immediately, enter AggroState
        {
            enemy.SwitchState(enemy.AggroState);
        }
        else if (enemy.HasLineOfSight(true)) // Otherwise, move towards player
        {
            Move(enemy);
        }
        else // If line of sight is lost, enter IdleState
        {
            enemy.SwitchState(enemy.IdleState);
        }
    }
}
