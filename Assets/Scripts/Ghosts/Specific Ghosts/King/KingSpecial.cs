using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public class KingSpecial : MonoBehaviour, ISpecialMove
{
    private PlayerStateMachine playerStateMachine;

    [HideInInspector] public KingManager manager;
    [HideInInspector] public bool isCasting;
    [HideInInspector] public bool isInvincibilityActive;

    // Start is called before the first frame update
    void Start()
    {
        playerStateMachine = GetComponent<PlayerStateMachine>();
        isCasting = false;
        isInvincibilityActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (manager != null)
        {
            if (manager.getSpecialCooldown() > 0)
            {
                playerStateMachine.OnCooldown("c_special");
            }
            else
            {
                playerStateMachine.OffCooldown("c_special");
            }
        }
    }

    public void StartDash()
    {
        StartCoroutine(specialAbilityCoroutine());
    }

    private IEnumerator specialAbilityCoroutine()
    {
        // Set flags and inits
        isCasting = true;
        isInvincibilityActive = true;
        float castTime = 0.15f;

        // VFX
        CameraShake.instance.Shake(0.2f, 10f, 0f, 10f, new Vector2(Random.Range(-0.5f, 0.5f), 1f));
        GameObject ringExplosion = Instantiate(manager.specialExplosionVFX, transform.position, Quaternion.identity);
        ringExplosion.GetComponent<RingExplosionHandler>().playRingExplosion(manager.GetStats().ComputeValue("Special Explosion Radius"), manager.GetComponent<GhostIdentity>().GetCharacterInfo().primaryColor);

        // Minor vertical mid-air boost for juice
        if (!GetComponent<Animator>().GetBool("p_grounded"))
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, Mathf.Max(GetComponent<Rigidbody2D>().velocity.y, 0f));
            GetComponent<Move>().ApplyKnockback(Vector2.up, 8f, false);
        }

        // Start invincibility
        GameplayEventHolder.OnDamageFilter.Add(invincibilityFilter);

        // Affect enemies
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, manager.GetStats().ComputeValue("Special Explosion Radius"), LayerMask.GetMask("Enemy"));
        foreach (Collider2D enemy in enemies)
        {
            enemy.gameObject.GetComponent<Health>().Damage(manager.specialDamage, gameObject);
            enemy.gameObject.GetComponent<EnemyStateManager>().Stun(manager.specialDamage, manager.GetStats().ComputeValue("Special Stun Duration"));

            // Deal knockback
            Vector2 enemyDirection = (enemy.transform.position - transform.position).normalized;
            Vector3 knockbackDirection = new Vector2(enemyDirection.x, Mathf.Max(enemyDirection.y, 0f)).normalized; // Enemies under player will be knocked laterally, not down
            float knockbackStrength = manager.GetStats().ComputeValue("Special Knockback Strength");
            Vector2 knockbackVector = knockbackDirection * knockbackStrength;
            float extraUpwardKnockback = Mathf.Max(manager.GetStats().ComputeValue("Special Minimum Upward Knockback Strength") - knockbackVector.y, 0f); // Enemies are guaranteed to be knocked up some amount
            enemy.gameObject.GetComponent<EnemyStateManager>().ApplyKnockback(knockbackDirection, knockbackStrength); // Base knockback
            enemy.gameObject.GetComponent<EnemyStateManager>().ApplyKnockback(Vector2.up, extraUpwardKnockback); // Extra knock up
        }

        // End cast
        yield return new WaitForSeconds(castTime);
        endSpecial(true, false);

        // End invincibility
        yield return new WaitForSeconds(Mathf.Max((manager.GetStats().ComputeValue("Special Invincibility Duration") - castTime), 0f));
        endSpecial(false, true);

        // Trigger events
        GameplayEventHolder.OnAbilityUsed.Invoke(manager.specialContext);
    }

    /// <summary>
    /// End the special ability if it is active.
    /// </summary>
    /// <param name="stopCasting">If true, the ability's cast will be cancelled.</param>
    /// <param name="stopInvincibility">If true, the ability's invincibility buff will be cancelled.</param>
    public void endSpecial(bool stopCasting, bool stopInvincibility)
    {
        // Stop casting special
        if (isCasting && stopCasting)
        {
            playerStateMachine.EnableTrigger("OPT");
            manager.startSpecialCooldown();
            isCasting = false;
        }

        // Stop special invincibility
        if (isInvincibilityActive && stopInvincibility)
        {
            GameplayEventHolder.OnDamageFilter.Remove(invincibilityFilter);
            isInvincibilityActive = false;
        }
    }

    public void invincibilityFilter(ref DamageContext context)
    {
        if (context.victim.tag == "Player")
        {
            context.damage = 0f;
        }
    }

    public bool GetBool()
    {
        return true;
    }
}
