using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enables moving platforms to automatically parent the player to make the moving platform functional 
/// </summary>
public class PlatformParentPlayer : MonoBehaviour
{
    public Vector3 lastPlatformPosition;
    public Vector3 playerpos;
    public GameObject player;
    private bool playerOnPlatform = false;

    void Start()
    {
        lastPlatformPosition = transform.position;
        playerpos = transform.position;
    }

    void Update()
    {
        //Platform position is updated
        lastPlatformPosition = transform.position;
    }

    void LateUpdate()
    { //player position is updated according to the platform movement

        if (playerOnPlatform && player != null)
        {
            Vector3 platformMovement = transform.position - lastPlatformPosition;
            player.transform.position += platformMovement;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Empty platform turns into collision object to get the possition
        if (collision.gameObject.CompareTag("Player"))
        {
                player = collision.gameObject;
                playerOnPlatform = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //player empty object becomes empty again
        if (collision.gameObject.CompareTag("Player"))
        {
            playerOnPlatform = false;
            player = null;
        }
    }
}
