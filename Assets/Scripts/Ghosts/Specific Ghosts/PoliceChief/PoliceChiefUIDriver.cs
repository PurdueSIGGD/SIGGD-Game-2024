using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceChiefUIDriver : GhostUIDriver
{
    private PoliceChiefManager manager;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        manager = GetComponent<PoliceChiefManager>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (!isInParty) return;
        updateBasicAbility();
        updateSpecialAbility();
        updateSkill1();
        updateSkill2();
        if (ghostIdentity.IsSelected()) updateMeter();
    }

    private void updateBasicAbility()
    {
        basicAbilityUIManager.setAbilityHighlighted(GetComponent<PoliceChiefPowerSpike>().ableToCrit);
    }

    private void updateSpecialAbility()
    {
        specialAbilityUIManager.setAbilityCooldownTime(manager.getSpecialCooldown(), stats.ComputeValue("Special Cooldown"));
    }

    private void updateSkill1()
    {

    }

    private void updateSkill2()
    {

    }

    private void updateMeter()
    {
        if(GetComponent<PoliceChiefLethalForce>() != null && GetComponent<PoliceChiefLethalForce>().numHits != -1)
        {
            meterUIManager.setMeterColor(Color.red);
            meterUIManager.setMeterValue(GetComponent<PoliceChiefLethalForce>().consecutiveHits, GetComponent<PoliceChiefLethalForce>().numHits);
            meterUIManager.setBackgroundColor(Color.grey);
            meterUIManager.setSubMeterValue(0f, 0f);
            meterUIManager.activateWidget();
        }
    }
}
