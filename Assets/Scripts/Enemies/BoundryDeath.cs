using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundryDeath : MonoBehaviour
{
    [SerializeField] DamageContext damageContext;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Boundary"))
        {
            damageContext.damage = GetComponent<Health>().currentHealth;
            GetComponent<Health>().Damage(damageContext, gameObject);
        }
    }
}
