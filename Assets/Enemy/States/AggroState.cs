using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class AggroState : EnemyStates
{
    private Transform player;
    private Rigidbody2D rb;

    public override void EnterState(EnemyStateManager enemy)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = enemy.GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;
        enemy.pool.idle.Play(enemy.animator);
    }

    public override void UpdateState(EnemyStateManager enemy)
    {
        if (!enemy.HasLineOfSight(true))
        {
            enemy.SwitchState(enemy.IdleState);
            return;
        }
        if (!enemy.pool.HasActionsInRange())
        {
            enemy.SwitchState(enemy.MoveState);
        }
        Action nextAction = enemy.pool.NextAction();
        if (nextAction != null)
        {
            nextAction.Play(enemy.animator);
            enemy.SwitchState(enemy.BusyState);
        }
    }


}
