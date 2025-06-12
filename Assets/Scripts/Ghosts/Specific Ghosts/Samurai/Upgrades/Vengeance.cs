using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vengeance : Skill {

    private static int pointIndex = 0;

    private void OnEnable()
    {
        GameplayEventHolder.OnDamageDealt += ApplyBonusWrath;
    }

    private void OnDisable()
    {
        GameplayEventHolder.OnDamageDealt -= ApplyBonusWrath;
    }

    private void ApplyBonusWrath(DamageContext context)
    {
        if (pointIndex > 0 && context.actionID == )
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
