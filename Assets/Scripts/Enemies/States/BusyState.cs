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

    public override void UpdateState(EnemyStateManager enemy)
    {
        // TODO replace with bool and animation event?
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1.0f <= 0.95f) // Wait until attack animation finishes
        {
            return;
        }
        // now that animation finishes
        if (enemy.HasLineOfSight(true))
        {
            if (enemy.pool.HasActionsInRange()) // If enemy still in attack range, return to AggroState
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
