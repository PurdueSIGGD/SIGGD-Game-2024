using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PoliceChiefSpecial : MonoBehaviour
{
    private bool shouldChangeBack = true;
    private PlayerStateMachine playerStateMachine;
    private Animator camAnim;
    private Camera cam;
    [HideInInspector] public PoliceChiefManager manager;
    [HideInInspector] public bool isCharging = false;
    [HideInInspector] public bool isPrimed = false;
    private float chargingTime = 0f;

    private LockedAndLoadedSkill lockedAndLoaded;
    private PoliceChiefOvercharged overcharged;



    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        camAnim = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Animator>();
        playerStateMachine = GetComponent<PlayerStateMachine>();
    }

    void Update()
    {
        if (isCharging && chargingTime > 0f) chargingTime -= Time.deltaTime;
        if (isCharging && chargingTime <= 0f) playerStateMachine.EnableTrigger("OPT");

        if (manager != null && lockedAndLoaded == null)
        {
            lockedAndLoaded = manager.GetComponent<LockedAndLoadedSkill>();
        }

        if (manager != null && overcharged == null)
        {
            overcharged = manager.GetComponent<PoliceChiefOvercharged>();
        }

        if (manager != null)
        {
            if (lockedAndLoaded.reservedCount > 0)
            {
                playerStateMachine.OnCooldown("c_reserves");
            }
            else
            {
                playerStateMachine.OffCooldown("c_reserves");
            }
            if (manager.getSpecialCooldown() > 0)
            {
                playerStateMachine.OnCooldown("c_special");
            }
            else
            {
                playerStateMachine.OffCooldown("c_special");
            }
        }

        // SFX
        if (isCharging)
        {
            AudioManager.Instance.SFXBranch.GetSFXTrack("North-Railgun Charging").SetPitch(manager.GetStats().ComputeValue("Special Charge Up Time") - chargingTime, manager.GetStats().ComputeValue("Special Charge Up Time"));
        }
    }



    // Charge Up
    void StartSpecialChargeUp()
    {
        chargingTime = manager.GetStats().ComputeValue("Special Charge Up Time");
        isCharging = true;
        camAnim.SetBool("pullBack", true);
        GetComponent<Move>().PlayerStop();
        manager.specialDamage.extraContext = "Reserve Shot";

        // SFX
        AudioManager.Instance.SFXBranch.PlaySFXTrack("North-Railgun Charging");
        AudioManager.Instance.VABranch.PlayVATrack("North-Police_Chief Railgun Charging Up");
    }

    void StopSpecialChargeUp()
    {
        if (chargingTime > 0f) endSpecial(false, false);
        isCharging = false;
        chargingTime = 0f;

        // SFX
        AudioManager.Instance.SFXBranch.StopSFXTrack("North-Railgun Charging");
    }



    // Primed
    void StartSpecialPrimed()
    {
        isPrimed = true;
        manager.specialDamage.extraContext = "";

        // SFX
        AudioManager.Instance.SFXBranch.PlaySFXTrack("North-Railgun Primed");
        AudioManager.Instance.SFXBranch.GetSFXTrack("North-Railgun Primed Loop").SetPitch(0f, 1f);
        AudioManager.Instance.SFXBranch.PlaySFXTrack("North-Railgun Primed Loop");
        if (overcharged.pointIndex > 0)
        {
            AudioManager.Instance.VABranch.PlayVATrack("North-Police_Chief Overcharging");
        }
        else
        {
            AudioManager.Instance.VABranch.PlayVATrack("North-Police_Chief Railgun Full Charge");
        }

        if (overcharged.pointIndex > 0) overcharged.StartOvercharging();
    }
    
    void StopSpecialPrimed()
    {
        isPrimed = false;
        endSpecial(false, false);

        if (overcharged.pointIndex > 0) overcharged.StopOvercharging();
    }



    // Railgun Attack
    public void StartSpecialAttack()
    {
        playerStateMachine.ConsumeLightAttackInput();
        camAnim.SetBool("pullBack", true);
        GetComponent<Move>().PlayerStop();

        // Calculate initial shot aiming vector
        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 pos = transform.position;
        Vector2 dir = (mousePos - pos).normalized;

        // Fire shot
        GameObject railgunShot = Instantiate(manager.specialShot, Vector3.zero, Quaternion.identity);
        railgunShot.GetComponent<PoliceChiefRailgunShot>().fireRailgunShot(manager, pos, dir);
        GameplayEventHolder.OnAbilityUsed?.Invoke(manager.policeChiefRailgun);

        // SFX
        AudioManager.Instance.SFXBranch.PlaySFXTrack("North-Railgun Attack");
        AudioManager.Instance.SFXBranch.StopSFXTrack("North-Railgun Charging");
        AudioManager.Instance.SFXBranch.StopSFXTrack("North-Railgun Primed Loop");
    }

    void StopSpecialAttack()
    {
        KillSpecial();
    }

    public void KillSpecial()
    {
        bool startCooldown = !(manager.getSpecialCooldown() > 0); // if cooldown already exists, don't restart it
        if (manager.getSpecialCooldown() > 0 || manager.specialDamage.extraContext.Equals("Reserve Shot")) lockedAndLoaded.ConsumeReserveCharge();
        bool loop = (lockedAndLoaded.reservedCount > 0 && PlayerID.instance.GetComponent<Animator>().GetBool("i_special")); // if has reserve, and still holding down right click

        endSpecial(startCooldown, loop);
    }

    /// <summary>
    /// End the special ability if it is active.
    /// </summary>
    /// <param name="startCooldown">If true, the special ability's cooldown will begin when the ability ends.</param>
    public void endSpecial(bool startCooldown, bool loop)
    {
        // if preparing another shot using reserve charges, do not reset camera
        if (!loop)
        {
            camAnim.SetBool("pullBack", false);
            GetComponent<Move>().PlayerGo();
        }
        if (startCooldown)
        {
            playerStateMachine.OnCooldown("c_special");
            manager.startSpecialCooldown();
        }

        // SFX
        AudioManager.Instance.SFXBranch.StopSFXTrack("North-Railgun Charging");
        AudioManager.Instance.SFXBranch.StopSFXTrack("North-Railgun Primed Loop");
    }
}
