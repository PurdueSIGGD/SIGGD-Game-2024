#define DEBUG_LOG
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEditor.Rendering.LookDev;
using UnityEngine;

/// <summary>
/// While Radiant Shield is at full health, a portion of damage that 
/// King Aegis or his shield takes is reflected to his attacker instead.
/// Damage Reflected: 8% | 16% | 24% | 32%
/// </summary>
public class ShieldOfThorns : Skill
{

    private static int pointIndex = 0;
    private KingManager manager;
    private KingBasic basic;
    private StatManager stats;

    private void OnEnable()
    {
        manager = gameObject.GetComponent<KingManager>();
        stats = manager.GetStats();

        // FIXME
        GameplayEventHolder.OnDamageFilter.Insert(0, ReflectDamage);

    }

    private void OnDisable()
    {
        GameplayEventHolder.OnDamageFilter.Remove(ReflectDamage);

    }

    /// <summary>
    /// If player/shield was damaged and Shield of Thorns is in effect, call the
    /// reflect damage method
    /// </summary>
    /// <param name="context"></param>
    public void ReflectDamage(ref DamageContext context)
    {

#if DEBUG_LOG
        Debug.Log("Shield of Thorns: " + context.victim + " hurt " + context.damage + " by " + context.attacker);
        Debug.Log("Shield of Thorns: Shield health " + manager.currentShieldHealth + ", skill points " + GetPoints());
#endif

        // Conditions

        if (GetPoints() <= 0 || !manager.selected)
        {
            return;
        }

        if (!context.attacker.CompareTag("Enemy") || !context.victim.CompareTag("Player"))
        {
            return;
        }

        if (manager.currentShieldHealth < stats.ComputeValue("Shield Max Health"))
        {
            return;
        }

        // Calculate damage that will be reflected

        float damageReflected = 0.08f * GetPoints() * context.damage;

        // Reduce damage

        DamageContext newContext = new DamageContext();
        newContext.damage = damageReflected;

        context.attacker.GetComponent<Health>().Damage(newContext, context.victim);

#if DEBUG_LOG
        Debug.Log("Shield of Thorns: Reflected " + damageReflected*100 + "% percent damage to " + context.attacker);
#endif
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
