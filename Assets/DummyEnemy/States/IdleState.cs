using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class IdleState : EnemyStates
{
    private float? countDown = null;

    public override void EnterState(EnemyStateManager enemy)
    {
        Rigidbody rb = enemy.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
    }

    public void EnterState(EnemyStateManager enemy, float countDown)
    {
        EnterState(enemy);
        this.countDown = countDown;
    }

    public override void UpdateState(EnemyStateManager enemy)
    {
        if (countDown != null)
        {
            countDown -= Time.deltaTime;
            if (countDown <= 0)
            {
                enemy.SwitchState(enemy.AggroState);
            }
        }
        else if (enemy.HasLineOfSight(false))
        {
            enemy.SwitchState(enemy.AggroState);
        }
    }
}
