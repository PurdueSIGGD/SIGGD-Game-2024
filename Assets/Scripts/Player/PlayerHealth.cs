using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    // Start is called before the first frame update
    // Integrate Script into main player script when main player script exists 

    public int health = 100; // Health of player
    public bool isAlive = true; // Checks if player is still alive, if not Player lose (?)


    public void takeDamage(int damage)
    {

        health -= damage;
        print(health);

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

}
