using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class DoubleTapSkill : Skill
{
    private float chargeTimeChange;
    private PoliceChiefBasic policeChiefBasic;
    private bool reduceReady = false;
    private int pointindex;
    private float[] chargeTimeChanging = {0f,0.25f, 0.45f, 0.65f, 0.85f};
    public override void AddPointTrigger()
    {
        pointindex=GetPoints();
    }

    public override void ClearPointsTrigger()
    {
  
    }

    public override void RemovePointTrigger()
    {
        pointindex = GetPoints();
    }

    // Start is called before the first frame update
    void Start()
    {
        AddPoint();
        PoliceChiefSidearmShot.enemyWasShot += changeChargeTime;
        policeChiefBasic = PlayerID.instance.GetComponent<PoliceChiefBasic>();
        chargeTimeChange = 0f;

    }
    private void changeChargeTime()
    {
        chargeTimeChange = chargeTimeChanging[pointindex];
        reduceReady = true;
    }
    private void Update()
    {
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
    }
}
