using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Enemy AI for Riot Police
/// </summary>
[Serializable]
public class RiotPolice : EnemyStateManager
{
    protected Transform batonTrigger;
    protected float batonDamage;

    // Check for collision in swing range to deal damage
    protected void OnBatonEvent()
    {
        GenerateDamageFrame(batonTrigger.position, batonTrigger.lossyScale.x, batonTrigger.lossyScale.y, batonDamage);
    }

    // Draws the Enemy attack range in the editor
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireCube(batonTrigger.position, batonTrigger.lossyScale);
    }
}
