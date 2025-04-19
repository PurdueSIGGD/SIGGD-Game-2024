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
    private Camera mainCamera;

    [HideInInspector] public PoliceChiefManager manager;

    private bool isCharging = false;
    private float chargingTime = 0f;
    private bool isPrimed = false;
    private float primedTime = 0f;



    void Start()
    {
        playerStateMachine = GetComponent<PlayerStateMachine>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    private void Update()
    {
        if (isCharging && chargingTime > 0f) chargingTime -= Time.deltaTime;
        if (isCharging && chargingTime <= 0f) playerStateMachine.EnableTrigger("OPT");

        if (isPrimed && primedTime > 0f) primedTime -= Time.deltaTime;
        if (isPrimed && primedTime <= 0f) playerStateMachine.EnableTrigger("OPT");
    }



    // Charge Up
    public void StartSidearmChargeUp()
    {
        chargingTime = manager.GetStats().ComputeValue("Basic Charge Up Time");
        isCharging = true;
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
        GetComponent<Move>().PlayerStop();

        // Calculate shot aiming vector
        Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 pos = transform.position;
        Vector2 dir = (mousePos - pos).normalized;

        // Fire shot
        GameObject sidearmShot = Instantiate(manager.basicShot, Vector3.zero, Quaternion.identity);
        sidearmShot.GetComponent<PoliceChiefSidearmShot>().fireSidearmShot(manager, pos, dir);
    }

    public void StopSidearm()
    {
        GetComponent<Move>().PlayerGo();
    }
}
