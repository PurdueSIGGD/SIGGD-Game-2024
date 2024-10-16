using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggroState : EnemyStates
{
    private Transform player;
    private Rigidbody rb;

    public override void EnterState(EnemyStateManager enemy)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = enemy.GetComponent<Rigidbody>();
    }

    public override void UpdateState(EnemyStateManager enemy)
    {
        Move(enemy);

        if (!enemy.HasLineOfSight(true))
        {
            enemy.SwitchState(enemy.IdleState);
            return;
        }
        if (enemy.pool.NextAction() != null)
        {

        }
    }

    private void Move(EnemyStateManager enemy)
    {
        if (player.position.x - enemy.transform.position.x < 0)
        {
            enemy.Flip(false);
            rb.velocity = Vector3.left * enemy.speed;
        }
        else
        {
            enemy.Flip(true);
            rb.velocity = Vector3.right * enemy.speed;
        }
    }
}
