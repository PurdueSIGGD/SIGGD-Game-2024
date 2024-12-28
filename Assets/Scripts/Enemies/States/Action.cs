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
    public string name;
    public float priority;
    public bool ready = true;
    [SerializeField] private Transform hitBox; // The area in which if a player is inside, the action will be performed
    [SerializeField] private float coolDown;
    [SerializeField] private AnimationClip animationClip;

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
