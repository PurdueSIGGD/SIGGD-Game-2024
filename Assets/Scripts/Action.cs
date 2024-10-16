using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action
{
    public Transform hitBox;
    public float coolDown;
    public float priority;
    
    private float maxCD;

    public Action(Transform hitBox, float coolDown, float priority)
    {
        this.hitBox = hitBox;
        this.maxCD = coolDown;
        this.coolDown = 0;
        this.priority = priority;
    }

    public virtual bool InAttackRange()
    {
        Collider[] c = Physics.OverlapBox(hitBox.position, hitBox.lossyScale / 2, hitBox.rotation, LayerMask.GetMask("Player"));
        return c.Length > 0;
    }

    public bool Ready()
    {
        return coolDown <= 0;
    }

    public void ResetCD()
    {
        coolDown = maxCD;
    }
}
