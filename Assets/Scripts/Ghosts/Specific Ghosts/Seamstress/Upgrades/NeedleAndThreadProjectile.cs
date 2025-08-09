using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedleAndThreadProjectile : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 dir;
    private float speed;
    private float lifeTime; // time the projectile will travel before expiring
    
    private int debuffIntensity;
    private float debuffDuration;

    public void Init(GameObject target, float speed, float lifeTime, int intensity, float duration)
    {
        dir = (target.transform.position - transform.position).normalized;
        this.speed = speed;
        this.lifeTime = lifeTime;
        debuffIntensity = intensity;
        debuffDuration = duration;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = dir * speed;
    }

    void Update()
    {
        if (lifeTime < 0) // avoid missed projectile persisting in scene
        {
            Destroy(gameObject);
        }
        lifeTime -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<NeedleAndThreadDebuff>()) // if already have debuff, ignore the target
        {
            Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), collision.gameObject.GetComponent<Collider2D>(), true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            StatManager stats = collision.gameObject.GetComponent<StatManager>();
            if (stats)
            {
                NeedleAndThreadDebuff debuff = collision.gameObject.AddComponent<NeedleAndThreadDebuff>();
                debuff.Init(debuffIntensity, debuffDuration);
            }
            else
            {
                Debug.LogError("Needle and Thread projectile has collided with " +  collision.gameObject.name + " which does not have a stat component.");
            }
        }
        Destroy(gameObject);
    }
}
