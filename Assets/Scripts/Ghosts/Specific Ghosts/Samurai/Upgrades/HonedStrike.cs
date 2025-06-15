using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HonedStrike : Skill
{
    private SamuraiManager manager;
    private bool buffApplied;
    private static int pointIndex;

    void Start()
    {
        manager = GetComponent<SamuraiManager>();    
    }

    private void OnEnable()
    {
        GameplayEventHolder.OnAbilityUsed += BuffDashSpeed;
        GameplayEventHolder.OnAbilityUsed += RemoveDashBuff;
    }

    private void OnDisable()
    {
        GameplayEventHolder.OnAbilityUsed -= BuffDashSpeed;
        GameplayEventHolder.OnAbilityUsed -= RemoveDashBuff;
    }

    private void BuffDashSpeed(ActionContext context)
    {
        Debug.Log("hey " + context.actionID);
        if (pointIndex > 0 && context.actionID == ActionID.SAMURAI_SPECIAL && 
            !buffApplied && context.extraContext.Equals("Parry Success"))
        {
            manager.GetStats().ModifyStat("Heavy Attack Minimum Travel Distance", 20 * pointIndex);
            manager.GetStats().ModifyStat("Heavy Attack Maximum Travel Distance", 20 * pointIndex);
            manager.GetStats().ModifyStat("Heavy Attack Travel Speed", 20 * pointIndex);

            buffApplied = true;
        }
    }

    private void RemoveDashBuff(ActionContext context)
    {
        if (pointIndex > 0 && buffApplied && context.actionID == ActionID.SAMURAI_BASIC)
        {
            manager.GetStats().ModifyStat("Heavy Attack Minimum Travel Distance", -20 * pointIndex);
            manager.GetStats().ModifyStat("Heavy Attack Maximum Travel Distance", -20 * pointIndex);
            manager.GetStats().ModifyStat("Heavy Attack Travel Speed", -20 * pointIndex);

            buffApplied = false;
        }
    }

    public override void AddPointTrigger()
    {
        pointIndex = GetPoints();
    }

    public override void ClearPointsTrigger()
    {
        pointIndex = GetPoints();
    }

    public override void RemovePointTrigger()
    {
        pointIndex = GetPoints();
    }
}
