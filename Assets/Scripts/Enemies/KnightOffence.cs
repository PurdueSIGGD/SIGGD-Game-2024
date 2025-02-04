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
        GenerateDamageFrame(swordTrigger.position, swordTrigger.lossyScale.x, swordTrigger.lossyScale.y, swordDamage, gameObject);
    }

    // Draws the Enemy attack range in the editor
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireCube(swordTrigger.position, swordTrigger.lossyScale);
    }
}
