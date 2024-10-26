using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : EnemyStates
{
    public Transform player;
    public Rigidbody2D rb;

    public override void EnterState(EnemyStateManager enemy)
    {
        player = enemy.player;
        rb = enemy.GetComponent<Rigidbody2D>();
        enemy.pool.move.Play(enemy.animator);
    }

    public override void UpdateState(EnemyStateManager enemy)
    {
        if (enemy.pool.HasActionsInRange())
        {
            enemy.SwitchState(enemy.AggroState);
        }
        else if (enemy.HasLineOfSight(true))
        {
            Move(enemy);
        }
        else
        {
            enemy.SwitchState(enemy.IdleState);
        }
    }

    private void Move(EnemyStateManager enemy)
    {
        if (player.position.x - enemy.transform.position.x < 0)
        {
            enemy.Flip(false);
            rb.velocity = Vector2.left * enemy.speed;
        }
        else
        {
            enemy.Flip(true);
            rb.velocity = Vector2.right * enemy.speed;
        }
    }
}
