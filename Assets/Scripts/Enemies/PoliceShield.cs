using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script for Shielded Riot Police's shield
/// </summary>
public class PoliceShield : MonoBehaviour
{
    [SerializeField] private float chargeDamage;
    private bool isCharging = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // TODO implement knockback
        if (!isCharging || !collision.gameObject.CompareTag("Player"))
        {
            return;
        }
        GameObject player = collision.gameObject;

        player.GetComponent<PlayerHealth>().TakeDamage(chargeDamage);
        gameObject.GetComponentInParent<Animator>().SetBool("HasCollided", true);

        ToggleCollision();
    }

    public void ToggleCollision()
    {
        isCharging = !isCharging;
    }
}
