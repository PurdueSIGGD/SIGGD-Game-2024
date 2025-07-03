using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script for Shielded Riot Police's shield
/// </summary>
public class PoliceShield : MonoBehaviour
{
    [SerializeField] private DamageContext chargeDamage;
    private bool isCharging = false;

    [SerializeField] float damageVal;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        chargeDamage.damage = damageVal;

        // TODO implement knockback
        if (!isCharging || !collision.gameObject.CompareTag("Player"))
        {
            return;
        }
        GameObject player = collision.gameObject;

        player.GetComponent<Health>()?.Damage(chargeDamage, transform.parent.gameObject);
        gameObject.GetComponentInParent<Animator>().SetBool("HasCollided", true);

        SetCharging(false);
    }

    public void SetCharging(bool isCharging)
    {
        this.isCharging = isCharging;
    }
}
