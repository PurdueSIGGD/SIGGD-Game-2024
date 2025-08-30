using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class OldrionManager : EnemyStateManager
{

    OldrionComboManager comboManager;
    bool combo;

    [Header("Light vals")]
    [SerializeField] DamageContext lightDamage;
    [SerializeField] float lightDamageVal;
    [SerializeField] GameObject lightVisual1;
    [SerializeField] GameObject lightVisual2;
    int lastLightAttackPerformed; // either 1 or 2
    [SerializeField] Transform lightTrigger;
    [SerializeField] float lightSpeed;
    [Header("Heavy vals")]
    [SerializeField] DamageContext heavyDamage;
    [SerializeField] float heavyDamageVal;
    [SerializeField] GameObject heavyVisual;
    [SerializeField] Transform heavyTrigger;
    [Header("Dash vals")]
    [SerializeField] DamageContext dashDamage;
    [SerializeField] float dashDamageVal;
    [SerializeField] float dashSpeed;
    [SerializeField] Transform dashTrigger;
    [SerializeField] Collider2D dashCollider;
    bool crushing; // mirrors the boolean variable of the same name in oldrionController

    void Start()
    {
        base.Start();
        comboManager = GetComponent<OldrionComboManager>();
    }

    protected override void FixedUpdate()
    {
        if (crushing || combo)
        {
            SwitchState(BusyState);
        }
        else
        {
            base.FixedUpdate();
        }
    }
    public override bool HasLineOfSight(bool tracking)
    {
        // override L.O.S. calculation to be really super generous to the mage rather than require direct L.O.S.
        return Physics2D.OverlapCircle(transform.position, 100, LayerMask.GetMask("Player"));
    }

    void LightDamageFrame()
    {
        lightDamage.damage = lightDamageVal;
        GenerateDamageFrame(lightTrigger.position, lightTrigger.lossyScale.x, lightTrigger.lossyScale.y, lightDamage, gameObject);
    }
    void OnEnterLight1()
    {
        animator.ResetTrigger("light1_recent");
        if (lastLightAttackPerformed == 1)
            animator.SetTrigger("light1_recent");
    }
    void OnStartLight1()
    {
        LightDamageFrame();
        lightVisual1.SetActive(true);
        SetLastLightAttackPerformed(1);
        rb.velocity = new Vector2(lightSpeed, rb.velocity.y) * transform.right;
    }
    void OnStartLight2()
    {
        LightDamageFrame();
        lightVisual2.SetActive(true);
        SetLastLightAttackPerformed(2);
        rb.velocity = new Vector2(lightSpeed, rb.velocity.y) * transform.right;
    }
    void EndLight()
    {
        lightVisual1.SetActive(false);
        lightVisual2.SetActive(false);
        rb.velocity = Vector2.zero;
    }

    void OnStartHeavy()
    {
        heavyDamage.damage = heavyDamageVal;
        GenerateDamageFrame(heavyTrigger.position, heavyTrigger.lossyScale.x, heavyTrigger.lossyScale.y, heavyDamage, gameObject);
        heavyVisual.SetActive(true);
    }
    void EndHeavy()
    {
        heavyVisual.SetActive(false);
    }

    void DashStart()
    {
        rb.velocity = new Vector2(dashSpeed, rb.velocity.y) * transform.right;
        dashCollider.enabled = true;
    }
    void EndDash()
    {
        rb.velocity = Vector2.zero;
        dashCollider.enabled = false;
    }
    public void OnDashHit(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            dashDamage.damage = dashDamageVal;
            col.gameObject.GetComponent<Health>().Damage(dashDamage, gameObject);
            EndDash();
        }
    }
    public void AoeDamage(Transform tform, DamageContext context)
    {
        GenerateDamageFrame(tform.position, tform.lossyScale.x, tform.lossyScale.y, context, this.gameObject);
    }
    void OnEnterComboSeed()
    {
        combo = true;
        comboManager.StartCombo();
        PlayNextActionFromCombo();
    }
    void OnExitComboSeed()
    {
        print("EXIT!");
        combo = false;
        OnFinishAnimation();
    }
    void PlayNextActionFromCombo()
    {
        // manual flip to face player

        if (player.position.x - transform.position.x < 0)
        {
            Flip(false);
        }
        else
        {
            Flip(true);
        }

        Action nextAction = comboManager.GetNextAction();
        print(nextAction);
        print(nextAction.name);
        nextAction.PlayNoCD(GetComponent<EnemyStateManager>(), 0.0f);
    }
    protected override void OnFinishAnimation()
    {
        StopAllActions();
        if (combo)
        {
            PlayNextActionFromCombo();
        }
        else
        {
            base.OnFinishAnimation();
        }
    }
    protected override void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(lightTrigger.position, lightTrigger.lossyScale);
        Gizmos.DrawWireCube(heavyTrigger.position, heavyTrigger.lossyScale);
        Gizmos.DrawWireCube(dashTrigger.position, dashTrigger.lossyScale);
        base.OnDrawGizmos();
    }
    void SetLastLightAttackPerformed(int val)
    {
        lastLightAttackPerformed = val;
    }
    int GetLastLightAttackPerformed()
    {
        return lastLightAttackPerformed;
    }
    public void StopAllActions()
    {
        lightVisual1.SetActive(false);
        lightVisual2.SetActive(false);
        heavyVisual.SetActive(false);
        rb.velocity = Vector2.zero;
    }
    public void SetEnemyManagerCrushing(bool val)
    {
        crushing = val;
    }
}
