using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Attach this script to player to make the player invisible to all enemies while the script is attached.
 * Remove this script from player to remove invisibility.
 */

public class Invisible : MonoBehaviour
{
    private SpriteRenderer playerSpriteRenderer;
    public float invisibilityAlpha = 0.5f;  // 0-1 alpha value where 0f is completely invisible, 1f is fully visible
    private void Start()
    {
        playerSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        Color playerColor = playerSpriteRenderer.color;
        playerColor.a = 0.0f;

        playerSpriteRenderer.color = playerColor;

        Debug.Log("turn on invisibility");
    }

    private void OnDestroy()
    {
        playerSpriteRenderer.color = Color.white;

        Debug.Log("turn off invisibility");
    }
}
