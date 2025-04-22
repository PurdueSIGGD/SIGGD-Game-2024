using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script for Idol's clone, destroys self after duration time has passed
/// </summary>
public class IdolClone : MonoBehaviour
{
    private IdolManager manager;
    [SerializeField] public float duration; // duration clone can last
    [SerializeField] private float inactiveModifier;

    [Header("Used by clone to kill self")]
    [SerializeField] DamageContext expireContext = new DamageContext();
    private GameObject player;
    //private IdolManager manager;

    void Update()
    {
        TickTimer();
    }

    void TickTimer()
    {
        if (duration <= 0)
        {
            DeallocateDecoy();
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
        this.manager = manager;
    }

    public void DeallocateDecoy()
    {
        if (manager)
        {
            manager.clones.Remove(gameObject);
        }
        expireContext.victim = gameObject;
        GameplayEventHolder.OnDeath.Invoke(expireContext);
        Destroy(gameObject);
    }
}
