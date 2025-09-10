using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class KingThrownShield : MonoBehaviour
{
    [Header("Damage Context of Thrown Shield")]
    [SerializeField] private DamageContext context;
    [Header("Range before Projectile begins to deaccelerate")]
    [SerializeField] private float deaccelZone;
    [Header("Minimum speed of Projectile")]
    [SerializeField] private float minSpeed; 
    [Header("Deacceleration speed")]
    [SerializeField] private float deaccel;
    [Header("Acceleration speed")]
    [SerializeField] private float accel;
    [Header("Acceleration speed while returning")]
    [SerializeField] private float returningAccel;
    [Header("Time before shield can be picked up (unless returning)")]
    [SerializeField] private float pickUpImmunityTime;

    [SerializeField] private float initialSpeed;
    [SerializeField] private float ricochetSpeed;

    [SerializeField] private GameObject impactVFX;

    [SerializeField] private float knockbackStrength;
    [SerializeField] private float returnKnockbackStrength;
    [SerializeField] private float minimumUpwardKnockback;

    private KingBasic basic;
    private GameObject player;
    private Rigidbody2D rb;
    private Vector2 orig;
    private Vector2 dir;
    private bool returning = false;

    void Awake()
    {
        player = PlayerID.instance.gameObject;
        rb = GetComponent<Rigidbody2D>();
    }

    public void Init(KingBasic basic, Vector2 dir, float dmg)
    {
        this.basic = basic;
        basic.DisableShield(true);
        orig = transform.position;
        this.dir = dir;
        UpdateOrientation();
        context.damage = dmg;
        //rb.velocity = minSpeed * dir;
        rb.velocity = initialSpeed * dir;
        returning = false;
    }

    void FixedUpdate()
    {
        if (!returning)
        {
            if (Vector2.Distance(transform.position, orig) > deaccelZone) // if in deaccel zone
            {
                TryDestroyShield();
                rb.velocity -= deaccel * Time.deltaTime * dir;

                // if velocity is already below the minimum speed, begin the return process
                if (rb.velocity.magnitude <= minSpeed)
                {
                    BeginReturn(-1f);
                }
            }
            /*
            else // before reaching the deaccel zone, accelerate continuously
            {
                rb.velocity += accel * Time.deltaTime * dir;
            }
            */
        }
        else // if attempting to return to king, accelerate indefinitely
        {
            dir = (player.transform.position - transform.position).normalized;
            UpdateOrientation();
            rb.velocity = rb.velocity.magnitude * dir;
            rb.velocity += returningAccel * Time.deltaTime * dir;
        }
        pickUpImmunityTime -= Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if ((collider.CompareTag("Enemy")))
        {
            DamageHitEnemy(collider.gameObject);
            TryDestroyShield();
            AudioManager.Instance.SFXBranch.PlaySFXTrack("Aegis-Shield Throw Impact");
            CameraShake.instance.Shake(0.15f, 10f, 0f, 10f, new Vector2(Random.Range(-0.5f, 0.5f), 1f));
            GameObject shieldImpact = Instantiate(impactVFX, transform.position, Quaternion.identity);
            shieldImpact.GetComponent<RingExplosionHandler>().playRingExplosion(1.5f, basic.manager.GetComponent<GhostIdentity>().GetCharacterInfo().highlightColor);
        }
        if (!returning) // if the shield is still shooting outwards
        {
            // if collides with player, only allow picking up the shield after some time
            if (collider.CompareTag("Player")) 
            {
                if (pickUpImmunityTime < 0)
                {
                    PickUpShield();
                }
            }
            // if not player, the shield should only be able to collide with environment or enemy
            else if (LayerMask.LayerToName(collider.gameObject.layer).Equals("Ground"))
            {
                // in either case, always begin the return process
                TryDestroyShield();
                AudioManager.Instance.SFXBranch.PlaySFXTrack("Aegis-Shield Throw Impact");
                CameraShake.instance.Shake(0.15f, 10f, 0f, 10f, new Vector2(Random.Range(-0.5f, 0.5f), 1f));
                GameObject shieldImpact = Instantiate(impactVFX, transform.position, Quaternion.identity);
                shieldImpact.GetComponent<RingExplosionHandler>().playRingExplosion(1.5f, basic.manager.GetComponent<GhostIdentity>().GetCharacterInfo().highlightColor);
                BeginReturn(ricochetSpeed);
            }
        }
        else // if the shield is now returning
        {
            // if collides with player while returning, pick it up
            if (collider.CompareTag("Player"))
            {
                PickUpShield();
            }
        }
    }

    private void BeginReturn(float ricochetSpeed)
    {
        if (returning) return;
        TryDestroyShield();
        returning = true;
        Vector2 diff = player.transform.position - transform.position;
        // if shield is already next to player, simply pick it immediately
        // (this happens when player throws shield right below their feet)
        if (diff.sqrMagnitude < 1.5)
        {
            PickUpShield();
        }
        dir = diff.normalized;
        rb.velocity = ((ricochetSpeed < 0f) ? minSpeed : ricochetSpeed) * dir;
    }

    private void DamageHitEnemy(GameObject enemy)
    {
        Health health = enemy.GetComponent<Health>();
        // I don't want anything to happen if the shield hasn't taken any damage :)
        // otherwise there will be shennigans like spamming shield to stun enemy
        // even though no damage is done
        if (health != null && context.damage > 0)
        {
            health.Damage(context, player);
        }

        // Deal knockback
        Vector2 enemyDirection = (enemy.transform.position - transform.position).normalized;
        Vector3 knockbackDirection = new Vector2(enemyDirection.x, enemyDirection.y).normalized;
        Vector2 knockbackVector = knockbackDirection * ((returning) ? returnKnockbackStrength : knockbackStrength);
        float extraUpwardKnockback = Mathf.Max(minimumUpwardKnockback - knockbackVector.y, 0f); // Enemies are guaranteed to be knocked up some amount
        enemy.gameObject.GetComponent<EnemyStateManager>().ApplyKnockback(knockbackDirection, knockbackStrength); // Base knockback
        enemy.gameObject.GetComponent<EnemyStateManager>().ApplyKnockback(Vector2.up, extraUpwardKnockback); // Extra knock up
    }

    private void UpdateOrientation()
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private void PickUpShield()
    {
        basic.DisableShield(false);
        Destroy(gameObject);
    }

    private void TryDestroyShield()
    {
        if (basic.manager.currentShieldHealth <= 0f)
        {
            basic.DestroyShield(transform.position);
            basic.manager.setBasicCooldown(basic.manager.GetStats().ComputeValue("Basic Cooldown"));
            PickUpShield();
        }
    }
}
