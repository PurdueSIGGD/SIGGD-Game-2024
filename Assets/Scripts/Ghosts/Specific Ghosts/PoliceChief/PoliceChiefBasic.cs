using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

/// <summary>
/// Defines functions for PoliceChief Basic attack to be called through state machine behavior
/// Note: The effective lower bound for cooldown between shots is the animation length, as the state machine will
/// not proceed until the animation is finished running.
/// </summary>
public class PoliceChiefBasic : MonoBehaviour
{
    private PlayerStateMachine playerStateMachine;
    private Animator animator;
    private Camera mainCamera;

    [HideInInspector] public PoliceChiefManager manager;

    private bool isCharging = false;
    public float chargingTime = 0f;
    private bool isPrimed = false;
    private float primedTime = 0f;
    private float chargetimeChanger;



    void Start()
    {
        playerStateMachine = GetComponent<PlayerStateMachine>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        animator = GetComponent<Animator>();
        animator.SetBool("has_ammo", (manager.basicAmmo > 0));
    }

    private void Update()
    {
        if (isCharging && chargingTime > 0f) chargingTime -= Time.deltaTime;
        if (isCharging && chargingTime <= 0f) playerStateMachine.EnableTrigger("OPT");

        if (isPrimed && primedTime > 0f) primedTime -= Time.deltaTime;
        if (isPrimed && primedTime <= 0f) playerStateMachine.EnableTrigger("OPT");

        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 mouseDiff = transform.position - mousePos;

        if (isCharging || isPrimed)
        {
            if (mouseDiff.x < 0) // update player facing direction
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (mouseDiff.x > 0)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
    }



    // Charge Up
    public void StartSidearmChargeUp()
    {
        chargingTime = manager.GetStats().ComputeValue("Basic Charge Up Time");
        isCharging = true;
        AudioManager.Instance.SFXBranch.PlaySFXTrack("HeavyAttackWindUp");
        SetSidearmDamage(2);
        GetComponent<Move>().PlayerStop();
    }

    public void StopSidearmChargeUp()
    {
        isCharging = false;
        chargingTime = 0f;
        GetComponent<Move>().PlayerGo();
    }



    // Primed
    public void StartSidearmPrimed()
    {
        primedTime = manager.GetStats().ComputeValue("Basic Primed Autofire Time");
        isPrimed = true;
        AudioManager.Instance.SFXBranch.PlaySFXTrack("HeavyAttackPrimed");
        SetSidearmDamage(3);
        GetComponent<Move>().PlayerStop();
    }

    public void StopSidearmPrimed()
    {
        isPrimed = false;
        primedTime = 0f;
        GetComponent<Move>().PlayerGo();
    }



    // Sidearm Attack
    public void FireSidearm()
    {
        playerStateMachine.ConsumeHeavyAttackInput();
        GetComponent<Move>().PlayerStop();

        // Calculate shot aiming vector
        Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 pos = transform.position;
        Vector2 dir = (mousePos - pos).normalized;

        // Fire shot
        GameObject sidearmShot = Instantiate(manager.basicShot, Vector3.zero, Quaternion.identity);
        sidearmShot.GetComponent<PoliceChiefSidearmShot>().fireSidearmShot(manager, pos, dir);
        manager.basicAmmo -= 1;
        animator.SetBool("has_ammo", (manager.basicAmmo > 0));
    }

    public void StopSidearm()
    {
        SetSidearmDamage(1);
        GetComponent<Move>().PlayerGo();
    }

    /// <summary>
    /// Modifies the Police Chief's Basic Ability damage context based on the given attack type.
    /// </summary>
    /// <param name="attackType">1 = Light Attack, 2 = Heavy Attack, 3 = Super Heavy Attack</param>
    private void SetSidearmDamage(int attackType)
    {
        switch (attackType)
        {
            case 1:
                manager.basicDamage.damage = manager.GetStats().ComputeValue("Basic Damage");
                manager.basicDamage.damageStrength = DamageStrength.MINOR;
                break;
            case 2:
                manager.basicDamage.damage = manager.GetStats().ComputeValue("Basic Heavy Damage");
                manager.basicDamage.damageStrength = DamageStrength.LIGHT;
                break;
            case 3:
                manager.basicDamage.damage = manager.GetStats().ComputeValue("Basic Super Heavy Damage");
                manager.basicDamage.damageStrength = DamageStrength.MODERATE;
                break;
            default:
                manager.basicDamage.damage = manager.GetStats().ComputeValue("Basic Damage");
                manager.basicDamage.damageStrength = DamageStrength.MINOR;
                break;
        }
    }
}
