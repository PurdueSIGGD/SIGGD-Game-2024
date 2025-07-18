//#define DEBUG_LOG
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Relentless Fury skill for Akihito
/// </summary>
public class RelentlessFury : Skill
{

    // **********
    //
    // NOTE: THERE IS SOME SPOOKY MAGIC SHIT GOING ON HERE THAT IS BREAKING THIS AND I DON'T KNOW WHAT'S HAPPENING BUT I DON'T HAVE TIME TO FIX IT
    //
    // **********

    private SamuraiManager samuraiManager;

    [SerializeField] private DamageContext boostedDamageContext;

    [SerializeField]
    List<float> values = new List<float>
    {
        0f, 0.15f, 0.25f, 0.35f, 0.45f
    };
    private int pointIndex;


    private void OnEnable()
    {
        samuraiManager = gameObject.GetComponent<SamuraiManager>();

        GameplayEventHolder.OnDamageFilter.Add(BuffLightAttack);
#if DEBUG_LOG
        Debug.Log("Relentless Fury enabled");
#endif

    }

    private void OnDisable()
    {
        GameplayEventHolder.OnDamageFilter.Add(BuffLightAttack);
#if DEBUG_LOG
        Debug.Log("Relentless Fury disabled");
#endif
    }

    private void Update()
    {
        boostedDamageContext.damage = 20f * values[pointIndex] * samuraiManager.wrathPercent; // GOD DAMMIT THIS WAS TEMP BUT I AT LEAST THOUGHT IT WOULD WORK
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
    private void BuffLightAttack(ref DamageContext damageContext)
    {
        /*if (samuraiManager.selected && pointIndex > 0 &&
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
        }*/

        /*
        if (pointIndex > 0 && samuraiManager.selected && samuraiManager.wrathPercent > 0f && damageContext.actionID == ActionID.PLAYER_LIGHT_ATTACK)
        {
            damageContext.victim.GetComponent<Health>().Damage(boostedDamageContext, damageContext.attacker);
        }
        */
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
