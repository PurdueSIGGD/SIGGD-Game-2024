using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDrone : EnemyStateManager
{
    [Header("Call Reinforcement")]
    [SerializeField] protected Transform alarmTrigger;
    [SerializeField] protected GameObject enemyToSummon;

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
