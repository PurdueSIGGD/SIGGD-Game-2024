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
    protected Vector2 chargePos;
    [SerializeField] bool shieldUp;

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

    void SetCharging(bool isCharging)
    {
        this.isCharging = isCharging;
    }
    void ShieldUp()
    {
        shieldUp = true;

    }
    void ShieldDown()
    {
        shieldUp = false;
    }

    // Generate damage frame for baton swing
    protected void OnBatonEvent()
    {
        ShieldDown();
        batonDamage.damage = batonDamageVal;
        GenerateDamageFrame(batonTrigger.position, batonTrigger.lossyScale.x, batonTrigger.lossyScale.y, batonDamage, gameObject);
    }

    // Ask police to begin charging and enable shield damage
    protected void OnChargeEvent1()
    {
        SetCharging(true);
        //chargePos = new Vector3(player.position.x, player.position.y, player.position.z);
    }

    // Call on impact with the player to disable shield damage
    protected void OnChargeEvent2()
    {
        SetCharging(false);
    }

    public void ProcessShieldCollision(Collision2D collision)
    {
        // TODO implement knockback
        if (!isCharging)
        {
            return;
        }
        gameObject.GetComponent<Animator>().SetBool("HasCollided", true);
        if (!collision.gameObject.CompareTag("Player"))
        {
            SetCharging(false);
            return;
        }
        GameObject player = collision.gameObject;
        Health playerHealth = player.GetComponent<Health>();

        chargeDamage.damage = chargeDamageVal;
        playerHealth.Damage(chargeDamage, transform.gameObject);
        SetCharging(false);
    }

    public void OnChargeEnd()
    {
        animator.SetBool("HasCollided", false);
        float direction = new Vector2(rb.velocity.x, 0).normalized.x;
        rb.velocity = new Vector2(-0.25f * direction * math.abs(rb.velocity.x), rb.velocity.y);
        SetCharging(false);
    }

    protected override void OnFinishAnimation()
    {
        base.OnFinishAnimation();
    }

    // Draws the Enemy attack range in the editor
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireCube(batonTrigger.position, batonTrigger.lossyScale);
        Gizmos.DrawWireCube(chargeTrigger.position, chargeTrigger.lossyScale);
    }
}