using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attached to a poision prefab that is attached to a target
/// </summary>
public class PoisonDebuff : MonoBehaviour
{

    Health health;
    [SerializeField] public DamageContext damageContext;
    [SerializeField] float damage;
    [SerializeField] float interval; // seconds per tick
    private float timer = 999f;
    public float duration = 999f;

    // Start is called before the first frame update
    void Start()
    {
        health = gameObject.GetComponentInParent<Health>();
        timer = interval;
        damageContext.damage = damage;
    }

    // Update is called once per frame
    void Update()
    {
        if (duration <= 0f)
        {
            health = gameObject.GetComponentInParent<Health>();
            if (health != null)
            {
                health.Damage(damageContext, damageContext.attacker);
            }
            Destroy(gameObject);
        }
        duration -= Time.deltaTime;

        if (timer > 0)
        {
            timer -= Time.deltaTime;
            return;
        }
        health = gameObject.GetComponentInParent<Health>();
        if (health != null)
        {
            health.Damage(damageContext, damageContext.attacker);
        }
        timer = interval;
    }

    public void SetAttacker(GameObject attacker)
    {
        damageContext.attacker = attacker;
    }

    public void Init(DamageContext context, float dps, float time, float interval)
    {
        damageContext = context;
        //damageContext.actionID = context.actionID;
        this.damage = dps * interval;
        this.interval = interval;

        duration = time;
        health = gameObject.GetComponentInParent<Health>();
        timer = 0f;
        damageContext.damage = damage;
    }
}
