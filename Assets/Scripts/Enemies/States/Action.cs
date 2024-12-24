using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

/// <summary>
/// An Action that an Enemy can take
/// </summary>
public class Action
{
    public bool ready = true; // Could change later so certain actions may not be immediately accessible at the start of encounter

    private Transform hitBox;
    private float maxCD;
    private float coolDown;
    private float priority;
    private string anim;
    
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
        animator.CrossFade(anim, fadeDuration);
        DoCoolDown();
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
    /// Resets the Action's cooldown
    /// </summary>
    public void ResetCD()
    {
        coolDown = maxCD;
    }

    public float GetPriority()
    {
        return this.priority;
    }

    /// <summary>
    /// Make this action go into cooldown
    /// </summary>
    private async Task DoCoolDown()
    {
        ready = false;
        await Task.Delay((int)(coolDown * 1000));
        ready = true;
    }
}
