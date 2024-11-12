using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class RiotPolice : EnemyStateManager
{
    [SerializeField] protected Transform batonHit;
    protected override ActionPool GenerateActionPool()
    {
        Action batonSwing = new(batonHit, 2.0f, 3f, "Riot_Police_Swing");

        Action move = new(null, 0.0f, 0.0f, "Riot_Police_Run");
        Action idle = new(null, 0.0f, 0.0f, "Riot_Police_Idle");

        return new ActionPool(new List<Action> { batonSwing }, move, idle);
    }

    protected void OnBatonEvent()
    {
        Collider2D hit = Physics2D.OverlapBox(batonHit.position, batonHit.lossyScale, 0f, LayerMask.GetMask("Player"));
        if (hit)
        {
            hit.GetComponent<PlayerHealth>().takeDamage(15);
        }
    }

    // Draws the Enemy attack range in the editor
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireCube(batonHit.position, batonHit.lossyScale);
    }
}
