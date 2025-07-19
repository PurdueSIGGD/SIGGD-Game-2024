using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PoliceChiefEnergySiphonSkill : Skill
{
    private StatManager stats;
    private PoliceChiefManager policeChiefManager;
    [SerializeField] float[] values = {0f, 2f, 4f, 6f, 8f };
    private int pointIndex = 0;

    private float accumulatedCooldownReduction;

    private void Start()
    {
        stats = GetComponent<StatManager>();
        policeChiefManager = GetComponent<PoliceChiefManager>();
        accumulatedCooldownReduction = 0f;
    }

    private void Update()
    {
        if (accumulatedCooldownReduction > 0f && policeChiefManager.getSpecialCooldown() > 0f)
        {
            policeChiefManager.setSpecialCooldown(policeChiefManager.getSpecialCooldown() - accumulatedCooldownReduction);
            accumulatedCooldownReduction = 0f;
        }
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

    void OnDmg(DamageContext context)
    {
        if (context.attacker == PlayerID.instance.gameObject && context.actionID == ActionID.POLICE_CHIEF_SPECIAL)
        {
            if (pointIndex <= 0) return;
            float cooldownReduction = stats.ComputeValue("Special Cooldown") * (values[pointIndex] / 100f);
            if (policeChiefManager.getSpecialCooldown() > 0)
            {
                policeChiefManager.setSpecialCooldown(policeChiefManager.getSpecialCooldown() - cooldownReduction);
            }
            else
            {
                accumulatedCooldownReduction += cooldownReduction;
            }
        }
    }
}
