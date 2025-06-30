using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class DoubleTapSkill : Skill
{
    //private float chargeTimeChange;
    private Camera mainCamera;
    private PoliceChiefManager manager;
    //private bool reduceReady = false;
    private static int pointIndex;
    [SerializeField] private int[] values = {0, 8, 16, 24, 32};
    [SerializeField] public DamageContext secondaryShotDamage;

    public override void AddPointTrigger()
    {
        pointIndex = GetPoints();
    }

    public override void ClearPointsTrigger()
    {

    }

    public override void RemovePointTrigger()
    {
        pointIndex = GetPoints();
    }

    
    private void OnEnable()
    {
        //GameplayEventHolder.OnDamageFilter.Add(OnDamage);
        GameplayEventHolder.OnAbilityUsed += FireSecondaryShot;
    }

    private void OnDisable()
    {
        //GameplayEventHolder.OnDamageFilter.Remove(OnDamage);
        GameplayEventHolder.OnAbilityUsed -= FireSecondaryShot;
    }
    

    // Start is called before the first frame update
    void Start()
    {
        if (pointIndex <= 0) pointIndex = 0;
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        manager = GetComponent<PoliceChiefManager>();
        //baseDamage = manager.GetStats().ComputeValue("Basic Super Heavy Damage");
        //PoliceChiefSidearmShot.enemyWasShot += changeChargeTime;
        //chargeTimeChange = 0f;
    }

    /*
    private void changeChargeTime()
    {
        chargeTimeChange = chargeTimeChanging[pointindex];
        reduceReady = true;
    }
    */
    private void Update()
    {
        /*
        if (policeChiefBasic == null)
        {
            policeChiefBasic = PlayerID.instance.GetComponent<PoliceChiefBasic>();
            return;
        }
        if (policeChiefBasic.chargingTime > 0 && reduceReady==true)
        {
            policeChiefBasic.chargingTime = (policeChiefBasic.chargingTime - (policeChiefBasic.chargingTime * chargeTimeChange));
            reduceReady = false;
        }
        */
    }

    /*
    void OnDamage(ref DamageContext context)
    {
        if (context.actionID == ActionID.POLICE_CHIEF_BASIC && context.damage >= baseDamage)
        {
            context.damage *= ((baseDamage + secondaryShotDamage[pointindex]) / baseDamage);
        }
    }
    */

    public void FireSecondaryShot(ActionContext actionContext)
    {
        if (actionContext.actionID != ActionID.POLICE_CHIEF_BASIC || !actionContext.extraContext.Equals("Full Charge") || pointIndex <= 0) return;
        StartCoroutine(SecondaryShotCoroutine(actionContext));
    }

    private IEnumerator SecondaryShotCoroutine(ActionContext actionContext)
    {
        // Secondary shot delay
        yield return new WaitForSeconds(0.1f);

        // Calculate shot aiming vector
        Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 pos = PlayerID.instance.transform.position;
        Vector2 dir = (mousePos - pos).normalized;

        // Fire shot
        secondaryShotDamage.damage = values[pointIndex];
        GameObject sidearmShot = Instantiate(manager.basicShot, Vector3.zero, Quaternion.identity);
        sidearmShot.GetComponent<PoliceChiefSidearmShot>().fireSidearmShot(manager, pos, dir, true);

        // SFX
        AudioManager.Instance.SFXBranch.GetSFXTrack("North-Sidearm Attack").SetPitch(0f, 1f);
        AudioManager.Instance.SFXBranch.PlaySFXTrack("North-Sidearm Attack");
    }
}
