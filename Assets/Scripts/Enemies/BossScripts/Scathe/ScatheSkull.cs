using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ScatheSkull : MonoBehaviour
{
    [SerializeField] DamageContext damageContext;
    [SerializeField] float damage;
    [SerializeField] GameObject attacker;
    void Start()
    {
        damageContext.damage = damage;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<BossController>() != null)
            return;

        if (collision.gameObject.CompareTag("Player") ||
            collision.gameObject.CompareTag("Enemy"))
        {
            Health hp = collision.gameObject.GetComponent<Health>();
            hp.Damage(damageContext, attacker);
        }
    }
}
