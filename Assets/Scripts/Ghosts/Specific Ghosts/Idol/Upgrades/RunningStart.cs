using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningStart : Skill
{
    private GameObject player;

    void Start()
    {
        player = PlayerID.instance.gameObject;
        GameplayEventHolder.OnAbilityUsed += ApplyBuff;
    }

    private void ApplyBuff(ActionContext context)
    {
        if (context.actionID == ActionID.IDOL_SPECIAL)
        {
            // attempt to grab the Idol passive script
            IdolPassive passive = player.GetComponent<IdolPassive>();
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
