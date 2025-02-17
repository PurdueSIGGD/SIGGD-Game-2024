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

    private float currentStun;
    private float maxStun;
    private StatManager stats;
    private EnemyStateManager esm;

    void Start()
    {
        stats = GetComponent<StatManager>();
        currentStun = maxStun = stats.ComputeValue("Stun Threshold");
        esm = GetComponent<EnemyStateManager>();
    }

    public float Damage(DamageContext context, GameObject attacker)
    {
        float stun = (float)StunLookUpTable.table[context.damageStrength];
        currentStun -= stun;

        if (currentStun <= 0)
        {
            currentStun = maxStun;
            esm.Stun(context, 0.5f);
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
