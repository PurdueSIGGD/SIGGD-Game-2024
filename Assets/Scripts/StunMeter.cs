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
public class StunMeter : MonoBehaviour, IStatList
{
    [SerializeField] public StatManager.Stat[] statList;

    public float currentStun;
    public float maxStun;
    private StatManager stats;
    private EnemyStateManager esm;

    void OnEnable()
    {
        GameplayEventHolder.OnDamageDealt += Damage;
    }

    void OnDisable()
    {
        GameplayEventHolder.OnDamageDealt -= Damage;
    }

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
            DamageStrength.MINOR => 20f,
            DamageStrength.LIGHT => 40f,
            DamageStrength.MODERATE => 80f,
            DamageStrength.HEAVY => 120f,
            DamageStrength.DEVASTATING => 400f,
            _ => 0f,
        };
    }

    public void Damage(DamageContext context)
    {
        if (context.victim != gameObject) return;
        float stun = ComputeStunBuildUp(context.damageStrength);
        currentStun -= stun;

        if (currentStun <= 0)
        {
            currentStun = maxStun = stats.ComputeValue("Stun Threshold");
            esm.Stun(context, 0.6f);
        }
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
