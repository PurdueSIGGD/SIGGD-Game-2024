using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class IdleState : EnemyStates
{
    public override void EnterState(EnemyStateManager enemy)
    {
        Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;
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
