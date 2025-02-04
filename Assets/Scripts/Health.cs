using System;
using UnityEngine;

[DisallowMultipleComponent]
public class Health : MonoBehaviour, IDamageable, IStatList
{
    [SerializeField]
    public StatManager.Stat[] statList;

    [NonSerialized] public float currentHealth; // Current health of player
    [NonSerialized] public bool isAlive = true; // Checks if player is still alive
    private StatManager stats;



    // Start is called before the first frame update
    void Start()
    {
        stats = GetComponent<StatManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public float Damage(DamageContext context, GameObject attacker)
    {
        // Configure damage context
        context.attacker = attacker;
        context.victim = gameObject;
        context.trueDamage = context.damage;
        context.damage = Mathf.Clamp(context.damage, 0f, currentHealth);
        context.invokingScript = this;

        // Reduce current health
        currentHealth -= context.damage;

        // Trigger events
        GameplayEventHolder.OnDamageDealt?.Invoke(context);

        // Kill entity
        if (currentHealth <= 0f)
        {
            Kill(context);
        }

        return context.damage;
    }



    public float Heal(HealingContext context, GameObject healer)
    {
        // Configure healing context
        float missingHealth = stats.ComputeValue("Max Health") - currentHealth;
        context.healer = healer;
        context.healee = gameObject;
        context.trueHealing = context.healing;
        context.healing = Mathf.Clamp(context.healing, 0f, missingHealth);
        context.invokingScript = this;

        // Increase current health
        currentHealth += context.healing;

        // Trigger events
        GameplayEventHolder.OnHealingDealt?.Invoke(context);

        return context.healing;
    }

    public void Kill(DamageContext context)
    {
        isAlive = false;

        //Trigger Events
        GameplayEventHolder.OnDeath?.Invoke(context);

        Destroy(this.gameObject);
    }

    public StatManager.Stat[] GetStatList()
    {
        return statList;
    }
}
