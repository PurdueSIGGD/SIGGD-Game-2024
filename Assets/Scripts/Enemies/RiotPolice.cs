using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Enemy AI for Riot Police
/// </summary>
public class RiotPolice : EnemyStateManager
{
    [SerializeField] protected Transform batonTrigger;
    protected override ActionPool GenerateActionPool()
    {
        Action batonSwing = new(batonTrigger, 2.0f, 1f, "Riot_Police_Swing");

        Action move = new(null, 0.0f, 0.0f, "Riot_Police_Run");
        Action idle = new(null, 0.0f, 0.0f, "Riot_Police_Idle");

        return new ActionPool(new List<Action> { batonSwing }, move, idle);
    }

    // Check for collision in swing range to deal damage
    protected void OnBatonEvent()
    {
        GenerateDamageFrame(batonTrigger, 1);
    }

    // Draws the Enemy attack range in the editor
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireCube(batonTrigger.position, batonTrigger.lossyScale);
    }
}
