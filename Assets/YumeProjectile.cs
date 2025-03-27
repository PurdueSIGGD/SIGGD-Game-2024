using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YumeProjectile : MonoBehaviour
{
    float lifeTime;
    bool hit = false; // if the projectile has hit an enemy
    GameObject hitTarget;


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


    public bool hasExpired() { return hit || lifeTime <= 0; }

    public GameObject getHitTarget() { return hitTarget; }
}
