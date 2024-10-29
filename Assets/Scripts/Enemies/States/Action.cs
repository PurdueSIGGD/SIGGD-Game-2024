using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class Action
{
    public Transform hitBox;
    public float coolDown;
    public float priority;
    public string anim;
    
    private float maxCD;

    public Action(Transform hitBox, float coolDown, float priority, string anim)
    {
        this.hitBox = hitBox;
        this.maxCD = coolDown;
        this.coolDown = 0;
        this.priority = priority;
        this.anim = anim;
    }

    public void Play(Animator animator, float fadeDuration = 0.2f)
    {
        ResetCD();
        animator.CrossFade(anim, fadeDuration);
    }

    public virtual bool InAttackRange()
    {
        return Physics2D.OverlapBox(hitBox.position, hitBox.lossyScale, 0f, LayerMask.GetMask("Player"));
    }

    public bool Ready()
    {
        return coolDown <= 0;
    }

    public void UpdateCD()
    {
        coolDown -= Time.deltaTime;
    }

    public void ResetCD()
    {
        coolDown = maxCD;
    }
}
