using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

public class CrowMoveState : MoveState
{

    // Movement override to make bird keep birding when action is on cooldown
    public virtual void UpdateState(EnemyStateManager enemy)
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

    // Movement override to make bird more bird
    protected override void Move(EnemyStateManager enemy)
    {

        float heightOffset = enemy.stats.ComputeValue("HEIGHT_OFFSET"); // how far up target is from player
        float flightForce = enemy.stats.ComputeValue("FLIGHT_FORCE"); // force of flight
        float catchupFactor = enemy.stats.ComputeValue("CATCHUP_FACTOR"); // idk if this even does anything anymore

        float repellFactor = enemy.stats.ComputeValue("REPELL_FACTOR"); // repelling force of player on crow
        float minRepell = enemy.stats.ComputeValue("MIN_REPELL"); // minimum distance for repelling force to take effect
        float randomFactor = enemy.stats.ComputeValue("RANDOM_FACTOR"); // force of random force

        if (player.position.x - enemy.transform.position.x < 0)
        {
            enemy.Flip(false);
        }
        else
        {
            enemy.Flip(true);
        }

        Vector2 target = new Vector2(player.position.x, player.position.y) + Vector2.up * heightOffset;
        Vector2 current = new Vector2(enemy.gameObject.transform.position.x, enemy.gameObject.transform.position.y);

        Vector2 directionRaw = target - current;
        Vector2 direction = new Vector2(directionRaw.x, directionRaw.y).normalized;

        rb.AddForce(flightForce * direction, ForceMode2D.Impulse);

        // Slowing down as approaching target y level
        float yDiff = target.y - enemy.transform.position.y;
        yDiff = yDiff < 0.001f ? 0.1f : yDiff;
        rb.AddForce(1 / yDiff * catchupFactor * Vector2.up, ForceMode2D.Impulse);

        // BUT ALSO DON'T GET TOO CLOSE!!!
        Vector2 directionToPlayer = new Vector2(player.position.x - enemy.transform.position.x, player.position.y - enemy.transform.position.y);
        float distanceToPlayer = directionToPlayer.magnitude;
        if (distanceToPlayer < minRepell)
        {
            // avoid any division by zero
            distanceToPlayer = distanceToPlayer < 0.001f ? 0.1f : distanceToPlayer;
            float repellMagnitude = -1 / distanceToPlayer * repellFactor;
            if (repellMagnitude < -2)
            {
                repellMagnitude = -2;
            }
            rb.AddForce(repellMagnitude * directionToPlayer.normalized.x * Vector2.right, ForceMode2D.Impulse);
        }

        // RANDOM MOVEMENT!!!
        Vector2 random_vector = new Vector2(Random.value * 2 - 1, Random.value * 2 - 1);
        rb.AddForce(randomFactor * random_vector.normalized, ForceMode2D.Impulse);
    }
}
