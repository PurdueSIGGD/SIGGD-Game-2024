using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HonedStrike : Skill
{
    private SamuraiManager manager;
    private static int pointIndex;

    void Start()
    {
        manager = GetComponent<SamuraiManager>();    
    }

    private void OnEnable()
    {
        GameplayEventHolder.OnAbilityUsed += BuffDashSpeed;
    }

    private void OnDisable()
    {
        GameplayEventHolder.OnAbilityUsed -= BuffDashSpeed;
    }

    private void BuffDashSpeed(ActionContext context)
    {
        if (pointIndex > 0 && context.actionID == ActionID.SAMURAI_SPECIAL && context.extraContext.Equals("Parry Success"))
        {
            int modifier = 100 + 20 * pointIndex;

            manager.GetStats().SetStat("Heavy Attack Minimum Travel Distance", modifier);
            manager.GetStats().SetStat("Heavy Attack Maximum Travel Distance", modifier);
            manager.GetStats().SetStat("Heavy Attack Travel Speed", modifier);
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
