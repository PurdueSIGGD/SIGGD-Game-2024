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

    // Check for collision in swing range to deal damage
    protected void OnBatonEvent()
    {
        GenerateDamageFrame(batonTrigger.position, batonTrigger.lossyScale.x, batonTrigger.lossyScale.y, 1);
    }

    // Draws the Enemy attack range in the editor
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireCube(batonTrigger.position, batonTrigger.lossyScale);
    }
}
