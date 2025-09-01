using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HonedStrike : Skill
{
    private SamuraiManager manager;
    [HideInInspector] public bool buffApplied;

    [SerializeField]
    List<float> values = new List<float>
    {
        0f, 20f, 40f, 60f, 80f
    };
    private int pointIndex;

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
        if (pointIndex > 0 && context.actionID == ActionID.SAMURAI_SPECIAL && 
            !buffApplied && (context.extraContext != null && context.extraContext.Equals("Parry Success")))
        {
            manager.GetStats().ModifyStat("Heavy Attack Minimum Travel Distance", Mathf.CeilToInt(values[pointIndex]));
            manager.GetStats().ModifyStat("Heavy Attack Maximum Travel Distance", Mathf.CeilToInt(values[pointIndex]));
            manager.GetStats().ModifyStat("Heavy Attack Travel Speed", Mathf.CeilToInt(values[pointIndex]));
            manager.GetStats().ModifyStat("Heavy Charge Up Time", -(Mathf.CeilToInt(values[pointIndex])));

            buffApplied = true;
        }
    }

    private void RemoveDashBuff(ActionContext context)
    {
        if (pointIndex > 0 && buffApplied && context.actionID == ActionID.SAMURAI_BASIC)
        {
            manager.GetStats().ModifyStat("Heavy Attack Minimum Travel Distance", -(Mathf.CeilToInt(values[pointIndex])));
            manager.GetStats().ModifyStat("Heavy Attack Maximum Travel Distance", -(Mathf.CeilToInt(values[pointIndex])));
            manager.GetStats().ModifyStat("Heavy Attack Travel Speed", -(Mathf.CeilToInt(values[pointIndex])));
            manager.GetStats().ModifyStat("Heavy Charge Up Time", Mathf.CeilToInt(values[pointIndex]));

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
