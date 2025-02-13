using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enemy AI for Mage
/// </summary>
public class Mage : EnemyStateManager
{
    [Header("Lightning Attack")]
    [SerializeField] protected Transform lightningTrigger;
    [SerializeField] protected DamageContext lightningDamage;
    [SerializeField] protected float damageRadius = 100;
    

    // Check for collision in swing range to deal damage
    protected void OnLightningEvent()
    {
        GenerateDamageFrame(lightningTrigger.position, damageRadius, lightningDamage, gameObject);
    }

    // Draws the Enemy attack range in the editor
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        UnityEditor.Handles.DrawWireDisc(collider.transform.position, Vector3.back, damageRadius)
    }
}
