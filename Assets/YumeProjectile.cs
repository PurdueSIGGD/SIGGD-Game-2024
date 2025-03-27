using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YumeProjectile : MonoBehaviour
{
    float lifeTime; // time after which the projectile should expire
    bool hit = false; // if the projectile has hit an enemy
    GameObject hitTarget;

    // standard projectile fields
    Rigidbody2D rb;
    Vector2 dir;
    float speed;


    void OnCollisionEnter2D(Collision2D collision)
    {
        // TODO handle bounce logic

        if (collision.collider.CompareTag("Enemy"))
        {
            hit = true;
            hitTarget = collision.gameObject;
            gameObject.SetActive(false); // hides the gameobject from view
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
    }

    public void Initialize(Vector2 dest, float speed, float range)
    {
        lifeTime = range / speed;
        dir = (dest - (Vector2)transform.position).normalized;
        this.speed = speed;
    }

    public bool HasExpired() { return hit || lifeTime <= 0; }

    public GameObject GetHitTarget() { return hitTarget; }
}
