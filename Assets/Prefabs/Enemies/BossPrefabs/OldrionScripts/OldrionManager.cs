using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class OldrionManager : EnemyStateManager
{

    [Header("Light vals")]
    [SerializeField] DamageContext lightDamage;
    [SerializeField] float lightDamageVal;
    [SerializeField] GameObject lightVisual1;
    [SerializeField] GameObject lightVisual2;
    int lastLightAttackPerformed; // either 1 or 2
    [SerializeField] Transform lightTrigger;
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

    protected override void FixedUpdate()
    {
        if (crushing)
        {
            SwitchState(BusyState);
        }
        else
        {
            base.FixedUpdate();
        }
    }

    void LightDamageFrame()
    {
        lightDamage.damage = lightDamageVal;
        GenerateDamageFrame(lightTrigger.position, lightTrigger.lossyScale.x, lightTrigger.lossyScale.y, lightDamage, gameObject);
    }
    void OnStartLight1()
    {
        LightDamageFrame();
        lightVisual1.SetActive(true);
        SetLastLightAttackPerformed(1);
    }
    void OnStartLight2()
    {
        LightDamageFrame();
        lightVisual2.SetActive(true);
        SetLastLightAttackPerformed(2);
    }
    void EndLight()
    {
        lightVisual1.SetActive(false);
        lightVisual2.SetActive(false);
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

    protected override void OnFinishAnimation()
    {
        ComboCheck();
        base.OnFinishAnimation();
    }
    void ComboCheck()
    {
        print("what a bum u are >:(");
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
    public void DisableAllVFX()
    {
        lightVisual1.SetActive(false);
        lightVisual2.SetActive(false);
    }
    public void SetEnemyManagerCrushing(bool val)
    {
        crushing = val;
    }
}
