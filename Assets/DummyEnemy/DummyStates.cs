using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DummyStates : EnemyStateManager
{
    public Transform meleeHit;
    public Transform rangeHit;

    public new bool InAttackRange()
    {
        Collider[] c = Physics.OverlapBox(meleeHit.position, meleeHit.lossyScale / 2, meleeHit.rotation, LayerMask.GetMask("Player"));
        return c.Length > 0;
    }

    protected override ActionPool GenerateActionPool()
    {
        Action dummySlash = new(meleeHit, 2.0f, -1f);
        Action dummyShoot = new(rangeHit, 3.0f, 1f);

        return new ActionPool(new List<Action> { dummySlash, dummyShoot });
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireCube(meleeHit.position, meleeHit.lossyScale);
        Gizmos.DrawWireCube(rangeHit.position, rangeHit.lossyScale);
    }
}
