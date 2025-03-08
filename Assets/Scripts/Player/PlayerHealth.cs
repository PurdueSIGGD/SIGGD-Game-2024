using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    // Start is called before the first frame update
    // Integrate Script into main player script when main player script exists 

    public int health; // Health of player
    public bool isAlive = true; // Checks if player is still alive, if not Player lose (?)
    private StatManager stats;

    public void Start() {
        stats = GetComponent<StatManager>();
        health = (int) stats.ComputeValue("Max Health");
    }   

    public void TakeDamage(float damage)
    {

        health -= (int)damage;
        print("Player health ; " + health);
        if (health <= 0)
        {
            Kill();
        }
    }

    /// <summary>
    /// Immediately kills the player.
    /// </summary>
    public void Kill()
    {
        Destroy(this.gameObject);
    }


    public float Damage(DamageContext context, GameObject attacker)
    {
        return 0f;
    }

    public float Heal(HealingContext context, GameObject healer)
    {
        return 0f;
    }

}
