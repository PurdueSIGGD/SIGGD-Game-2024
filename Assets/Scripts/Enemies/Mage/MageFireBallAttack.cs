using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageFireBallAttack : EnemyProjectile
{
    GameObject targetObject;
    [SerializeField] float speedPerSec; // speed to multiply every second to excitedly ramp up speed with time
    float trackIntensity; // 0 to 1 range float
    DamageContext fireDamageTick;
    float damageTickIntervalSec;
    float damageTickDurationSec;
    [SerializeField] GameObject poisonDebuffPrefab;

    public void Initialize(GameObject target,
                            GameObject attacker,
                            DamageContext fireDamageImpact,
                            DamageContext fireDamageTick,
                            float baseSpeed,
                            float trackIntensity,
                            float damageTickIntervalSec,
                            float damageTickDurationSec)
    {
        rb = GetComponent<Rigidbody2D>();

        this.targetObject = target;
        this.projectileDamage = fireDamageImpact;
        this.fireDamageTick = fireDamageTick;
        this.speed = baseSpeed;
        this.trackIntensity = Mathf.Clamp01(trackIntensity);
        this.damageTickIntervalSec = damageTickIntervalSec;
        this.damageTickDurationSec = damageTickDurationSec;

        base.Init(attacker, target.transform.position);
        speed = baseSpeed;
        base.projectileDamage = fireDamageImpact;
    }
    void FixedUpdate()
    {
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
            dir = realDirection.normalized;
        }
        rb.velocity = dir * speed; // set new velocity
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
            targetHealth.Damage(projectileDamage, attacker);
            PoisonDebuff myFire = Instantiate(poisonDebuffPrefab, target.transform).GetComponent<PoisonDebuff>();
            myFire.Init(fireDamageTick, fireDamageTick.damage, damageTickDurationSec, damageTickIntervalSec);
            myFire.SetAttacker(attacker);
        }
    }
}
