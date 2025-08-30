using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class PlayerBuffStats : MonoBehaviour, IStatList
{
    [SerializeField]
    public StatManager.Stat[] statList;
    protected StatManager stats;

    [SerializeField] private int baseCritChance;

    [SerializeField] private GameObject pulseExplosionVFX;
    [SerializeField] private Color critDealtPulseColor;
    [SerializeField] private Color critTakenPulseColor;
    [SerializeField] private Sprite critIcon;
    [SerializeField] private Sprite damageResistanceIcon;



    private void OnEnable()
    {
        GameplayEventHolder.OnDamageFilter.Add(BoostAttackDamage);
        GameplayEventHolder.OnDamageFilter.Add(CheckCritChance);
        GameplayEventHolder.OnDamageDealt += CritReaction;
        GameplayEventHolder.OnDamageFilter.Add(ApplyDamageResistance);
        GameplayEventHolder.OnHealingFilter.Add(BoostHealing);
    }

    private void OnDisable()
    {
        GameplayEventHolder.OnDamageFilter.Remove(BoostAttackDamage);
        GameplayEventHolder.OnDamageFilter.Remove(CheckCritChance);
        GameplayEventHolder.OnDamageDealt -= CritReaction;
        GameplayEventHolder.OnDamageFilter.Remove(ApplyDamageResistance);
        GameplayEventHolder.OnHealingFilter.Remove(BoostHealing);
    }

    // Start is called before the first frame update
    void Start()
    {
        stats = GetComponent<StatManager>();
        stats.ModifyStat("Crit Chance", baseCritChance);
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    // Attack Damage
    private void BoostAttackDamage(ref DamageContext context)
    {
        if (!context.attacker || !context.attacker.CompareTag("Player")) return;

        // General
        if (stats.ComputeValue("General Attack Damage Boost") > 0f /*&& !context.actionTypes.Contains(ActionType.SKILL)*/)
        {
            context.damage *= stats.ComputeValue("General Attack Damage Boost");
        }

        // Light Attack
        if (stats.ComputeValue("Light Attack Damage Boost") > 1f && context.actionTypes.Contains(ActionType.LIGHT_ATTACK))
        {
            context.damage *= stats.ComputeValue("Light Attack Damage Boost");
        }

        // Heavy Attack
        if (stats.ComputeValue("Heavy Attack Damage Boost") > 1f && context.actionTypes.Contains(ActionType.HEAVY_ATTACK))
        {
            context.damage *= stats.ComputeValue("Heavy Attack Damage Boost");
        }

        // Stunned Enemy
        if (stats.ComputeValue("Stun Damage Boost") > 1f && context.victim.GetComponent<EnemyStateManager>() != null && context.victim.GetComponent<EnemyStateManager>().StunState.isStunned)
        {
            context.damage *= stats.ComputeValue("Stun Damage Boost");
        }
    }



    // Crit Chance
    private void CheckCritChance(ref DamageContext context)
    {
        if (!context.attacker || !context.attacker.CompareTag("Player")) return;
        if (context.damageTypes.Contains(DamageType.STATUS) || context.damageTypes.Contains(DamageType.ENVIRONMENTAL)) return;

        if (Random.Range(100f, 200f) > stats.ComputeValue("Crit Chance")) return;
        context.damage *= stats.ComputeValue("Crit Damage Boost");
        if (context.damageStrength == DamageStrength.MINOR) context.damageStrength = DamageStrength.LIGHT;
        else if (context.damageStrength == DamageStrength.LIGHT) context.damageStrength = DamageStrength.MODERATE;
        else if (context.damageStrength == DamageStrength.MODERATE) context.damageStrength = DamageStrength.HEAVY;
        context.isCriticalHit = true;
        context.icon = critIcon;
    }

    private void CritReaction(DamageContext context)
    {
        if (!context.isCriticalHit) return;
        GameObject critPulseVFX = Instantiate(pulseExplosionVFX, context.victim.transform.position, Quaternion.identity);
        Color critColor = (context.victim == PlayerID.instance.gameObject) ? critTakenPulseColor : critDealtPulseColor;
        critPulseVFX.GetComponent<RingExplosionHandler>().playRingExplosion(2f, critColor);
    }



    // Damage Resistance
    private void ApplyDamageResistance(ref DamageContext context)
    {
        if (!context.attacker || !context.victim.CompareTag("Player") || context.damage <= 0f) return;

        // General
        if (stats.ComputeValue("Damage Resistance") > 1f && !context.damageTypes.Contains(DamageType.ENVIRONMENTAL))
        {
            float damageReduction = stats.ComputeValue("Damage Resistance") - 1f;
            context.damage *= 1f - damageReduction;
            context.icon = damageResistanceIcon;
        }

        // Melee Damage
        if (stats.ComputeValue("Melee Damage Resistance") > 1f && context.damageTypes.Contains(DamageType.MELEE))
        {
            float damageReduction = stats.ComputeValue("Melee Damage Resistance") - 1f;
            context.damage *= 1f - damageReduction;
            context.icon = damageResistanceIcon;
        }

        // Projectile/Area Damage
        if (stats.ComputeValue("Projectile/Area Damage Resistance") > 1f && (context.damageTypes.Contains(DamageType.PROJECTILE) || context.damageTypes.Contains(DamageType.AREA)))
        {
            float damageReduction = stats.ComputeValue("Projectile/Area Damage Resistance") - 1f;
            context.damage *= 1f - damageReduction;
            context.icon = damageResistanceIcon;
        }

        // Elite Enemy Damage
        if (stats.ComputeValue("Elite Damage Resistance") > 1f && context.attacker.GetComponent<EnemyStateManager>() != null && context.attacker.GetComponent<EnemyStateManager>().isPinkAndProud)
        {
            float damageReduction = stats.ComputeValue("Elite Damage Resistance") - 1f;
            context.damage *= 1f - damageReduction;
            context.icon = damageResistanceIcon;
        }

        // Elite Enemy Damage
        if (stats.ComputeValue("Critical Health Damage Resistance") > 1f && context.attacker.GetComponent<EnemyStateManager>() != null && context.victim.GetComponent<PlayerHealth>().MortallyWounded)
        {
            float damageReduction = stats.ComputeValue("Critical Health Damage Resistance") - 1f;
            context.damage *= 1f - damageReduction;
            context.icon = damageResistanceIcon;
        }
    }



    private void BoostHealing(ref HealingContext context)
    {
        if (!context.healee.CompareTag("Player")) return;

        // General
        if (stats.ComputeValue("Healing Boost") > 1f)
        {
            context.healing *= stats.ComputeValue("Healing Boost");
        }
    }



    public StatManager.Stat[] GetStatList()
    {
        return statList;
    }

    public StatManager GetStats()
    {
        return stats;
    }
}
