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

    public delegate void DamageFilters(DamageContext context);


    // Start is called before the first frame update
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

        foreach (GameplayEventHolder.DamageFilterEvent filter in GameplayEventHolder.OnDamageFilter)
        {
            filter(ref context);
            Debug.Log("After Filter: " + context.damage);
        }

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

        currentHealth -= context.damage;

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
        GameplayEventHolder.OnDeath?.Invoke(ref context);

        StartCoroutine(DeathCoroutine(context));
    }

    private IEnumerator DeathCoroutine(DamageContext context)
    {
        if (context.victim != PlayerID.instance.gameObject)
        {
            Destroy(gameObject);
            yield break;
        }

        Time.timeScale = 0;
        gameObject.layer = 0; // I really hope this doesn't collide with anything
        if (GetComponent<PlayerInput>() != null) GetComponent<PlayerInput>().enabled = false;

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
        Time.timeScale = 1;
    }

    public StatManager.Stat[] GetStatList()
    {
        return statList;
    }
}
