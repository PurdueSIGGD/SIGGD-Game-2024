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
    private bool isCharging = false;
    private float chargingTime = 0f;
    [HideInInspector] public int reserves = 0;



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

        if (manager != null)
        {
            if (reserves > 0)
            {
                playerStateMachine.OnCooldown("c_reserves");
            }
            else
            {
                playerStateMachine.OffCooldown("c_reserves");
            }
            if (manager.getSpecialCooldown() > 0)
            {
                if (reserves > 0)
                {
                    playerStateMachine.OnCooldown("c_special");
                }
            }
            else
            {
                playerStateMachine.OffCooldown("c_special");
            }
        }
    }



    // Charge Up
    void StartSpecialChargeUp()
    {
        chargingTime = manager.GetStats().ComputeValue("Special Charge Up Time");
        isCharging = true;
        camAnim.SetBool("pullBack", true);
        GetComponent<Move>().PlayerStop();
    }

    void StopSpecialChargeUp()
    {
        if (chargingTime > 0f) endSpecial(false, false);
        isCharging = false;
        chargingTime = 0f;
    }



    // Primed
    void StartSpecialPrimed()
    {

    }
    
    void StopSpecialPrimed()
    {
        endSpecial(false, false);
    }



    // Railgun Attack
    public void StartSpecialAttack()
    {
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
    }

    void StopSpecialAttack(Animator animator)
    {
        bool startCooldown = !(manager.getSpecialCooldown() > 0); // if cooldown already exists, don't restart it
        bool loop = (reserves > 0 && animator.GetBool("i_special")); // if has reserve, and still holding down right click

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
    }
}
