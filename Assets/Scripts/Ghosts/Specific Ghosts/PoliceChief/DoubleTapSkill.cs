using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class DoubleTapSkill : Skill
{
    private float chargeTimeChange;
    private PoliceChiefManager manager;
    private bool reduceReady = false;
    private static int pointindex;
    [SerializeField] private int[] secondaryShotDamage = {8, 16, 24, 32};
    private float baseDamage;
    public override void AddPointTrigger()
    {
        pointindex = GetPoints() - 1;
    }

    public override void ClearPointsTrigger()
    {
  
    }

    public override void RemovePointTrigger()
    {
        pointindex = GetPoints() - 1;
    }

    /*
    private void OnEnable()
    {
        GameplayEventHolder.OnDamageFilter.Add(OnDamage);
    }

    private void OnDisable()
    {
        GameplayEventHolder.OnDamageFilter.Remove(OnDamage);
    }
    */

    // Start is called before the first frame update
    void Start()
    {
        //pointindex = GetPoints();
        //AddPoint();
        //manager = GetComponent<PoliceChiefManager>();
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
}
