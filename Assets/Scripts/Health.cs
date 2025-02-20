using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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

        StartCoroutine(DeathCoroutine());
    }

    private IEnumerator DeathCoroutine()
    {
        Time.timeScale = 0;
        gameObject.layer = 0; // I really hope this doesn't collide with anything
        GetComponent<PlayerInput>().enabled = false;

        float startTime = Time.unscaledTime;
        float endTime = startTime + 3;

        Vector3 originalScale = transform.localScale;

        while (Time.unscaledTime < endTime)
        {
            float timePercentage = (Time.unscaledTime - startTime) / (endTime - startTime);

            transform.Rotate(0, 0, Time.unscaledDeltaTime * 360);
            transform.localScale = originalScale * (1 - timePercentage);

            yield return null;
        }

        gameObject.SetActive(false);

        SceneManager.LoadScene("HubWorld");
    }

    public StatManager.Stat[] GetStatList()
    {
        return statList;
    }
}
