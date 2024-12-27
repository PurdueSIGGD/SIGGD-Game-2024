using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Enemy AI for a Shielded Riot Police
/// </summary>
public class ShieldPolice : EnemyStateManager
{
    [SerializeField] protected Transform batonTrigger;
    [SerializeField] protected Transform chargeTrigger;
    [SerializeField] protected float chargeSpeed;

    protected bool isCharging;
    protected Vector2 chargePos;

    void Update()
    {
        if (isCharging)
        {
            rb.velocity = new Vector2(chargeSpeed, rb.velocity.y) * transform.right;
        }
    }

    // Shielded Police will draw actions greedily
    protected override ActionPool GenerateActionPool()
    {
        Action batonSwing = new(batonTrigger, 0.0f, 3f, "Shield_Police_Swing");
        Action shieldCharge = new(chargeTrigger, 10.0f, 3f, "Shield_Police_Charge_1");

        Action move = new(null, 0.0f, 0.0f, "Shield_Police_Run");
        Action idle = new(null, 0.0f, 0.0f, "Shield_Police_Idle");

        return new GreedyActionPool(new List<Action> { batonSwing, shieldCharge }, move, idle);
    }

    // Generate damage frame for baton swing
    protected void OnBatonEvent()
    {
        GenerateDamageFrame(batonTrigger, 1.0f);
    }

    // Ask police to begin charging and enable shield damage
    protected void OnChargeEvent1()
    {
        GetComponentInChildren<PoliceShield>().ToggleCollision();
        isCharging = true;
        chargePos = player.position;
    }

    // Call on impact with the player to disable shield damage
    protected void OnChargeEvent2()
    {
        isCharging = false;
    }

    protected override void OnFinishAnimation()
    {
        base.OnFinishAnimation();
        animator.SetBool("HasCollided", false);
    }

    // Draws the Enemy attack range in the editor
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireCube(batonTrigger.position, batonTrigger.lossyScale);
        Gizmos.DrawWireCube(chargeTrigger.position, chargeTrigger.lossyScale);
    }
}