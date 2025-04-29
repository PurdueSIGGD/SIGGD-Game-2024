using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningStart : Skill
{
    private static int pointindex;
    void Start()
    {
    }

    private void OnEnable()
    {
        if(pointindex > 0)
            GameplayEventHolder.OnAbilityUsed += ApplyBuff;
    }

    private void OnDisable()
    {
        if (pointindex > 0)
            GameplayEventHolder.OnAbilityUsed -= ApplyBuff;
    }

    private void ApplyBuff(ActionContext context)
    {
        if (context.actionID == ActionID.IDOL_SPECIAL)
        {
            // attempt to grab the Idol passive script
            IdolPassive passive = GetComponent<IdolPassive>();
            if (passive)
            {
                passive.IncrementTempo(skillPts);
            }
        }
    }


    public override void AddPointTrigger()
    {
        pointindex = GetPoints();
    }

    public override void ClearPointsTrigger()
    {
        pointindex = GetPoints();
    }

    public override void RemovePointTrigger()
    {
        pointindex = GetPoints();
    }
}
