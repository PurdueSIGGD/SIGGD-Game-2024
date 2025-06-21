using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YumeProjectile : MonoBehaviour
{
    SeamstressManager manager;
    float lifeTime; // time after which the projectile should expire
    bool hit = false; // if the projectile has hit an enemy
    GameObject hitTarget;

    // standard projectile fields
    Rigidbody2D rb;
    Vector2 dir;
    float speed;

    // trigger is entered first before the collider
    // if there is a transparent platform, ignore it beforehand
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Transparent"))
        {
            Physics2D.IgnoreCollision(collider.gameObject.GetComponent<CompositeCollider2D>(), GetComponent<BoxCollider2D>(), true);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject hitObject = collision.gameObject;
        if (hitObject.layer == LayerMask.NameToLayer("Ground"))
        {
            float angle = Vector2.Angle(dir, -collision.contacts[0].normal);
            if (angle > 60)
            {
                hit = true; // trigger HasExpired
                Destroy(gameObject);
            }

            dir = Vector2.Reflect(dir, collision.contacts[0].normal);
            if (manager.IncrementRicochet()) // bouncing off surface counts as ricochet too
            {
                manager.ResetRicochet();
                Destroy(gameObject);
            }
        }
        else if (hitObject.CompareTag("Enemy"))
        {
            if (hitObject.GetComponent<FateboundDebuff>() != null)
            {
                Physics2D.IgnoreCollision(collision.collider, GetComponent<BoxCollider2D>());
            }
            else
            {
                hit = true;
                hitTarget = collision.gameObject;
                Destroy(gameObject);
            }
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();   
    }

    void Update()
    {
        rb.velocity = dir * speed;
        lifeTime -= Time.deltaTime;

        if (lifeTime < 0.1) Destroy(gameObject);
    }

    public void Initialize(Vector2 dest, float speed, float range, SeamstressManager manager)
    {
        lifeTime = range / speed;
        dir = (dest - (Vector2)transform.position).normalized;
        float angle = Mathf.Atan2(dest.y - transform.position.y, dest.x - transform.position.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        this.speed = speed;
        this.manager = manager;
    }

    public bool HasExpired() { return hit || lifeTime <= 0; }

    public GameObject GetHitTarget() { return hitTarget; }
}
