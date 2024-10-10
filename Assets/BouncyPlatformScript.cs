using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Code for a trampoline platform that launches the player upon landing

public class BouncyPlatformScript : MonoBehaviour
{
    // Instantiates player and player's rigidbody component
    public GameObject player;
    public Rigidbody2D rb;

    // Variable for bounciness of platform, can be tweaked to increase 
    public int platformBounce;

    void Start()
    {
        //Gets rigidBody component of Player
        rb = player.GetComponent<Rigidbody2D>();

        //sets default bounciness for paltform
        platformBounce = 10;
    }
    
    //runs upon collision with player
    public void OnCollisionEnter2D(Collision2D collision)
    {
        //sets player vertical velocity to platformBounce variable
        rb.velocity = new Vector2(rb.velocity.x, platformBounce);
    }
}
