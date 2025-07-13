using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BleedingEdge : Skill
{
    [SerializeField] GameObject poisonDebuff;
    [SerializeField] DamageContext poisonDamage;
    [SerializeField] float duration;
    [SerializeField] float interval;

    [SerializeField]
    List<float> values = new List<float>
    {
        0f, 6f, 12f, 18f, 24f
    };
    private int pointIndex;

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
        if (pointIndex > 0 &&
            (context.actionID == ActionID.SAMURAI_SPECIAL || (context.actionID == ActionID.SAMURAI_BASIC && context.extraContext.Equals("Full Charge"))) &&
            !context.damageTypes.Contains(DamageType.STATUS))
        {
            poisonDamage.attacker = context.attacker;
            poisonDamage.victim = context.victim;
            //poisonDamage.actionID = context.actionID;
            //poisonDamage.actionID = ActionID.SAMURAI_SPECIAL; // TODO: RAAAAAH WHAT THE FUCK CAN U JUST WORK PLEASE AAAAAAAAH (I forgot to take my meds so I'm done for the day I think)

            // creating a new damage over time effect
            float totalDamageOverTime = values[pointIndex];

            PoisonDebuff debuff = context.victim.transform.GetComponentInChildren<PoisonDebuff>();
            if (debuff != null)
            {
                //debuff.damageContext.actionID = context.actionID;
                debuff.duration = duration;
                return;
            }
            debuff = Instantiate(poisonDebuff, context.victim.transform).GetComponent<PoisonDebuff>();
            debuff.Init(poisonDamage, totalDamageOverTime / duration, duration, interval);
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
