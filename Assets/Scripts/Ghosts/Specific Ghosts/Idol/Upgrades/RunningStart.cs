using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningStart : Skill
{
    void Start()
    {
        
    }

    private void OnEnable()
    {
        if(skillPts > 0)
            GameplayEventHolder.OnAbilityUsed += ApplyBuff;
    }

    private void OnDisable()
    {
        if (skillPts > 0)
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
    }

    public override void ClearPointsTrigger()
    {
    }

    public override void RemovePointTrigger()
    {
    }
}
