using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunWithLegs : EnemyStateManager
{
    [SerializeField] protected Transform gunKick;
    [SerializeField] protected Transform gunShoot;
    [SerializeField] protected Transform rangeOrig;

    protected override ActionPool GenerateActionPool()
    {
        Action kick = new(gunKick, 4.0f, 3f, "EMPTY");
        Action shoot = new(gunShoot, 4.0f, 7f, "EMPTY");

        Action move = new(null, 0.0f, 0.0f, "kick");
        Action idle = new(null, 0.0f, 0.0f, "shoot");

        return new ActionPool(new List<Action> { kick, shoot }, move, idle);
    }


    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireCube(gunKick.position, gunKick.lossyScale);
        Gizmos.DrawWireCube(gunShoot.position, gunShoot.lossyScale);
        Gizmos.DrawWireSphere(rangeOrig.position, 0.1f);
    }
}
