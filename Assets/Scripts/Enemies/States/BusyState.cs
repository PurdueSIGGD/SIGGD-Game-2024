using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class BusyState : EnemyStates
{
    public Animator animator;
    public override void EnterState(EnemyStateManager enemy)
    {
        animator = enemy.animator;
    }

    public override void UpdateState(EnemyStateManager enemy)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1.0f <= 0.95f)
        {
            return;
        }

        if (enemy.HasLineOfSight(true))
        {
            if (enemy.pool.HasActionsInRange())
            {
                enemy.SwitchState(enemy.AggroState);
            }
            else
            {
                enemy.SwitchState(enemy.MoveState);
            }
        }
        else
        {
            enemy.SwitchState(enemy.IdleState);
        }
    }
}
