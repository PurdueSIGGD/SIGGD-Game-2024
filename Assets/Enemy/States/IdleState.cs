using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class IdleState : EnemyStates
{
    public override void EnterState(EnemyStateManager enemy)
    {
        Rigidbody rb = enemy.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        enemy.pool.idle.Play(enemy.animator);
    }

    public override void UpdateState(EnemyStateManager enemy)
    {
        if (enemy.pool.HasActionsInRange())
        {
            enemy.SwitchState(enemy.AggroState);
        }
        else if (enemy.HasLineOfSight(false))
        {
            enemy.SwitchState(enemy.MoveState);
        }
    }
}
