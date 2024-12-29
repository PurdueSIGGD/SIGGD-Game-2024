using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Enemy AI for gun with legs
/// </summary>
[Serializable]
public class GunWithLegs : EnemyStateManager
{
    protected Transform gunKick;
    protected float kickDamage;
    protected Transform gunShoot;
    protected Transform rangeOrig;

    protected GameObject projectile;

    // Check for kick collision and do damage
    protected void OnKickEvent()
    {
        GenerateDamageFrame(gunKick.position, gunKick.lossyScale.x, gunKick.lossyScale.y, kickDamage);
    }

    // Instantiate projectile prefab and push self back
    protected void OnShootEvent()
    {
        Instantiate(projectile, rangeOrig.position, transform.rotation);

        rb.AddForce(transform.right * -3, ForceMode2D.Impulse);
    }


    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireCube(gunKick.position, gunKick.lossyScale);
        Gizmos.DrawWireCube(gunShoot.position, gunShoot.lossyScale);
        Gizmos.DrawWireSphere(rangeOrig.position, 0.1f);
    }
}
