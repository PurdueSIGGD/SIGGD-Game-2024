using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BleedingEdge : Skill
{
    [SerializeField] GameObject poisonDebuff;
    [SerializeField] float duration;
    [SerializeField] float interval;
    private static int pointIndex;

    private void OnEnable()
    {
        GameplayEventHolder.OnDamageDealt += ApplyDOT;
    }

    private void OnDisable()
    {
        GameplayEventHolder.OnDamageDealt -= ApplyDOT;
    }

    private void ApplyDOT(DamageContext context)
    {
        if (pointIndex > 0 && context.actionID == ActionID.SAMURAI_SPECIAL)
        {
            // creating a damage context to be used by the DOT
            DamageContext newContext = new DamageContext();
            newContext.attacker = context.attacker;
            newContext.victim = context.victim;
            newContext.actionTypes = new List<ActionType>() { ActionType.SKILL };
            newContext.damageTypes = new List<DamageType>() { DamageType.STATUS };

            // creating a new damage over time effect
            float totalDamageOverTime = context.damage * pointIndex * 0.2f;
            PoisonDebuff debuff = Instantiate(poisonDebuff, context.victim.transform).GetComponent<PoisonDebuff>();
            debuff.Init(newContext, totalDamageOverTime / duration * interval, duration, interval);
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
