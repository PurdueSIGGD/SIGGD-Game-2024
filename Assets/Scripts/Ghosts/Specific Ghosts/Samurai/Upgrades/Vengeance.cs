//#define DEBUG_LOG

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vengeance : Skill {


    private int pointIndex = 0;

    private SamuraiManager samuraiManager;

    private void OnEnable()
    {
        GameplayEventHolder.OnDamageDealt += ApplyBonusWrath;

        samuraiManager = gameObject.GetComponent<SamuraiManager>();

    }

    private void OnDisable()
    {
        GameplayEventHolder.OnDamageDealt -= ApplyBonusWrath;
    }

    private void ApplyBonusWrath(DamageContext context)
    {
        if (pointIndex > 0 && context.actionID == ActionID.SAMURAI_SPECIAL)
        {
            // Grant bonus wrath

            float bonusWrath = pointIndex * 0.2f;
            float prevWrath = samuraiManager.basic.GetWrathPercent();
            float newWrath = Mathf.Min(1.0f, bonusWrath + prevWrath);

            samuraiManager.basic.SetWrathPercent(newWrath);

#if DEBUG_LOG
            Debug.Log("Old Wrath: " + prevWrath + " New Wrath: " + newWrath + " Skill Pts: " + pointIndex);
#endif

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
