using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

/// <summary>
/// An Action that an Enemy can take
/// </summary>
public class Action
{
    public Transform hitBox;
    public float coolDown;
    public float priority;
    public string anim;
    
    private float maxCD;

    /// <summary>
    /// Constructs an Action for a particular Enemy
    /// </summary>
    /// <param name="hitBox"> Range of the Action </param>
    /// <param name="coolDown"> Length of the Action's cool down </param>
    /// <param name="priority"> Tendancy to do this Action (1-10)  </param>
    /// <param name="anim"> Name of the animation to be played </param>
    public Action(Transform hitBox, float coolDown, float priority, string anim)
    {
        this.hitBox = hitBox;
        this.maxCD = coolDown;
        this.coolDown = 0;
        this.priority = priority;
        this.anim = anim;
    }

    /// <summary>
    /// Executes the Action
    /// </summary>
    /// <param name="animator"> The Enemy's animator component </param>
    /// <param name="fadeDuration"> Optional transition duration for the animation </param>
    public void Play(Animator animator, float fadeDuration = 0.2f)
    {
        ResetCD();
        animator.CrossFade(anim, fadeDuration);
    }

    /// <summary>
    /// Finds if a Player is in range
    /// </summary>
    /// <returns> True if there is a player in range </returns>
    public virtual bool InAttackRange()
    {
        return Physics2D.OverlapBox(hitBox.position, hitBox.lossyScale, 0f, LayerMask.GetMask("Player"));
    }

    /// <summary>
    /// Finds if Action has finished cool down
    /// </summary>
    /// <returns> True if Action finished cool down </returns>
    public bool Ready()
    {
        return coolDown <= 0;
    }

    /// <summary>
    /// Updates the Action's cool down
    /// </summary>
    public void UpdateCD()
    {
        coolDown -= Time.deltaTime;
    }

    // Resets the Action's cool down
    private void ResetCD()
    {
        coolDown = maxCD;
    }
}
