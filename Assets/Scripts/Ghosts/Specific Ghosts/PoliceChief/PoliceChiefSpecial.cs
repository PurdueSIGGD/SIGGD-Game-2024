using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PoliceChiefSpecial : MonoBehaviour, ISpecialMove
{
    private bool shouldChangeBack = true;
    private PlayerStateMachine playerStateMachine;
    private Animator camAnim;
    private Camera cam;

    [HideInInspector] public PoliceChiefManager manager;

    private bool isCharging = false;
    private float chargingTime = 0f;

    private bool isPrimed = false;
    private float primedTime = 0f;

    private bool isFiring = false;

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

        if (isPrimed && primedTime > 0f) primedTime -= Time.deltaTime;

        if (manager != null)
        {
            if (manager.getSpecialCooldown() > 0)
            {
                playerStateMachine.OnCooldown("c_special");
            }
            else
            {
                playerStateMachine.OffCooldown("c_special");
            }
        }
    }

    /*
    public void CheckPullBack()
    {
        if (shouldChangeBack) {
            endSpecial(false);
        }
        shouldChangeBack = true;
    }
    */

    void StartSpecialChargeUp()
    {
        chargingTime = manager.GetStats().ComputeValue("Special Charge Up Time");
        isCharging = true;
        camAnim.SetBool("pullBack", true);
        GetComponent<Move>().PlayerStop();
    }

    void StopSpecialChargeUp()
    {
        if (chargingTime > 0f) endSpecial(false); //camAnim.SetBool("pullBack", false);
        isCharging = false;
        chargingTime = 0f;
        //CheckPullBack();
        //updateCameraPullBack(0.1f);
    }

    void StartSpecialPrimed()
    {
        primedTime = manager.GetStats().ComputeValue("Special Overcharged Autofire Time");
        isPrimed = (primedTime > 0f);
        //shouldChangeBack = false;
    }
    
    void StopSpecialPrimed()
    {

        //endSpecial(false); //camAnim.SetBool("pullBack", false);
        //CheckPullBack();
        updateCameraPullBack(0.1f);
    }

    void StartSpecialAttack()
    {
        isFiring = true;
        //shouldChangeBack = false;
        //camAnim.SetBool("pullBack", true);
        //GetComponent<Move>().PlayerStop();

        // Calculate initial shot aiming vector
        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 pos = transform.position;
        Vector2 dir = (mousePos - pos).normalized;

        // Fire shot
        GameObject railgunShot = Instantiate(manager.specialShot, Vector3.zero, Quaternion.identity);
        railgunShot.GetComponent<PoliceChiefRailgunShot>().fireRailgunShot(manager, pos, dir);
    }

    void StopSpecialAttack()
    {
        isFiring = false;
        endSpecial(true);
    }

    /// <summary>
    /// End the special ability if it is active.
    /// </summary>
    /// <param name="startCooldown">If true, the special ability's cooldown will begin when the ability ends.</param>
    public void endSpecial(bool startCooldown)
    {
        camAnim.SetBool("pullBack", false);
        GetComponent<Move>().PlayerGo();
        if (!startCooldown) return;
        playerStateMachine.OnCooldown("c_special");
        manager.startSpecialCooldown();
    }

    private void updateCameraPullBack(float delay)
    {
        StartCoroutine(delayedPullBackCheck(delay));
    }

    private void updateCameraPullBack()
    {
        StartCoroutine(delayedPullBackCheck(0f));
    }

    private IEnumerator delayedPullBackCheck(float delay)
    {
        yield return new WaitForSeconds(delay);
        if ((isCharging || isPrimed || isFiring))
        {
            camAnim.SetBool("pullBack", true);
            yield break;
        }
        camAnim.SetBool("pullBack", false);
    }

    public bool GetBool()
    {
        return true;
    }
}
