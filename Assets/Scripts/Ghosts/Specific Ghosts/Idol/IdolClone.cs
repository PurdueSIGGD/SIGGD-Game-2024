using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script for Idol's clone, destroys self after duration time has passed
/// </summary>
public class IdolClone : MonoBehaviour
{
    [SerializeField] float duration = 6.0f; // duration clone can last
    private GameObject player;

    void Update()
    {
        if (duration <= 0)
        {
            Destroy(gameObject);
        }
        if (player.GetComponent<IdolSpecial>())
        {
            duration -= Time.deltaTime;
        }
        else // if player is no longer in idol mode, count down twice as fast
        {
            duration -= Time.deltaTime * 2;
        }
    }

    /// <summary>
    /// Pass reference of player to clone, so to check which ghost player is
    /// using
    /// </summary>
    /// <param name="player"> player gameobject </param>
    public void Initialize(GameObject player)
    {
        this.player = player;
    }
}
