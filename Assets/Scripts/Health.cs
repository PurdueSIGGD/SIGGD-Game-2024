using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static GameplayEventHolder;

[DisallowMultipleComponent]
public class Health : MonoBehaviour, IDamageable, IStatList
{
    [SerializeField]
    public StatManager.Stat[] statList;

    [NonSerialized] public float currentHealth; // Current health of player
    [NonSerialized] public bool isAlive = true; // Checks if player is still alive
    [NonSerialized] private float damageResistance = 0.0f; // 0 to 1, Multiply damage by (1 - resistance) 

    protected StatManager stats;
    [SerializeField] private string deathLevel;

    public delegate void DamageFilters(DamageContext context);


    void Start()
    {
        stats = GetComponent<StatManager>();
        currentHealth = stats.ComputeValue("Max Health");
    }

    public float Damage(DamageContext context, GameObject attacker)
    {
        // Configure damage context
        context.attacker = attacker;
        context.victim = gameObject;
        context.trueDamage = context.damage;
        context.damage = Mathf.Clamp(context.damage, 0f, currentHealth);
        context.invokingScript = this;

        // potential alternative implementation of the foreach header:
        //List<DamageFilterEvent> onDamageFilter = GameplayEventHolder.OnDamageFilter.ToArray(...idk something here);
        //foreach (GameplayEventHolder.DamageFilterEvent filter in onDamageFilter)
        foreach (GameplayEventHolder.DamageFilterEvent filter in GameplayEventHolder.OnDamageFilter)
        {
            filter(ref context);
            Debug.Log("After Filter " + filter + ": " + context.damage);
        }

        
        // Resistance
        context.damage *= 1.0f - damageResistance;
        
        Debug.Log("Damaged: " + context.damage);

        // Reduce current health
        currentHealth -= context.damage;

        // Trigger events
        GameplayEventHolder.OnDamageDealt?.Invoke(context);

        // Kill entity
        if (currentHealth <= 0f)
        {
            Kill(context);
        }

        AggroEnemy(context.victim);

        return context.damage;
    }

    /// <summary>
    /// Damage method that does not invoke any OnDamage events and also not
    /// processed by any damage filters, useful certain damage like Yume's
    /// fatebound effect
    /// </summary>
    public float NoContextDamage(DamageContext context, GameObject attacker)
    {
        context.attacker = attacker;
        context.damage = Mathf.Clamp(context.damage, 0f, currentHealth);

        // Resistance
        context.damage *= 1.0f - damageResistance;

        currentHealth -= context.damage;

        AggroEnemy(context.victim);

        if (currentHealth <= 0f)
        {
            Kill(context);
        }

        return context.damage;
    }

    public virtual float Heal(HealingContext context, GameObject healer)
    {
        // Configure healing context
        float missingHealth = stats.ComputeValue("Max Health") - currentHealth;
        context.healer = healer;
        context.healee = gameObject;
        context.trueHealing = context.healing;
        context.healing = Mathf.Clamp(context.healing, 0f, missingHealth);
        context.invokingScript = this;

        foreach (GameplayEventHolder.HealingFilterEvent filter in GameplayEventHolder.OnHealingFilter)
        {
            filter(ref context);
        }

        // Increase current health
        currentHealth += context.healing;

        // Trigger events
        GameplayEventHolder.OnHealingDealt?.Invoke(context);

        return context.healing;
    }

    public void Kill(DamageContext context)
    {
        isAlive = false;

        foreach (GameplayEventHolder.DeathFilterEvent filter in GameplayEventHolder.OnDeathFilter)
        {
            filter(ref context);
        }

        //Trigger Events
        GameplayEventHolder.OnDeath?.Invoke(context);

        StartCoroutine(DeathCoroutine(context));
    }

    private IEnumerator DeathCoroutine(DamageContext context)
    {
        if (context.victim != PlayerID.instance.gameObject)
        {
            Destroy(gameObject);
            yield break;
        }
        PlayerDeathManager playerDeath = gameObject.GetComponent<PlayerDeathManager>();
        playerDeath.PlayDeathAnim();
    }

    public StatManager.Stat[] GetStatList()
    {
        return statList;
    }

    public void AggroEnemy(GameObject obj)
    {
        if (obj.CompareTag("Enemy"))
        {
            EnemyStateManager enemy = obj.GetComponent<EnemyStateManager>();
            if (enemy != null && (enemy.GetCurrentState().GetType().Equals(typeof(IdleState)) || enemy.GetCurrentState().GetType().Equals(typeof(MoveState))))
            {
                enemy.SwitchState(enemy.AggroState);
            }
        }
    }

    public float GetDamageResistance()
    {
        return damageResistance;
    }

    public void SetDamageResistance(float damageResistance)
    {
        this.damageResistance = Mathf.Clamp(damageResistance, 0.0f, 1.0f);
    }

    public void ModifyDamageResistance(float delta)
    {
        damageResistance = Mathf.Clamp(damageResistance + delta, 0.0f, 1.0f);
    }

    public StatManager GetStats()
    {
        return stats;
    }
}