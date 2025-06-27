using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        rb.velocity = minSpeed * dir;
        returning = false;
    }

    void FixedUpdate()
    {
        if (!returning)
        {
            if (Vector2.Distance(transform.position, orig) > deaccelZone) // if in deaccel zone
            {
                rb.velocity -= deaccel * Time.deltaTime * dir;

                // if velocity is already below the minimum speed, begin the return process
                if (rb.velocity.magnitude <= minSpeed)
                {
                    BeginReturn();
                }
            }
            else // before reaching the deaccel zone, accelerate continuously
            {
                rb.velocity += accel * Time.deltaTime * dir;
            }
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
            else
            {
                // in either case, always begin the return process
                BeginReturn();
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

    private void BeginReturn()
    {
        if (returning) return;
        returning = true;
        Vector2 diff = player.transform.position - transform.position;
        // if shield is already next to player, simply pick it immediately
        // (this happens when player throws shield right below their feet)
        if (diff.sqrMagnitude < 1.5)
        {
            PickUpShield();
        }
        dir = diff.normalized;
        rb.velocity = minSpeed * dir;
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
}
