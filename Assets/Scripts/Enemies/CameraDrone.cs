using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDrone : EnemyStateManager
{
    [SerializeField] protected Transform alarmTrigger;
    [SerializeField] protected GameObject enemyToSummon;
    //protected override ActionPool GenerateActionPool()
    //{
    //    Action callAlarm = new(alarmTrigger, 5.0f, 1f, "Call Alarm");

    //    Action move = new(null, 0.0f, 0.0f, "move");
    //    Action idle = new(null, 0.0f, 0.0f, "idle");

    //    return new ActionPool(new List<Action> { callAlarm }, move, idle);
    //}

    /// <summary>
    /// Summons an enemy
    /// </summary>
    protected void OnCallAlarm()
    {
#if DEBUG
        GenerateDamageFrame(alarmTrigger.position, alarmTrigger.lossyScale.x, alarmTrigger.lossyScale.y, 0);
#endif
        Vector3 dest = transform.position + new Vector3(transform.right.x*transform.lossyScale.x, -transform.lossyScale.y, 0);
        Instantiate(enemyToSummon, dest, transform.rotation);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireCube(alarmTrigger.position, alarmTrigger.lossyScale);
    }
}
