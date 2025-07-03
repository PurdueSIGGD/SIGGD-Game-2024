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
        //basicAbilityUIManager.setAbilityHighlighted(GetComponent<PoliceChiefPowerSpike>().GetAbleToCrit());
        basicAbilityUIManager.setAbilityEnabled(manager.basicAmmo > 0);
        basicAbilityUIManager.setMeterValue(manager.basicAmmo, stats.ComputeValue("Basic Starting Ammo"));
        basicAbilityUIManager.setChargeWidgetActive(true);
        basicAbilityUIManager.setChargeValue(manager.basicAmmo, stats.ComputeValue("Basic Starting Ammo"));
        PoliceChiefLethalForce lethalForce = GetComponent<PoliceChiefLethalForce>();
        if (lethalForce != null && lethalForce.shotEmpowered)
        {
            basicAbilityUIManager.setAbilityHighlighted(true);
        }
        else if (lethalForce != null)
        {
            basicAbilityUIManager.setAbilityHighlighted(false);
        }
    }

    private void updateSpecialAbility()
    {
        specialAbilityUIManager.setAbilityCooldownTime(manager.getSpecialCooldown(), stats.ComputeValue("Special Cooldown"));
        if (lockedAndLoaded.reservedCount > 0)
        {
            specialAbilityUIManager.setChargeWidgetActive(true);
            specialAbilityUIManager.setChargeValue(lockedAndLoaded.reservedCount, lockedAndLoaded.reserveCharges[GetComponent<LockedAndLoadedSkill>().pointIndex]);
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
        // Meter
        meterUIManager.setMeterValue(manager.basicAmmo, stats.ComputeValue("Basic Starting Ammo"));
        meterUIManager.setMeterColor(ghostIdentity.GetCharacterInfo().highlightColor);

        // Lethal Force Submeter
        PoliceChiefLethalForce lethalForce = GetComponent<PoliceChiefLethalForce>();
        if (lethalForce != null && lethalForce.GetTotalHits() != -1)
        {
            meterUIManager.setSubMeterValue(lethalForce.GetConsecutiveHits(), lethalForce.GetTotalHits());
            if (lethalForce.GetConsecutiveHits() >= lethalForce.GetTotalHits())
            {
                meterUIManager.setMeterColor(ghostIdentity.GetCharacterInfo().primaryColor);
            }
        }
        else
        {
            meterUIManager.setSubMeterValue(0f, 0f);
        }

        // Widget active
        if (manager.basic == null) return;
        if (manager.basicAmmo < stats.ComputeValue("Basic Starting Ammo") ||
            (lethalForce != null && lethalForce.GetTotalHits() != -1 && lethalForce.GetConsecutiveHits() > 0))
        {
            meterUIManager.activateWidget();
            return;
        }
        meterUIManager.deactivateWidget(0.3f);
    }
}
