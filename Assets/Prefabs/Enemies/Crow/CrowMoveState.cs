using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

public class CrowMoveState : MoveState
{
    float heightOffset = 3;
    float flightForce = 0.5f;

    // Movement override to make bird more bird
    protected override void Move(EnemyStateManager enemy)
    {

        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();

        if (player.position.x - enemy.transform.position.x < 0)
        {
            enemy.Flip(false);
        }
        else
        {
            enemy.Flip(true);
        }

        float speed = enemy.stats.ComputeValue("Speed");
        Vector2 target = new Vector2(player.position.x, player.position.y) + Vector2.up * heightOffset;
        Vector2 current = new Vector2(enemy.gameObject.transform.position.x, enemy.gameObject.transform.position.y);

        Vector2 direction = target - current;
        direction.Normalize();

        rb.AddForce(flightForce * direction, ForceMode2D.Impulse);

    }
}
