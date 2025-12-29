using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageFireBallAttack : MonoBehaviour
{
    GameObject target;
    GameObject attacker;
    Rigidbody2D rb;
    float speed;
    [SerializeField] float speedPerSec; // speed to multiply every second to excitedly ramp up speed with time
    float trackIntensity; // 0 to 1 range float
    DamageContext fireDamageImpact;
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

        this.target = target;
        this.attacker = attacker;
        this.fireDamageImpact = fireDamageImpact;
        this.fireDamageTick = fireDamageTick;
        this.speed = baseSpeed;
        this.trackIntensity = Mathf.Clamp01(trackIntensity);
        this.damageTickIntervalSec = damageTickIntervalSec;
        this.damageTickDurationSec = damageTickDurationSec;

        speed = baseSpeed;
    }
    void FixedUpdate()
    {
        Track();
        MultiplicativeSpeedUp();
    }

    void MultiplicativeSpeedUp()
    {
        speed = speed * (1 + speedPerSec * Time.deltaTime);
    }

    void Track()
    {
        if (target == null) return;

        Vector3 goalDirection = (target.transform.position - transform.position).normalized;
        Vector3 realDirection = Vector2.Lerp(rb.velocity.normalized, goalDirection, trackIntensity); // Move partially towards goalDirection according to trackIntensity
        rb.velocity = realDirection.normalized * speed; // set new velocity
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == target)
        {
            OnHit(collision.gameObject);
        }
    }

    void OnHit(GameObject target)
    {
        Health targetHealth = target.GetComponent<Health>();
        if (targetHealth != null)
        {
            targetHealth.Damage(fireDamageImpact, attacker);
            PoisonDebuff myFire = Instantiate(poisonDebuffPrefab, target.transform).GetComponent<PoisonDebuff>();
            myFire.Init(fireDamageTick, fireDamageTick.damage, damageTickDurationSec, damageTickIntervalSec);
            myFire.SetAttacker(attacker);
        }
        Destroy(gameObject);
    }
}
