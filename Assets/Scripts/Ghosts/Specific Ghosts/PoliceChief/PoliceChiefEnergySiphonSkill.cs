using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PoliceChiefEnergySiphonSkill : Skill
{
    private StatManager stats;
    private PoliceChiefManager policeChiefManager;
    private static float amountCooldownReduction = 0.0f;
    [SerializeField] float[] pointCooldowns = { 0.0005f, 0.001f, 0.0015f, 0.002f };
    private void Start()
    {
        /*if(GetPoints() > 0)
        {
            amountCooldownReduction = pointCooldowns[GetPoints() - 1];
        }
        else
        {
            amountCooldownReduction = 0.0f;
        }*/

        stats = GetComponent<StatManager>();
        policeChiefManager = GetComponent<PoliceChiefManager>();
    }

    private void OnEnable()
    {
        GameplayEventHolder.OnDamageDealt += OnDmg;
    }

    private void OnDisable()
    {
        GameplayEventHolder.OnDamageDealt -= OnDmg;
    }

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
        if (context.attacker == PlayerID.instance.gameObject && context.actionID == ActionID.POLICE_CHIEF_BASIC)
        {
            policeChiefManager.setSpecialCooldown(Mathf.Max(0, policeChiefManager.getSpecialCooldown() - (stats.ComputeValue("Special Cooldown") * amountCooldownReduction * context.damage)));
        }
    }
}
