using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

/// <summary>
/// Stun meter for enemies. When stun meter has been depleted, the enemy will
/// be flinched/stunned for a breif duration.
/// </summary>
public class StunMeter : MonoBehaviour, IDamageable, IStatList
{
    [SerializeField] public StatManager.Stat[] statList;

    public float currentStun;
    public float maxStun;
    private StatManager stats;
    private EnemyStateManager esm;

    void Start()
    {
        stats = GetComponent<StatManager>();
        currentStun = maxStun = stats.ComputeValue("Stun Threshold");
        esm = GetComponent<EnemyStateManager>();
    }

    public float ComputeStunBuildUp(DamageStrength strength)
    {
        return strength switch
        {
            DamageStrength.MEAGER => 0f,
            DamageStrength.LIGHT => 35f,
            DamageStrength.MODERATE => 70f,
            DamageStrength.HEAVY => 125f,
            DamageStrength.DEVASTATING => 250f,
            _ => 0f,
        };
    }

    public float Damage(DamageContext context, GameObject attacker)
    {
        float stun = ComputeStunBuildUp(context.damageStrength);
        currentStun -= stun;

        if (currentStun <= 0)
        {
            currentStun = maxStun = stats.ComputeValue("Stun Threshold");
            esm.Stun(context, 0.4f);
        }

        return stun;
    }

    public float Heal(HealingContext context, GameObject healer)
    {
        // May implement later for stun decay
        return 0f;
    }

    public StatManager.Stat[] GetStatList()
    {
        return statList;
    }
}
