using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enemy AI for Roomba
/// </summary>
[SerializeField]
public class Roomba : EnemyStateManager
{
    protected Transform kaboomTrigger;
    protected float kaboomDamage;

    // Check for player in blast radius and do damage
    protected void OnKaboomEvent()
    {
        GenerateDamageFrame(kaboomTrigger.position, kaboomTrigger.lossyScale.x, kaboomDamage);
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
