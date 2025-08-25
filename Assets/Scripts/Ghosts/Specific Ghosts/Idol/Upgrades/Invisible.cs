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
    public float invisibilityAlpha = 0.25f;  // 0-1 alpha value where 0f is completely invisible, 1f is fully visible
    private void Start()
    {
        playerSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        Material playerMaterial = playerSpriteRenderer.material;
        Color playerColor = playerMaterial.GetColor("_BaseColor");

        playerColor.r = 0f;
        playerColor.g = 1f;
        playerColor.b = 165f / 255f;
        playerColor.a = invisibilityAlpha;

        playerMaterial.SetColor("_BaseColor", playerColor);

        //Debug.Log("turn on invisibility");
    }

    private void OnDestroy()
    {
        Material playerMaterial = playerSpriteRenderer.material;
        Color playerColor = playerMaterial.GetColor("_BaseColor");

        playerColor.r = 1f;
        playerColor.g = 1f;
        playerColor.b = 1f;
        playerColor.a = 1f;

        playerMaterial.SetColor("_BaseColor", playerColor);

        //Debug.Log("turn off invisibility");
    }
}
