using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Enemy AI for Knight (Offence Variant)
/// </summary>
public class KnightOffence : EnemyStateManager
{
    [Header("Sword Attack")]
    [SerializeField] protected Transform swordTrigger;
    [SerializeField] protected DamageContext swordDamage;

    // Check for collision in swing range to deal damage
    protected void OnSwordEvent()
    {
        GenerateDamageFrame(batonTrigger.position, batonTrigger.lossyScale.x, batonTrigger.lossyScale.y, batonDamage, gameObject);
    }

    // Draws the Enemy attack range in the editor
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireCube(batonTrigger.position, batonTrigger.lossyScale);
    }
}
