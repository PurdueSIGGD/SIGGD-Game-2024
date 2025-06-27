using UnityEngine;

public class KingBasic : MonoBehaviour
{
    private PlayerStateMachine playerStateMachine;

    [HideInInspector] public KingManager manager;
    [HideInInspector] public bool isShielding;

    private Camera mainCamera;

    private GameObject shieldCircle;

    // Start is called before the first frame update
    void Start()
    {
        playerStateMachine = GetComponent<PlayerStateMachine>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        isShielding = false;
    }

    // Update is called once per frame
    void Update()
    {
        
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

        // Set flag
        isShielding = true;

        // VFX
        shieldCircle = Instantiate(manager.shieldCircleVFX, gameObject.transform.position, Quaternion.identity, gameObject.transform);
        shieldCircle.GetComponent<CircleAreaHandler>().playCircleStart(1.4f, manager.GetComponent<GhostIdentity>().GetCharacterInfo().primaryColor);

        // Start player effects
        GameplayEventHolder.OnDamageFilter.Add(invincibilityFilter);
    }

    public void StopHeavyChargeUp()
    {
        isShielding = false;
        if (manager.getBasicCooldown() <= 0f) manager.endShieldHealth = manager.currentShieldHealth;
        float maxCooldown = manager.GetStats().ComputeValue("Shield Max Health") / manager.GetStats().ComputeValue("Shield Health Regeneration Rate");
        float cooldownMultiplier = (manager.GetStats().ComputeValue("Shield Max Health") - manager.currentShieldHealth - manager.GetStats().ComputeValue("Shield Health Cooldown Threshold"))
                                   / manager.GetStats().ComputeValue("Shield Max Health");
        manager.setBasicCooldown(maxCooldown * cooldownMultiplier);

        // VFX
        if (shieldCircle != null) shieldCircle.GetComponent<CircleAreaHandler>().playCircleEnd();

        // End effects
        GameplayEventHolder.OnDamageFilter.Remove(invincibilityFilter);
    }

    public void invincibilityFilter(ref DamageContext context)
    {
        if (context.victim.tag != "Player") return;

        // Shield damage absorb
        manager.currentShieldHealth = Mathf.Max(manager.currentShieldHealth - context.damage, 0f);
        context.damage = 0f;

        // Shield destroyed
        if (manager.currentShieldHealth <= 0f)
        {
            // VFX
            CameraShake.instance.Shake(0.2f, 10f, 0f, 10f, new Vector2(Random.Range(-0.5f, 0.5f), 1f));
            GameObject shieldExplosion = Instantiate(manager.shieldExplosionVFX, gameObject.transform.position, Quaternion.identity, gameObject.transform);
            shieldExplosion.GetComponent<RingExplosionHandler>().playRingExplosion(manager.GetStats().ComputeValue("Shield Break Explosion Radius"), manager.GetComponent<GhostIdentity>().GetCharacterInfo().primaryColor);

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
        PlayerID.instance.gameObject.GetComponent<Move>().PlayerStop();
        Vector2 target = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        GameObject shield = Instantiate(manager.thrownShield, transform.position, transform.rotation);
        shield.GetComponent<KingThrownShield>().Init(this, (target - (Vector2)transform.position).normalized);
    }

    private void StopThrowShield()
    {
        PlayerID.instance.gameObject.GetComponent<Move>().PlayerGo();
    }
}
