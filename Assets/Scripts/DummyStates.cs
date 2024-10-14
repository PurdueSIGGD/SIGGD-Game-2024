using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyStates : EnemyStateManager
{
    protected override void IdleBehavior()
    {
        rb.velocity = Vector3.zero;
    }

    protected override void AttackBehavior()
    {
        if (attackCD <= 0)
        {
            Debug.Log("attack!");
            attackCD = 2.0f;
        }
        rb.velocity = Vector3.zero;
    }

    protected override bool InAttackRange()
    {
        Collider[] c = Physics.OverlapBox(meleeHit.transform.position, meleeHit.transform.lossyScale / 2, meleeHit.transform.rotation, LayerMask.GetMask("Player"));
        return c.Length > 0;
    }
}
