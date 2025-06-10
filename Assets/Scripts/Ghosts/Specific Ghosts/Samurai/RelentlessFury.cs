using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

/// <summary>
/// Relentless Fury skill for Akihito
/// </summary>
public class RelentlessFury : Skill
{
    [HideInInspector] public WrathHeavyAttack passive;
    [HideInInspector] public GameObject player;

    //private static int pointIndex;

    private void Start()
    {
        player = GameObject.Find("Player");
        passive = PlayerID.instance.GetComponent<WrathHeavyAttack>();
    }


    /// <summary>
    /// Calculate bonus damage per wrath percent
    /// Returns percent increase according to skill points
    /// Bonus Damage per Wrath Percent: +0.15% | +0.25% | +0.35% | +0.45%
    /// </summary>
    private float CalculateDamageMultiplier()
    {
        float multiplier = 1.0f;
        int points = GetPoints();

        // Skill points

        if (points > 0) {
            multiplier = 1f + 0.15f + (GetPoints() - 1) * 0.1f;
        }
        else
        {
            return multiplier;
        }

        // Multiply by wrath percent

        multiplier *= passive.getWrathPercent();
        
        return multiplier;
    }

    private void BuffLightAttack(ref DamageContext damageContext)
    {
        if (GetPoints() > 0 && damageContext.attacker.CompareTag("Player"))
        {
            // buff damage

            damageContext.damage *= CalculateDamageMultiplier();
        }
    }

    public override void AddPointTrigger()
    {
    }

    public override void ClearPointsTrigger()
    {
    }

    public override void RemovePointTrigger()
    {
    }

}
