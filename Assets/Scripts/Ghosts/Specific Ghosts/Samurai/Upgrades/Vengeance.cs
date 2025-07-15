//#define DEBUG_LOG

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vengeance : Skill {

    [SerializeField]
    List<float> values = new List<float>
    {
        0f, 0.5f, 1.0f, 1.5f, 2.0f
    };
    private int pointIndex = 0;

    public float CalculateBoostedWrathGain(DamageContext context, float wrathGained)
    {
        if (context.actionID != ActionID.SAMURAI_SPECIAL || pointIndex <= 0) return wrathGained;
        return wrathGained + (wrathGained * values[pointIndex]);
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
