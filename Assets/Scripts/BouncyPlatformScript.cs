using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Code for a trampoline platform that launches the player upon landing
/// </summary>
public class BouncyPlatformScript : MonoBehaviour
{
    // Instantiates player and player's rigidbody component
    private GameObject player;
    private Rigidbody2D playerRb;

    // Variable for bounciness of platform, can be tweaked to increase 
    [SerializeField] int platformBounce;

    void Start()
    {
        //Gets player object
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            print("ERROR: no player tag in scene");
        }
        //Gets rigidBody component of Player
        playerRb = player.GetComponent<Rigidbody2D>();
        //sets default bounciness for paltform
        if (platformBounce == 0)
        {
            platformBounce = 10;
        }
    }

    //runs upon collision with player
    public void OnCollisionEnter2D(Collision2D collision)
    {
        //sets player vertical velocity to platformBounce variable
        if (collision.gameObject.CompareTag("Player"))
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x, platformBounce);
        }

    }
}
