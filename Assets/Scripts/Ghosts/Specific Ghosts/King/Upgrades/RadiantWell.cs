using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadientWell : Skill
{
    [SerializeField] GameObject wellFX;
    private int pointindex;

    private void Start()
    {
        GameplayEventHolder.OnAbilityUsed += SummonWell;
    }

    private void SummonWell(ActionContext context)
    {
        if (context.actionID == ActionID.KING_SPECIAL && GetPoints() > 0)
        {
            
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
