using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageFireBallAttack : EnemyProjectile
{

    [Header("Fireball Script Misc. Params")]
    GameObject targetObject;
    [SerializeField] GameObject visual;
    [SerializeField] float speedPerSec; // speed to multiply every second to excitedly ramp up speed with time
    [SerializeField] float maxDuration; // max duration before self-destruct
    [Header("FireBall Damage")]
    [SerializeField] float impactDamage;
    [SerializeField] private DamageContext fireDamageTick;
    [SerializeField] float tickDamage;
    [Header("FireBall Settings")]
    [SerializeField] float trackIntensity;
    [SerializeField] float damageTickIntervalSec;
    [SerializeField] float damageTickDurationSec;
    [SerializeField] GameObject poisonDebuffPrefab;

    public void Initialize(GameObject target,
                            GameObject attacker)
    {
        Init(attacker, target.transform.position);
        targetObject = target;
        trackIntensity = Mathf.Clamp01(trackIntensity);
        projectileDamage.damage = impactDamage;
        fireDamageTick.damage = tickDamage;
    }
    void Update()
    {
        maxDuration -= Time.deltaTime;
        if (maxDuration <= 0f)
        {
            Destroy(gameObject);
        }
    }
    new void FixedUpdate()
    {
        Move();
        MultiplicativeSpeedUp();
    }

    void MultiplicativeSpeedUp()
    {
        speed = speed * (1 + speedPerSec * Time.deltaTime);
    }

    protected override void Move()
    {
        if (target != null && !parried) // only run tracking logic if target exists and projectile hasn't been parried yet
        {
            Vector3 goalDirection = (targetObject.transform.position - transform.position).normalized;
            Vector3 realDirection = Vector2.Lerp(rb.velocity.normalized, goalDirection, trackIntensity); // Move partially towards goalDirection according to trackIntensity
            realDirection += new Vector3(Random.Range(-0.01f, 0.01f), Random.Range(-0.01f, 0.01f), 0); // noise machine
            dir = realDirection.normalized;
        }
        rb.velocity = dir * speed; // set new velocity

        // update visual rotation
        visual.transform.rotation = Quaternion.LookRotation(Vector3.forward, rb.velocity.normalized);
    }
    protected override void DamageTarget(GameObject target, GameObject attacker)
    {
        // reconfigure damage ownership
        projectileDamage.attacker = attacker;
        projectileDamage.victim = target;
        fireDamageTick.attacker = attacker;
        fireDamageTick.victim = target;

        Health targetHealth = target.GetComponent<Health>();
        if (targetHealth != null)
        {
            float result = targetHealth.Damage(projectileDamage, attacker);
            if (result > 0.001f)
            {
                PoisonDebuff myFire = Instantiate(poisonDebuffPrefab, target.transform).GetComponent<PoisonDebuff>();
                myFire.Init(fireDamageTick, fireDamageTick.damage, damageTickDurationSec, damageTickIntervalSec);
                myFire.SetAttacker(attacker);
            }

        }
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        base.ProcessCollision(collision.gameObject);
    }
}
