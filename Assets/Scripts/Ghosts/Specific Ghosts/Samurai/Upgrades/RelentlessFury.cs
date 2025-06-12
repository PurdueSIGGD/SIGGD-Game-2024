using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

/// <summary>
/// Relentless Fury skill for Akihito
/// </summary>
public class RelentlessFury : Skill
{
    private WrathHeavyAttack basic;
    private static int pointIndex;

    private void OnEnable()
    {
        SamuraiManager samuraiManager = gameObject.GetComponent<SamuraiManager>();
        basic = samuraiManager.basic;

        GameplayEventHolder.OnDamageDealt += BuffLightAttack;
    }

    private void OnDisable()
    {
        GameplayEventHolder.OnDamageDealt -= BuffLightAttack;
    }


    /// <summary>
    /// Calculate bonus damage per wrath percent
    /// Returns percent increase according to skill points
    /// Bonus Damage per Wrath Percent: +0.15% | +0.25% | +0.35% | +0.45%
    /// </summary>
    private float CalculateDamageMultiplier()
    {
        float multiplier = 1.0f;

        // Skill points

        if (pointIndex > 0) {
            multiplier = 1f + 0.15f + (pointIndex - 1) * 0.1f;
        }
        else
        {
            return multiplier;
        }

        // Multiply by wrath percent

        multiplier *= basic.GetWrathPercent();
        
        return multiplier;
    }

    private void BuffLightAttack(DamageContext damageContext)
    {
        if (pointIndex > 0 && damageContext.attacker.CompareTag("Player"))
        {
            // buff damage

            damageContext.damage *= CalculateDamageMultiplier();
            Debug.Log(damageContext.damage + " H UZZAH " + pointIndex);
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
