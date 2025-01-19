using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{

    public float maxHealth; // Max health of player
    [NonSerialized] public float currentHealth; // Current health of player
    [NonSerialized] public bool isAlive = true; // Checks if player is still alive
    private Stats stats;



    // Start is called before the first frame update
    void Start()
    {
        stats = GetComponent<Stats>();
        if (stats != null )
        {
            maxHealth = stats.ComputeValue("Max Health");
        }
        currentHealth = maxHealth;
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
        GameplayEventHolder.OnDamageReceived?.Invoke(context);

        // Kill entity
        if (currentHealth <= 0f)
        {
            Kill();
        }

        return context.damage;
    }



    public float Heal(HealingContext context, GameObject healer)
    {
        // Configure healing context
        float missingHealth = maxHealth - currentHealth;
        context.healer = healer;
        context.healee = gameObject;
        context.trueHealing = context.healing;
        context.healing = Mathf.Clamp(context.healing, 0f, missingHealth);
        context.invokingScript = this;

        // Increase current health
        currentHealth += context.healing;

        // Trigger events

        return context.healing;
    }



    public void Kill()
    {
        isAlive = false;

        //Trigger Events

        Destroy(this.gameObject);
    }
}
