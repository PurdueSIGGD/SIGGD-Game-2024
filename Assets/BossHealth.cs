using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : Health
{
    [SerializeField] EnemyStateManager manager;

    // cannot take damage until ai is enabled
    public override float Damage(DamageContext context, GameObject attacker)
    {
        if (!manager.enabled) return 0;
        return base.Damage(context, attacker);
    }

    public override float NoContextDamage(DamageContext context, GameObject attacker)
    {
        if (!manager.enabled) return 0;
        return base.NoContextDamage(context, attacker);
    }
}
