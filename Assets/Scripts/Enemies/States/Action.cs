using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

/// <summary>
/// An Action that an Enemy can take
/// </summary>
[Serializable]
public class Action
{
    [SerializeField] public string name;
    [SerializeField] private Transform hitBox; // The area in which if a player is inside, the action will be performed
    [SerializeField] private float coolDown;
    [SerializeField] private AnimationClip animationClip;
    public float priority;
    public bool ready = true;

    /// <summary>
    /// Constructs an Action for a particular Enemy
    /// </summary>
    /// <param name="hitBox"> The area in which if a player is inside, the action will be performed </param>
    /// <param name="coolDown"> Length of the Action's cool down </param>
    /// <param name="priority"> Tendency to do this Action (1-10)  </param>
    /// <param name="anim"> Name of the animation to be played </param>
    public Action(Transform hitBox, float coolDown, float priority, AnimationClip anim)
    {
        this.hitBox = hitBox;
        this.coolDown = coolDown;
        this.priority = priority;
        this.animationClip = anim;
        this.ready = true;
    }

    /// <summary>
    /// Executes the Action
    /// </summary>
    /// <param name="animator"> The Enemy's animator component </param>
    /// <param name="fadeDuration"> Optional transition duration for the animation </param>
    public void Play(Animator animator, float fadeDuration = 0.2f)
    {
        animator.CrossFade(animationClip.name, fadeDuration);
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

    public float GetPriority()
    {
        return this.priority;
    }

    public bool IsReady()
    {
        return ready;
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
