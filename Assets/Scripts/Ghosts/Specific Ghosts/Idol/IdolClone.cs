using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script for Idol's clone, destroys self after duration time has passed
/// </summary>
public class IdolClone : MonoBehaviour
{
    [SerializeField] public float duration; // duration clone can last
    [SerializeField] private float inactiveModifier;
    private GameObject player;
    private IdolManager manager;

    void Update()
    {
        TickTimer();
    }

    void TickTimer()
    {
        print("Duration: " + duration);
        if (duration <= 0)
        {
            manager.startSpecialCooldown();
            Destroy(gameObject);
        }
        if (player.GetComponent<IdolSpecial>())
        {
            duration -= Time.deltaTime;
        }
        else // if player is no longer in idol mode, count down twice as fast
        {
            duration -= Time.deltaTime * inactiveModifier;
        }
    }

    /// <summary>
    /// Pass reference of player to clone, so to check which ghost player is
    /// using
    /// </summary>
    /// <param name="player"> player gameobject </param>
    public void Initialize(GameObject player, IdolManager manager, float duration, float inactiveModifier)
    {
        this.player = player;
        this.manager = manager;
        this.duration = duration;
        this.inactiveModifier = inactiveModifier;
    }
}
