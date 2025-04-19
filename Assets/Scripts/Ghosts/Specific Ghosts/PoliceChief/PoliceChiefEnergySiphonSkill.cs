using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PoliceChiefEnergySiphonSkill : Skill
{
    private StatManager stats;
    [SerializeField] private PoliceChiefManager policeChiefManager;
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
            policeChiefManager.setSpecialCooldown(Mathf.Max(0, policeChiefManager.getSpecialCooldown() - stats.ComputeValue("Special Cooldown") * amountCooldownReduction * context.damage));
        }
    }
}
