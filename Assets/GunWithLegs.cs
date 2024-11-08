using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunWithLegs : EnemyStateManager
{
    [SerializeField] protected Transform gunKick;
    [SerializeField] protected Transform gunShoot;
    [SerializeField] protected Transform rangeOrig;

    [SerializeField] protected Transform projectile;

    protected override ActionPool GenerateActionPool()
    {
        Action kick = new(gunKick, 4.0f, 3f, "Gun_W_Leg_Kick");
        Action shoot = new(gunShoot, 4.0f, 7f, "Gun_W_Leg_Shoot");

        Action move = new(null, 0.0f, 0.0f, "move");
        Action idle = new(null, 0.0f, 0.0f, "idle");

        return new ActionPool(new List<Action> { kick, shoot }, move, idle);
    }

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
