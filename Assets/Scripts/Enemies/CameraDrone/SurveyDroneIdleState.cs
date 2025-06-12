using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurveyDroneIdleState : IdleState
{
    public override void EnterState(EnemyStateManager enemy)
    {
        base.EnterState(enemy);
        enemy.pool.SetActionReady("CallAlarm", false);
        enemy.pool.SetActionReady("Move to Spawn Point", true);
    }
}
