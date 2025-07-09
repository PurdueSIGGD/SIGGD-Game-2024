using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningStart : Skill
{
    private int pointindex;

    //void Start()
    //{
    //    GameplayEventHolder.OnAbilityUsed += ApplyBuff;
    //}

    private void OnEnable()
    {
        GameplayEventHolder.OnAbilityUsed += ApplyBuff;
    }

    private void OnDisable()
    {
        GameplayEventHolder.OnAbilityUsed -= ApplyBuff;
    }

    private void ApplyBuff(ActionContext context)
    {
        if (context.actionID == ActionID.IDOL_SPECIAL && pointindex > 0)
        {
            // attempt to grab the Idol passive script
            IdolPassive passive = GetComponent<IdolPassive>();
            if (passive)
            {
                passive.IncrementTempo(pointindex);
            }
            // Activate Feedback Loop manually because the clone doesn't spawn fast enough in the special script
            GetComponent<FeedbackLoop>().reduceCooldown(true);
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
