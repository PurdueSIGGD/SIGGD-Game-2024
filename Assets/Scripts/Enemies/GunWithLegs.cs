using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enemy AI for gun with legs
/// </summary>
public class GunWithLegs : EnemyStateManager
{
    [Header("Kick")]
    //[SerializeField] protected Transform gunKick;
    //[SerializeField] protected DamageContext kickDamage;

    [Header("Shoot")]
    [SerializeField] protected Transform gunShoot;
    [SerializeField] protected Transform rangeOrig;
    [SerializeField] protected Transform projectile;

    protected Vector3 target; // used to track player location for shooting

    // Check for kick collision and do damage
    //protected void OnKickEvent()
    //{
    //    GenerateDamageFrame(gunKick.position, gunKick.lossyScale.x, gunKick.lossyScale.y, kickDamage, gameObject);
    //}

    // deprecated, as GREG will now only shoot forward, instead of at player's exact position
    protected void OnShootEvent1()
    {
        target = player.position;
    }

    // Instantiate projectile prefab and push self back
    protected void OnShootEvent2()
    {
        Instantiate(projectile, rangeOrig.position, transform.rotation).GetComponent<EnemyProjectile>().Init(gameObject, rangeOrig.position + transform.right);

        rb.AddForce(transform.right * -3, ForceMode2D.Impulse);
    }


    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireCube(gunShoot.position, gunShoot.lossyScale);
        Gizmos.DrawWireSphere(rangeOrig.position, 0.1f);
    }
}
