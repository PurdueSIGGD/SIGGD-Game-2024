using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ronin : EnemyStateManager
{

    [Header("Sword Attack")]
    [SerializeField] protected Transform swordTrigger;
    [SerializeField] protected DamageContext swordDamage;
    [SerializeField] float swordDamageVal;
    [Header("Dash Attack")]
    [SerializeField] Collider2D dashCollider;
    [SerializeField] protected Transform dashTrigger;
    [SerializeField] private DamageContext dashDamage;
    [SerializeField] float dashDamageVal;
    [SerializeField] protected float dashSpeed;
    [SerializeField] protected bool isDashing;
    [SerializeField] protected bool canDashSwing = false;

    private void OnEnable()
    {
        GameplayEventHolder.OnEntityStunned += OnRoninStunned;
    }

    private void OnDisable()
    {
        GameplayEventHolder.OnEntityStunned -= OnRoninStunned;
    }

    void OnSwordStart()
    {
        AudioManager.Instance.SFXBranch.PlaySFXTrack("RoninSwordWindup");
    }

    void SwingSword()
    {
        AudioManager.Instance.SFXBranch.PlaySFXTrack("RoninSword");
        swordDamage.damage = swordDamageVal;
        GenerateDamageFrame(swordTrigger.position, swordTrigger.lossyScale.x, swordTrigger.lossyScale.y, swordDamage, gameObject);
    }
    protected void OnSwordEvent()
    {
        SwingSword();
    }

    protected void OnEnterDashEvent()
    {
        AudioManager.Instance.SFXBranch.PlaySFXTrack("RoninDashWindup");
    }
    protected void DashStart()
    {
        AudioManager.Instance.SFXBranch.PlaySFXTrack("RoninDash");
        rb.mass = 3f;
        rb.velocity = new Vector2(dashSpeed, rb.velocity.y) * transform.right;
        dashCollider.enabled = true;
        isDashing = true;
    }
    protected void EndDash()
    {
        //rb.mass = 1f;
        rb.velocity = Vector2.zero;
        dashCollider.enabled = false;
        if (canDashSwing) animator.ResetTrigger("dash_hit");
        isDashing = false;
    }
    public void OnDashHit(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player") || col.gameObject.CompareTag("Idol_Clone"))
        {
            print("hit player!");
            dashDamage.damage = dashDamageVal;
            col.gameObject.GetComponent<Health>().Damage(dashDamage, gameObject);
            EndDash();
            if (canDashSwing) animator.SetTrigger("dash_hit");
        }
    }
    protected override void OnFinishAnimation()
    {
        rb.mass = 1f;
        base.OnFinishAnimation();
    }
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireCube(swordTrigger.position, swordTrigger.lossyScale);
        Gizmos.DrawWireCube(dashTrigger.position, dashTrigger.lossyScale);
    }

    public void OnRoninStunned(GameObject stunnedEntity)
    {
        if (stunnedEntity != gameObject) return;
        if (isDashing) EndDash();
    }
}
