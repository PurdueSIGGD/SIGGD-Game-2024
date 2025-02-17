using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class StunMeter : MonoBehaviour, IDamageable, IStatList
{
    [SerializeField] public StatManager.Stat[] statList;

    private float currentStun;
    private StatManager stats;
    private EnemyStateManager esm;

    void Start()
    {
        stats = GetComponent<StatManager>();
        currentStun = stats.ComputeValue("Stun Threshold");
        esm = GetComponent<EnemyStateManager>();
    }

    public float Damage(DamageContext context, GameObject attacker)
    {
        float stun = (float)StunLookUpTable.table[context.damageStrength];
        currentStun -= stun;

        if (currentStun <= 0)
        {
            esm.Stun(context, 2f);
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
