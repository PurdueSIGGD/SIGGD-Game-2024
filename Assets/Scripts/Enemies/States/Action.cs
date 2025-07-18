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
    [SerializeField] bool usesCircleHitbox = false;
    [SerializeField] private float coolDown;
    [SerializeField] private AnimationClip animationClip;

    /// <summary>
    /// Executes the Action
    /// </summary>
    /// <param name="animator"> The Enemy's animator component </param>
    /// <param name="fadeDuration"> Optional transition duration for the animation </param>
    public void Play(EnemyStateManager enemy, float fadeDuration = 0.2f)
    {
        enemy.animator.CrossFade(animationClip.name, fadeDuration);
        enemy.pool.DoCoolDown(this);
    }

    /// <summary>
    /// Finds if a Player is in range
    /// </summary>
    /// <returns> True if there is a player in range </returns>
    public virtual bool InAttackRange()
    {
        return usesCircleHitbox

                ? Physics2D.OverlapBox(
                    hitBox.position,
                    hitBox.lossyScale, 0f,
                    LayerMask.GetMask("Player"))

                : Physics2D.OverlapCircle(hitBox.position,
                    hitBox.lossyScale.x + hitBox.lossyScale.y / 2,
                    LayerMask.GetMask("Player"));
    }

    public float GetPriority()
    {
        return this.priority;
    }

    public float GetCoolDown()
    {
        return coolDown;
    }
}
