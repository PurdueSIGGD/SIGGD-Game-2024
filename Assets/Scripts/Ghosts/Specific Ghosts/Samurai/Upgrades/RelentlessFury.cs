//#define DEBUG_LOG

using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

/// <summary>
/// Relentless Fury skill for Akihito
/// </summary>
public class RelentlessFury : Skill
{
    private SamuraiManager samuraiManager;
    private static int pointIndex;


    private void OnEnable()
    {
        samuraiManager = gameObject.GetComponent<SamuraiManager>();

        GameplayEventHolder.OnDamageDealt += BuffLightAttack;
#if DEBUG_LOG
        Debug.Log("Relentless Fury enabled");
#endif

    }

    private void OnDisable()
    {
        GameplayEventHolder.OnDamageDealt -= BuffLightAttack;
#if DEBUG_LOG
        Debug.Log("Relentless Fury disabled");
#endif
    }


    /// <summary>
    /// Calculate bonus damage per wrath percent
    /// Returns percent increase according to skill points
    /// Bonus Damage per Wrath Percent: +0.15% | +0.25% | +0.35% | +0.45%
    /// </summary>
    private float CalculateDamageMultiplier()
    {
        float multiplier = 1.0f;
        float wrathPercent = samuraiManager.basic.GetWrathPercent();

        if (pointIndex == 0 || wrathPercent <= 0)
        {
            return multiplier;
        }

        // Skill points

        float multiplierAdd = 0.15f + (pointIndex - 1) * 0.1f;
#if DEBUG_LOG
        Debug.Log("Multiplier per Wrath percent: " + multiplierAdd +"%");
#endif

        // Multiply by wrath percent

        multiplier += wrathPercent * multiplierAdd;
        
        return multiplier;
    }

    /// <summary>
    /// Buffs damage for light attacks according to Wrath Percent.
    /// </summary>
    /// <param name="damageContext"></param>
    private void BuffLightAttack(DamageContext damageContext)
    {
        if (samuraiManager.selected && pointIndex > 0 &&
            samuraiManager.basic.GetWrathPercent() >= 0 &&
            damageContext.actionID == ActionID.PLAYER_LIGHT_ATTACK)
        {
            // buff damage
#if DEBUG_LOG
            Debug.Log("Skill points: " + pointIndex + "\nWrath %: " + samuraiManager.basic.GetWrathPercent());
#endif
            damageContext.damage *= CalculateDamageMultiplier();
#if DEBUG_LOG
            Debug.Log("Damage: " + damageContext.damage);
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
