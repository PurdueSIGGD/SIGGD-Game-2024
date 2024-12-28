using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enemy AI for gun with legs
/// </summary>
public class GunWithLegs : EnemyStateManager
{
    [SerializeField] protected Transform gunKick;
    [SerializeField] protected Transform gunShoot;
    [SerializeField] protected Transform rangeOrig;

    [SerializeField] protected Transform projectile;

    // Check for kick collision and do damage
    protected void OnKickEvent()
    {
        GenerateDamageFrame(gunKick.position, gunKick.lossyScale.x, gunKick.lossyScale.y, 1.0f);
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
