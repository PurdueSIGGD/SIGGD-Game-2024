using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DummyStates : EnemyStateManager
{
    public Transform meleeHit;
    public Transform rangeHit;

    protected override ActionPool GenerateActionPool()
    {
        Action dummySlash = new(meleeHit, 2.0f, -1f, "HeroKnight_Attack1");
        Action dummyShoot = new(rangeHit, 3.0f, 1f, "HeroKnight_Block");
        Action move = new(null, 0.0f, 0.0f, "HeroKnight_Run");
        Action idle = new(null, 0.0f, 0.0f, "HeroKnight_Idle");

        return new ActionPool(new List<Action> { dummySlash, dummyShoot }, move, idle);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireCube(meleeHit.position, meleeHit.lossyScale);
        Gizmos.DrawWireCube(rangeHit.position, rangeHit.lossyScale);
    }
}
