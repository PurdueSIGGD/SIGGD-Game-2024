using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enemy behavior when aggro but cannot reach player
/// </summary>
public class MoveState : IEnemyStates
{
    public Transform player;
    public Rigidbody2D rb;

    public void EnterState(EnemyStateManager enemy)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = enemy.GetComponent<Rigidbody2D>();
        enemy.pool.move.Play(enemy.animator); // Play the moving animation on entering state
    }

    public void UpdateState(EnemyStateManager enemy)
    {
        if (enemy.pool.HasActionsReady()) // If Enemy attacks can reach player, enter AggroState
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

    // Moves the Enemy body towards the player
    protected virtual void Move(EnemyStateManager enemy)
    {
        if (player.position.x - enemy.transform.position.x < 0)
        {
            enemy.Flip(false);
        }
        else
        {
            enemy.Flip(true);
        }

        float speed = enemy.stats.ComputeValue("Speed");
        rb.velocity = new Vector2(speed * enemy.transform.right.x, rb.velocity.y);
    }
}
