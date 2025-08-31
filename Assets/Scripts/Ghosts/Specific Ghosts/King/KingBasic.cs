using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class KingBasic : MonoBehaviour
{
    private PlayerStateMachine playerStateMachine;
    private GameObject shieldCircle;

    [HideInInspector] public KingManager manager;
    [HideInInspector] public bool isShielding;
    private Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        playerStateMachine = GetComponent<PlayerStateMachine>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        isShielding = false;
    }

    private void OnDisable()
    {
        if (GameplayEventHolder.OnDamageFilter.Contains(invincibilityFilter)) GameplayEventHolder.OnDamageFilter.Remove(invincibilityFilter);
    }

    // Update is called once per frame
    void Update()
    {
        if (manager.getBasicCooldown() > 0)
        {
            playerStateMachine.OnCooldown("c_basic");
        }
        else
        {
            playerStateMachine.OffCooldown("c_basic");
        }
    }

    public void StartHeavyChargeUp()
    {
        if (manager.currentShieldHealth <= 0f || manager.getBasicCooldown() > 0f)
        {
            playerStateMachine.EnableTrigger("OPT");
            return;
        }
        if (manager.recompenceAvaliable)
        {
            playerStateMachine.OnCooldown("recompence_avaliable");
        }
        else
        {
            playerStateMachine.OffCooldown("recompence_avaliable");
        }

        GetComponent<PartyManager>().SetSwappingEnabled(false);

        // Set flag
        isShielding = true;

        // VFX
        shieldCircle = Instantiate(manager.shieldCircleVFX, gameObject.transform.position, Quaternion.identity, gameObject.transform);
        shieldCircle.GetComponent<CircleAreaHandler>().playCircleStart(1.4f, manager.GetComponent<GhostIdentity>().GetCharacterInfo().primaryColor);

        // SFX
        AudioManager.Instance.SFXBranch.PlaySFXTrack("Aegis-Shield Deploy");
        AudioManager.Instance.SFXBranch.PlaySFXTrack("Aegis-Shield Holding");
        AudioManager.Instance.VABranch.PlayVATrack("Aegis-King Shield Deploy");

        // Start player effects
        GameplayEventHolder.OnDamageFilter.Add(invincibilityFilter);
    }

    public void StopHeavyChargeUp()
    {
        float maxHealth = manager.GetStats().ComputeValue("Shield Max Health");

        isShielding = false;
        if (manager.getBasicCooldown() <= 0f) manager.endShieldHealth = manager.currentShieldHealth;

        //float maxCooldown = maxHealth / manager.GetStats().ComputeValue("Shield Health Regeneration Rate");
        //float cooldownMultiplier = (maxHealth - manager.currentShieldHealth + manager.GetStats().ComputeValue("Shield Health Cooldown Threshold")) / maxHealth;
        //float cooldownMultiplier = (manager.currentShieldHealth <= 0f) ? 1f : 0.1f;
        //manager.setBasicCooldown(maxCooldown * cooldownMultiplier);

        float maxCooldown = manager.GetStats().ComputeValue("Basic Cooldown");
        manager.setBasicCooldown((manager.currentShieldHealth <= 0f) ? maxCooldown : 0.5f);

        // VFX
        if (shieldCircle != null) shieldCircle.GetComponent<CircleAreaHandler>().playCircleEnd();

        // SFX
        AudioManager.Instance.SFXBranch.StopSFXTrack("Aegis-Shield Deploy");
        AudioManager.Instance.SFXBranch.StopSFXTrack("Aegis-Shield Holding");

        // End effects
        GameplayEventHolder.OnDamageFilter.Remove(invincibilityFilter);

        GetComponent<PartyManager>().SetSwappingEnabled(true);
    }

    public void invincibilityFilter(ref DamageContext context)
    {
        if (!context.victim.CompareTag("Player")) return;

        // Shield damage absorb
        manager.currentShieldHealth = Mathf.Max(manager.currentShieldHealth - context.damage, 0f);

        manager.TakeShieldDamage(context.damage);
        
        context.damage = 0f;
        context.icon = null;

        DamageNumberManager.instance.PlayMessage(this.gameObject, 0f, manager.GetComponent<GhostIdentity>().GetCharacterInfo().basicAbilityIcon, "Blocked!", manager.GetComponent<GhostIdentity>().GetCharacterInfo().highlightColor);

        // VFX
        CameraShake.instance.Shake(0.2f, 10f, 0f, 10f, new Vector2(Random.Range(-0.5f, 0.5f), 1f));
        GameObject surfaceExplosion = Instantiate(manager.shieldExplosionVFX, transform);
        surfaceExplosion.GetComponent<RingExplosionHandler>().playRingExplosion(1.7f, manager.GetComponent<GhostIdentity>().GetCharacterInfo().highlightColor);

        // SFX
        AudioManager.Instance.SFXBranch.GetSFXTrack("Aegis-Shield On Damage").SetPitch(manager.GetStats().ComputeValue("Shield Max Health") - manager.currentShieldHealth, manager.GetStats().ComputeValue("Shield Max Health"));
        AudioManager.Instance.SFXBranch.PlaySFXTrack("Aegis-Shield On Damage");
        AudioManager.Instance.VABranch.PlayVATrack("Aegis-King Shield Damaged");

        // Shield destroyed
        if (manager.currentShieldHealth <= 0f)
        {
            DestroyShield(transform.position);

            // Cancel shield
            playerStateMachine.ConsumeHeavyAttackInput();
            GetComponent<PlayerStateMachine>().EnableTrigger("OPT");
            /*
            // VFX
            CameraShake.instance.Shake(0.25f, 10f, 0f, 10f, new Vector2(Random.Range(-0.5f, 0.5f), 1f));
            GameObject shieldExplosion = Instantiate(manager.shieldExplosionVFX, gameObject.transform.position, Quaternion.identity, gameObject.transform);
            shieldExplosion.GetComponent<RingExplosionHandler>().playRingExplosion(manager.GetStats().ComputeValue("Shield Break Explosion Radius"), manager.GetComponent<GhostIdentity>().GetCharacterInfo().highlightColor);

            // SFX
            AudioManager.Instance.SFXBranch.PlaySFXTrack("Aegis-Shield Break");
            AudioManager.Instance.VABranch.PlayVATrack("Aegis-King Shield Destroyed");

            // Affect enemies
            Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, manager.GetStats().ComputeValue("Shield Break Explosion Radius"), LayerMask.GetMask("Enemy"));
            foreach (Collider2D enemy in enemies)
            {
                enemy.gameObject.GetComponent<Health>().Damage(manager.shieldBreakDamage, gameObject);
                enemy.gameObject.GetComponent<EnemyStateManager>().Stun(manager.shieldBreakDamage, manager.GetStats().ComputeValue("Shield Break Stun Duration"));

                // Deal knockback
                Vector2 enemyDirection = (enemy.transform.position - transform.position).normalized;
                Vector3 knockbackDirection = new Vector2(enemyDirection.x, Mathf.Max(enemyDirection.y, 0f)).normalized; // Enemies under player will be knocked laterally, not down
                float knockbackStrength = manager.GetStats().ComputeValue("Shield Break Knockback Strength");
                Vector2 knockbackVector = knockbackDirection * knockbackStrength;
                float extraUpwardKnockback = Mathf.Max(manager.GetStats().ComputeValue("Shield Break Minimum Upward Knockback Strength") - knockbackVector.y, 0f); // Enemies are guaranteed to be knocked up some amount
                enemy.gameObject.GetComponent<EnemyStateManager>().ApplyKnockback(knockbackDirection, knockbackStrength); // Base knockback
                enemy.gameObject.GetComponent<EnemyStateManager>().ApplyKnockback(Vector2.up, extraUpwardKnockback); // Extra knock up
            }

            // Cancel shield
            GetComponent<PlayerStateMachine>().EnableTrigger("OPT");
            */
        }
    }

    public void DestroyShield(Vector3 position)
    {
        // VFX
        CameraShake.instance.Shake(0.25f, 10f, 0f, 10f, new Vector2(Random.Range(-0.5f, 0.5f), 1f));
        GameObject shieldExplosion = Instantiate(manager.shieldExplosionVFX, position, Quaternion.identity, gameObject.transform);
        shieldExplosion.GetComponent<RingExplosionHandler>().playRingExplosion(manager.GetStats().ComputeValue("Shield Break Explosion Radius"), manager.GetComponent<GhostIdentity>().GetCharacterInfo().highlightColor);

        // SFX
        AudioManager.Instance.SFXBranch.PlaySFXTrack("Aegis-Shield Break");
        AudioManager.Instance.VABranch.PlayVATrack("Aegis-King Shield Destroyed");

        // Affect enemies
        Collider2D[] enemies = Physics2D.OverlapCircleAll(position, manager.GetStats().ComputeValue("Shield Break Explosion Radius"), LayerMask.GetMask("Enemy"));
        foreach (Collider2D enemy in enemies)
        {
            enemy.gameObject.GetComponent<Health>().Damage(manager.shieldBreakDamage, gameObject);
            enemy.gameObject.GetComponent<EnemyStateManager>().Stun(manager.shieldBreakDamage, manager.GetStats().ComputeValue("Shield Break Stun Duration"));

            // Deal knockback
            Vector2 enemyDirection = (enemy.transform.position - position).normalized;
            Vector3 knockbackDirection = new Vector2(enemyDirection.x, Mathf.Max(enemyDirection.y, 0f)).normalized; // Enemies under player will be knocked laterally, not down
            float knockbackStrength = manager.GetStats().ComputeValue("Shield Break Knockback Strength");
            Vector2 knockbackVector = knockbackDirection * knockbackStrength;
            float extraUpwardKnockback = Mathf.Max(manager.GetStats().ComputeValue("Shield Break Minimum Upward Knockback Strength") - knockbackVector.y, 0f); // Enemies are guaranteed to be knocked up some amount
            enemy.gameObject.GetComponent<EnemyStateManager>().ApplyKnockback(knockbackDirection, knockbackStrength); // Base knockback
            enemy.gameObject.GetComponent<EnemyStateManager>().ApplyKnockback(Vector2.up, extraUpwardKnockback); // Extra knock up
        }
    }

    public void DisableShield(bool disable)
    {
        if (disable)
        {
            manager.hasShield = false;
            playerStateMachine.OffCooldown("has_shield");
        }
        else 
        { 
            manager.hasShield = true;
            playerStateMachine.OnCooldown("has_shield"); 
        }
    }

    // should only be avaliable with Recompence skill
    private void StartThrowShield()
    {
        //PlayerID.instance.gameObject.GetComponent<Move>().PlayerStop();
        AudioManager.Instance.SFXBranch.PlaySFXTrack("Silas-Bottle Throw");
        AudioManager.Instance.VABranch.PlayVATrack("Aegis-King Retaliatory Strike");

        playerStateMachine.ConsumeLightAttackInput();
        GetComponent<PartyManager>().SetSwappingEnabled(false);

        Vector2 target = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        GameObject shield = Instantiate(manager.thrownShield, transform.position, transform.rotation);
        shield.GetComponent<KingThrownShield>().Init(this, (target - (Vector2)transform.position).normalized, manager.GetComponent<Recompence>().ComputeDamage());

        manager.currentShieldHealth = Mathf.Max(manager.currentShieldHealth - manager.GetComponent<Recompence>().shieldHealthCost, 0f);
        //float maxCooldown = manager.GetStats().ComputeValue("Basic Cooldown");
        //manager.setBasicCooldown((manager.currentShieldHealth <= 0f) ? maxCooldown : 0.5f);
    }

    private void StopThrowShield()
    {
        //PlayerID.instance.gameObject.GetComponent<Move>().PlayerGo();
        GetComponent<PartyManager>().SetSwappingEnabled(true);
    }
}
