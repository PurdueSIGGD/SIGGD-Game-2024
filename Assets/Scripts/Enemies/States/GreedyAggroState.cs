using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A Greedy Aggro State where enemy will appear more aggressive
/// Please use with GreedyMoveState and make sure the enemy has at least 1
/// attack with no cooldown
/// </summary>
public class GreedyAggroState : AggroState
{
    public override void UpdateState(EnemyStateManager enemy)
    {
        enemy.pool.UpdateAllCD(); // Count down enemy's cool down

        if (!enemy.HasLineOfSight(true)) // If line of sight is lost, return to IdleState
        {
            enemy.SwitchState(enemy.IdleState);
            return;
        }
        Action nextAction = enemy.pool.NextAction();
        if (nextAction != null) // If an action is avaliable, use it immediately
        {
            nextAction.Play(enemy.animator);
            enemy.SwitchState(enemy.BusyState);
            return;
        }
        enemy.SwitchState(enemy.MoveState); // If no action is avalible, move until one is avalible
    }
}
