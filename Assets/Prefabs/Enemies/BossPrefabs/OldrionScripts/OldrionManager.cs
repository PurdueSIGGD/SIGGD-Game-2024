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

    private void OnEnable()
    {
        GameplayEventHolder.OnDamageDealt += OnPlayerDamageTaken;
        GameplayEventHolder.OnDamageDealt += OnDamageTaken;
        GameplayEventHolder.OnDeath += OnPlayerDeath;
    }

    private void OnDisable()
    {
        GameplayEventHolder.OnDamageDealt -= OnPlayerDamageTaken;
        GameplayEventHolder.OnDamageDealt -= OnDamageTaken;
        GameplayEventHolder.OnDeath -= OnPlayerDeath;
    }

    protected override void Start()
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
            //if (curState == BusyState)
            //{
            //    busyStateTimer += Time.fixedDeltaTime;
            //    if (busyStateTimer >= 5.0f)
            //    {
            //        UnityEngine.Debug.Log("Oldrion AI stuck in busy state, forcing exit");
            //        busyStateTimer = 0.0f;
            //        curState.ExitState(this);
            //    }
            //}
            //else
            //{
            //    busyStateTimer = 0.0f;
            //}
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
        AudioManager.Instance.SFXBranch.PlaySFXTrack("OldrionLightAttack");
        AudioManager.Instance.VABranch.PlayVATrack("Oldrion Light Attack");
        LightDamageFrame();
        lightVisual1.SetActive(true);
        SetLastLightAttackPerformed(1);
        rb.velocity = new Vector2(lightSpeed, rb.velocity.y) * transform.right;
    }
    void OnEnterLight2()
    {
        // TODO: Audio
    }
    void OnStartLight2()
    {
        AudioManager.Instance.SFXBranch.PlaySFXTrack("OldrionLightAttack");
        AudioManager.Instance.VABranch.PlayVATrack("Oldrion Light Attack");
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

    void OnEnterHeavy()
    {
        AudioManager.Instance.SFXBranch.PlaySFXTrack("OldrionHeavyWindup");
    }
    void OnStartHeavy()
    {
        AudioManager.Instance.SFXBranch.PlaySFXTrack("OldrionHeavyAttack");
        AudioManager.Instance.VABranch.PlayVATrack("Oldrion Heavy Attack");
        heavyDamage.damage = heavyDamageVal;
        GenerateDamageFrame(heavyTrigger.position, heavyTrigger.lossyScale.x, heavyTrigger.lossyScale.y, heavyDamage, gameObject);
        heavyVisual.SetActive(true);
    }
    void EndHeavy()
    {
        heavyVisual.SetActive(false);
    }

    void DashEnter()
    {
        AudioManager.Instance.SFXBranch.PlaySFXTrack("RoninDashWindup");
        AudioManager.Instance.SFXBranch.PlaySFXTrack("OldrionHeavyWindup");
    }
    void DashStart()
    {
        AudioManager.Instance.SFXBranch.PlaySFXTrack("OldrionDash");
        AudioManager.Instance.VABranch.PlayVATrack("Oldrion Dash Attack");
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
        AudioManager.Instance.VABranch.PlayVATrack("Oldrion Combo Start");
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

    public void StopAuraFarming()
    {
        if (curState == BusyState) // if Oldrion is stuck in Busy in Idle anim
        {
            pool.GetActionByName("Light").PlayNoCD(this);
        }
    }



    public void OnDamageTaken(DamageContext context)
    {
        if (context.victim != gameObject) return;

        if (gameObject.GetComponent<Health>().currentHealth <= 0f)
        {
            AudioManager.Instance.VABranch.PlayVATrack("Oldrion Significant Damage Taken");
            return;
        }
        AudioManager.Instance.VABranch.PlayVATrack("Oldrion Light Damage Taken");
    }

    public void OnPlayerDamageTaken(DamageContext context)
    {
        if (!context.victim.CompareTag("Player")) return;

        AudioManager.Instance.VABranch.PlayVATrack("Oldrion Damaging Player");
    }

    public void OnPlayerDeath(DamageContext context)
    {
        if (!context.victim.CompareTag("Player")) return;

        AudioManager.Instance.VABranch.PlayVATrack("Oldrion Damaging Player");
    }
}
