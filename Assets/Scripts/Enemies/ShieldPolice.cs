using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Enemy AI for a Shielded Riot Police
/// </summary>
public class ShieldPolice : EnemyStateManager
{

    PoliceShield shield;

    [Header("Baton Attack")]
    [SerializeField] protected Transform batonTrigger;
    [SerializeField] protected DamageContext batonDamage;
    [SerializeField] float batonDamageVal;

    [Header("Charge Attack (See shield to define damage)")]
    [SerializeField] protected Transform chargeTrigger;
    [SerializeField] private DamageContext chargeDamage;
    [SerializeField] float chargeDamageVal;
    [SerializeField] protected float chargeSpeed;

    [SerializeField] protected bool isCharging;
    [SerializeField] bool shieldUp;

    void OnDestroy()
    {
        GameplayEventHolder.OnDamageFilter.Remove(ShieldUpDamageFilter);
    }

    void Update()
    {
        if (shield == null)
        {
            shield = GetComponentInChildren<PoliceShield>();
        }
        if (isCharging)
        {
            rb.velocity = new Vector2(chargeSpeed, rb.velocity.y) * transform.right;
        }
    }
    void ShieldHit()
    {
        print("CCCLANK chhhhhHHHH (the real sound of a kopesh hitting a shield)");
    }
    public void ShieldUpDamageFilter(ref DamageContext context)
    {
        if (context.raycastHitPosition == Vector2.zero)
        {
            return;
        }
        float horizontalDifference = context.raycastHitPosition.x - gameObject.transform.position.x;
        float direction = transform.rotation.y == 0 ? 1 : -1;
        float netHitDirection = horizontalDifference * direction; // positive number if hit from the front, negative if hit from behind
        if (netHitDirection > 0)
        {
            context.damage = 0;
            ShieldHit();
        }
        return;
    }

    void SetCharging(bool isCharging)
    {
        this.isCharging = isCharging;
        if (!isCharging)
        {
            animator.ResetTrigger("LAUNCH!!!");
            ShieldDown();
        }
    }
    void ShieldUp()
    {
        shieldUp = true;
        GameplayEventHolder.OnDamageFilter.Add(ShieldUpDamageFilter);
    }
    void ShieldDown()
    {
        shieldUp = false;
        GameplayEventHolder.OnDamageFilter.Remove(ShieldUpDamageFilter);
    }
    public void IsTheStrangeInsectStillStandingRightInfrontOfMeLikeAnIdiotWhenImReadyAndReallyItchingToShieldBashTheirSkullIntoTheEarth()
    {
        if (Physics2D.OverlapBox(chargeTrigger.position, chargeTrigger.lossyScale, 0f, LayerMask.GetMask("Player")))
        {
            animator.SetTrigger("LAUNCH!!!");
        }
    }

    // Generate damage frame for baton swing
    protected void OnBatonEvent()
    {
        batonDamage.damage = batonDamageVal;
        GenerateDamageFrame(batonTrigger.position, batonTrigger.lossyScale.x, batonTrigger.lossyScale.y, batonDamage, gameObject);
    }

    // Ask police to begin charging and enable shield damage
    protected void OnChargeEvent1()
    {
        SetCharging(true);
    }

    // Call on impact with the player to disable shield damage
    protected void OnChargeEvent2()
    {
        SetCharging(false);
    }

    public void ProcessShieldCollision(Collider2D collider)
    {
        // TODO implement knockback
        if (!isCharging)
        {
            return;
        }

        if (!collider.gameObject.CompareTag("Player"))
        {
            SetCharging(false);
            return;
        }
        GameObject player = collider.gameObject;
        Health playerHealth = player.GetComponent<Health>();

        chargeDamage.damage = chargeDamageVal;
        playerHealth.Damage(chargeDamage, transform.gameObject);
        SetCharging(false);
    }

    public void OnChargeEnd()
    {
        float direction = new Vector2(rb.velocity.x, 0).normalized.x;
        Vector2 endForce = new Vector2(-0.8f * direction * math.abs(rb.velocity.x), 0);
        rb.AddForce(endForce, ForceMode2D.Impulse);
        SetCharging(false);
    }

    protected override void OnFinishAnimation()
    {
        base.OnFinishAnimation();
        SetCharging(false);
    }

    // Draws the Enemy attack range in the editor
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireCube(batonTrigger.position, batonTrigger.lossyScale);
        Gizmos.DrawWireCube(chargeTrigger.position, chargeTrigger.lossyScale);
    }
}