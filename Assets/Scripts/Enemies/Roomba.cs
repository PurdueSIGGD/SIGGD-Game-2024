using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enemy AI for Roomba
/// </summary>
public class Roomba : EnemyStateManager
{
    [SerializeField] protected Transform kaboomTrigger;

    protected override ActionPool GenerateActionPool()
    {
        Action kaboom = new(kaboomTrigger, 0.0f, 1f, "kaboom");

        Action move = new(null, 0.0f, 0.0f, "move");
        Action idle = new(null, 0.0f, 0.0f, "idle");

        return new ActionPool(new List<Action> { kaboom }, move, idle);
    }

    // Check for player in blast radius and do damage
    protected void OnKaboomEvent()
    {
        Collider2D hit = Physics2D.OverlapCircle(kaboomTrigger.position, kaboomTrigger.lossyScale.x, LayerMask.GetMask("Player"));
        if (hit)
        {
            hit.GetComponent<PlayerHealth>().TakeDamage(20);
        }
    }

    protected override void OnFinishAnimation()
    {
        base.OnFinishAnimation();
        Destroy(gameObject);
    }

    // Draws the Enemy attack range in the editor
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireSphere(kaboomTrigger.position, kaboomTrigger.lossyScale.x);
    }
}
