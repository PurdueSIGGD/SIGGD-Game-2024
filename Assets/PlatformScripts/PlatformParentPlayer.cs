using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enables moving platforms to automatically parent the player to make the moving platform functional 
/// </summary>
public class PlatformParentPlayer : MonoBehaviour
{
    //make it so the player moves with the platform and not just the platform moving
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //when player collides w platformm set the platform as the parent object of the object that is colliding w the platform
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }

    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        //when the player exits the collision set it back to normal
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}
