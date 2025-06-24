using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceChiefUIDriver : GhostUIDriver
{
    private PoliceChiefManager manager;
    private LockedAndLoadedSkill lockedAndLoaded;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        manager = GetComponent<PoliceChiefManager>();
        lockedAndLoaded = manager.GetComponent<LockedAndLoadedSkill>();
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
        basicAbilityUIManager.setAbilityHighlighted(GetComponent<PoliceChiefPowerSpike>().GetAbleToCrit());
        basicAbilityUIManager.setAbilityEnabled(manager.basicAmmo > 0);
        basicAbilityUIManager.setMeterValue(manager.basicAmmo, stats.ComputeValue("Basic Starting Ammo"));
        basicAbilityUIManager.setChargeWidgetActive(true);
        basicAbilityUIManager.setChargeValue(manager.basicAmmo, stats.ComputeValue("Basic Starting Ammo"));
    }

    private void updateSpecialAbility()
    {
        specialAbilityUIManager.setAbilityCooldownTime(manager.getSpecialCooldown(), stats.ComputeValue("Special Cooldown"));
        if (lockedAndLoaded.reservedCount > 0)
        {
            specialAbilityUIManager.setChargeWidgetActive(true);
            specialAbilityUIManager.setChargeValue(lockedAndLoaded.reservedCount, lockedAndLoaded.reserveCharges[LockedAndLoadedSkill.pointIndex]);
        }
        else
        {
            specialAbilityUIManager.setChargeWidgetActive(false);
        }
    }

    private void updateSkill1()
    {

    }

    private void updateSkill2()
    {

    }

    private void updateMeter() {
        if (GetComponent<PoliceChiefLethalForce>() != null && GetComponent<PoliceChiefLethalForce>().GetTotalHits() != -1)
        {
            //meterUIManager.setMeterColor(Color.red);
            //meterUIManager.setMeterValue(GetComponent<PoliceChiefLethalForce>().GetConsecutiveHits(), GetComponent<PoliceChiefLethalForce>().GetTotalHits());
            //meterUIManager.setBackgroundColor(Color.grey);
            meterUIManager.setSubMeterValue(GetComponent<PoliceChiefLethalForce>().GetConsecutiveHits(), GetComponent<PoliceChiefLethalForce>().GetTotalHits());
            //meterUIManager.activateWidget();
        }
        else
        {
            //meterUIManager.setSubMeterColor(ghostIdentity.GetCharacterInfo().primaryColor);
            meterUIManager.setSubMeterValue(0f, 0f);
        }

        //Meter
        meterUIManager.setMeterValue(manager.basicAmmo, stats.ComputeValue("Basic Starting Ammo"));
        meterUIManager.setMeterColor(ghostIdentity.GetCharacterInfo().primaryColor);

        //Widget active
        if (manager.basic == null) return;
        if (manager.basicAmmo < stats.ComputeValue("Basic Starting Ammo"))
        {
            meterUIManager.activateWidget();
            return;
        }
        meterUIManager.deactivateWidget(0.3f);
    }
}
