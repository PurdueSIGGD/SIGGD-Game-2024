using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attached to a poision prefab that is attached to a target
/// </summary>
public class PoisonDebuff : MonoBehaviour
{

    Health health;
    [SerializeField] DamageContext damageContext;
    [SerializeField] float damage;
    [SerializeField] float time; // time before debuff ends
    [SerializeField] float interval; // seconds per tick
    float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ByeByeCoroutine(time));
        health = gameObject.GetComponentInParent<Health>();
        timer = interval;
        damageContext.damage = damage;
    }

    // Update is called once per frame
    void Update()
    {
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            return;
        }
        health = gameObject.GetComponentInParent<Health>();
        if (health != null)
        {
            health.Damage(damageContext, gameObject);
        }
        timer = interval;
    }
    IEnumerator ByeByeCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }

    public void Init(DamageContext context, float damage, float time, float interval)
    {
        damageContext = context;
        this.damage = damage;
        this.time = time;
        this.interval = interval;
    }
}
