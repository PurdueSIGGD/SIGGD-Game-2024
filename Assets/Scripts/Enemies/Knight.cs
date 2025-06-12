using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : EnemyStateManager
{
    [Header("Sword Attack")]
    [SerializeField] protected Transform swordTrigger;
    [SerializeField] protected DamageContext swordDamage;

    protected override void Start()
    {
        base.Start();
        swordDamage.damage = 20.0f;
    }

    // Check for collision in swing range to deal damage
    protected void OnSlashEvent()
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
