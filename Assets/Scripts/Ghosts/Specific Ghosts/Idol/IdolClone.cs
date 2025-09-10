using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script for Idol's clone, destroys self after duration time has passed
/// </summary>
public class IdolClone : MonoBehaviour
{
    private IdolManager manager;
    [SerializeField] public float duration; // duration clone can last
    [SerializeField] private float inactiveModifier;

    [Header("Used by clone to kill self")]
    [SerializeField] DamageContext expireContext = new DamageContext();
    private GameObject player;

    [SerializeField] public ParticleSystem teleportVFX;
    [SerializeField] public GameObject pulseVFX;

    void OnEnable()
    {
        GameplayEventHolder.OnDeath += CloneDeath;
        GameplayEventHolder.OnDamageDealt += CloneDamageTaken;
    }

    private void OnDisable()
    {
        GameplayEventHolder.OnDeath -= CloneDeath;
        GameplayEventHolder.OnDamageDealt -= CloneDamageTaken;
    }

    void Update()
    {
        TickTimer();
    }

    void TickTimer()
    {
        if (duration <= 0)
        {
            DeallocateDecoy();
        }
        if (player.GetComponent<IdolSpecial>())
        {
            duration -= Time.deltaTime;
        }
        else // if player is no longer in idol mode, count down twice as fast
        {
            duration -= Time.deltaTime * inactiveModifier;
        }
    }

    /// <summary>
    /// Pass reference of player to clone, so to check which ghost player is
    /// using
    /// </summary>
    /// <param name="player"> player gameobject </param>
    public void Initialize(GameObject player, IdolManager manager, float duration, float inactiveModifier)
    {
        this.player = player;
        this.manager = manager;
        this.duration = duration;
        this.inactiveModifier = inactiveModifier;
        this.manager = manager;
        teleportVFX.Play();
    }

    /// <summary>
    /// Pass reference of player to clone, so to check which ghost player is
    /// using
    /// </summary>
    /// <param name="player"> player gameobject </param>
    public void Initialize(GameObject player, IdolManager manager, float duration, float inactiveModifier, float maxHealth)
    {
        this.player = player;
        this.manager = manager;
        this.duration = duration;
        this.inactiveModifier = inactiveModifier;
        this.manager = manager;
        StartCoroutine(ChangeHealth(maxHealth));
    }

    private IEnumerator ChangeHealth(float maxHealth)
    {
        yield return new WaitForSeconds(0.1f);
        float initialMaxHealth = GetComponent<StatManager>().ComputeValue("Max Health");
        GetComponent<StatManager>().ModifyStat("Max Health", -(Mathf.CeilToInt((1f - (maxHealth / initialMaxHealth)) * 100f)));
        GetComponent<Health>().currentHealth = GetComponent<StatManager>().ComputeValue("Max Health");
    }

    public void DeallocateDecoy()
    {
        if (manager)
        {
            manager.clones.Remove(gameObject);
        }
        expireContext.attacker = gameObject;
        expireContext.victim = gameObject;
        GameplayEventHolder.OnDeath.Invoke(expireContext);
        GameplayEventHolder.OnDeath -= CloneDeath;
        Destroy(gameObject);
    }

    private void CloneDeath(DamageContext context)
    {
        if (context.victim != gameObject) return;

        if (manager)
        {
            manager.clones.Remove(gameObject);
        }
        if (manager.clones.Count > 0) manager.clones[0].GetComponent<IdolClone>().teleportVFX.Play();

        // SFX
        AudioManager.Instance.SFXBranch.PlaySFXTrack("Eva-Nova Pop");
    }

    private void CloneDamageTaken(DamageContext context)
    {
        if (context.victim != gameObject) return;
        GameObject damageImpactVFX = Instantiate(pulseVFX, transform.position, Quaternion.identity);
        damageImpactVFX.GetComponent<RingExplosionHandler>().playRingExplosion(2f, manager.GetComponent<GhostIdentity>().GetCharacterInfo().primaryColor);
    }
}
