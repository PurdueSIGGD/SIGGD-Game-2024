using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

/// <summary>
/// Enemy behavior when executing attack animations
/// </summary>
public class BusyState : EnemyStates
{
    public Animator animator;

    public override void EnterState(EnemyStateManager enemy)
    {
        animator = enemy.animator;
    }

    // We want the enemy to do nothing for the duration of the animation
    public override void UpdateState(EnemyStateManager enemy) { }

    // Once the animation is finished, transition to the next state
    public override void ExitState(EnemyStateManager enemy)
    {
        if (enemy.HasLineOfSight(true))
        {
            if (enemy.pool.HasActionsReady()) // If enemy still in attack range, return to AggroState
            {
                enemy.SwitchState(enemy.AggroState);
            }
            else // If enemy no longer in attack range, enter MoveState
            {
                enemy.SwitchState(enemy.MoveState);
            }
        }
        else // If no longer has line of sight, return to IdleState
        {
            enemy.SwitchState(enemy.IdleState);
        }
    }
}
