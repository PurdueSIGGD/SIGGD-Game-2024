using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PoliceChiefEnergySiphonSkill : Skill
{
    private StatManager stats;
    private int changeAmount = 0;
    private void Start()
    {
        GameplayEventHolder.OnDamageDealt += OnDmg;
        stats = GetComponent<StatManager>();
    }

    [SerializeField] float amountCooldownReduction = 0.0f;
    [SerializeField] float[] pointCooldowns = {0.0005f, 0.001f, 0.0015f, 0.002f};
    public override void AddPointTrigger()
    {
        amountCooldownReduction = pointCooldowns[GetPoints() - 1];
    }

    public override void ClearPointsTrigger()
    {
        amountCooldownReduction = 0.0f;
    }

    public override void RemovePointTrigger()
    {
    }

    void OnDmg(DamageContext context)
    {
        if (context.attacker == PlayerID.instance)
        {
            stats.ModifyStat("Special Cooldown", -Mathf.FloorToInt(amountCooldownReduction * 100f * context.damage));
            changeAmount -= Mathf.FloorToInt(amountCooldownReduction * 100f * context.damage);
        }
    }

    void OnAbilityUse(ActionID action)
    {
        if(action == ActionID.POLICE_CHIEF_SPECIAL)
        {
            stats.ModifyStat("Special Cooldown", -changeAmount);
            changeAmount = 0;
        }
    }
}
