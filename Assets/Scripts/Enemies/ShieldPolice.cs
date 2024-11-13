using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class ShieldPolice : EnemyStateManager
{
    [SerializeField] protected Transform batonHit;
    [SerializeField] protected Transform chargeHit;

    protected bool isCharging;
    protected Vector2 chargePos;

    void Update()
    {
        if (isCharging)
        {
            rb.velocity = new Vector2(speed * 4, rb.velocity.y) * transform.right;
        }
    }

    protected override ActionPool GenerateActionPool()
    {
        Action batonSwing = new(batonHit, 2.0f, 3f, "Shield_Police_Swing");
        Action shieldCharge = new(chargeHit, 10.0f, 3f, "Shield_Police_Charge_1");

        Action move = new(null, 0.0f, 0.0f, "Shield_Police_Run");
        Action idle = new(null, 0.0f, 0.0f, "Shield_Police_Idle");

        return new ActionPool(new List<Action> { batonSwing, shieldCharge }, move, idle);
    }

    protected void OnBatonEvent()
    {
        Collider2D hit = Physics2D.OverlapBox(batonHit.position, batonHit.lossyScale, 0f, LayerMask.GetMask("Player"));
        if (hit)
        {
            hit.GetComponent<PlayerHealth>().takeDamage(15);
        }
    }

    protected void OnCharge1Event()
    {
        //GetComponentInChildren<PoliceShield>().ToggleCollision();
        isCharging = true;
        chargePos = player.position;
    }

    protected override void OnFinishAnimation()
    {
        isCharging = false;
        base.OnFinishAnimation();
    }

    // Draws the Enemy attack range in the editor
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireCube(batonHit.position, batonHit.lossyScale);
        Gizmos.DrawWireCube(chargeHit.position, chargeHit.lossyScale);
    }
}