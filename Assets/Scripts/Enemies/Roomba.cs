using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enemy AI for Roomba
/// </summary>
public class Roomba : EnemyStateManager
{
    [Header("Self Detonate")]
    [SerializeField] protected Transform kaboomTrigger;
    [SerializeField] protected DamageContext kaboomDamage;

    // Check for player in blast radius and do damage
    protected void OnKaboomEvent()
    {
        GenerateDamageFrame(kaboomTrigger.position, kaboomTrigger.lossyScale.x, kaboomDamage, gameObject);
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
