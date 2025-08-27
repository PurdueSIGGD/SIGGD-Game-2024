using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceChiefUIDriver : GhostUIDriver
{
    private PoliceChiefManager manager;
    private LockedAndLoadedSkill lockedAndLoaded;

    [SerializeField] private Sprite lethalForceIcon;
    [SerializeField] private Sprite overclockedIcon;

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
        
        // Locked and Loaded
        if (lockedAndLoaded.reservedCount > 0)
        {
            specialAbilityUIManager.setNumberActive(false);
            specialAbilityUIManager.setAbilityEnabled(true);
            specialAbilityUIManager.setChargeWidgetActive(true);
            specialAbilityUIManager.setChargeValue(lockedAndLoaded.reservedCount, ((manager.special != null && manager.special.isPrimed) && manager.getSpecialCooldown() <= 0) ? 100f : 0f);
        }
        else
        {
            specialAbilityUIManager.setChargeWidgetActive(false);
        }
    }

    private void updateSkill1()
    {
        //skill1UIManager.setUIActive(false);
        PoliceChiefLethalForce lethalForce = GetComponent<PoliceChiefLethalForce>();
        if (lethalForce.GetTotalHits() != -1)
        {
            skill1UIManager.setUIActive(true);
            skill1UIManager.setAbilityEnabled(lethalForce.shotEmpowered);
            skill1UIManager.setNumberActive(!lethalForce.shotEmpowered);
            skill1UIManager.setNumberValue(lethalForce.GetTotalHits() - lethalForce.GetConsecutiveHits());
            skill1UIManager.setChargeWidgetActive(false);
            //skill1UIManager.setChargeValue(lethalForce.GetTotalHits() - lethalForce.GetConsecutiveHits(), lethalForce.GetTotalHits() - lethalForce.GetConsecutiveHits());
            skill1UIManager.setMeterValue(lethalForce.GetConsecutiveHits(), lethalForce.GetTotalHits());
            skill1UIManager.setIcon(lethalForceIcon);
            skill1UIManager.setAbilityHighlighted(lethalForce.shotEmpowered);
        }
        else
        {
            OverclockedUIDriver(skill1UIManager);
        }
    }

    private void updateSkill2()
    {
        PoliceChiefLethalForce lethalForce = GetComponent<PoliceChiefLethalForce>();
        if (lethalForce.GetTotalHits() != -1) OverclockedUIDriver(skill2UIManager);
    }

    private void OverclockedUIDriver(PlayerAbilityUIManager UIManager)
    {
        UIManager.setUIActive(false);
        PoliceChiefOvercharged overcharged = GetComponent<PoliceChiefOvercharged>();
        if (manager.special == null) return;
        if (overcharged.pointIndex > 0 && (manager.special.isCharging || overcharged.isOvercharging))
        {
            UIManager.setUIActive(true);
            UIManager.setAbilityEnabled((manager.special.isCharging) ? false : true);
            UIManager.setNumberActive(false);
            UIManager.setChargeWidgetActive(false);
            UIManager.setMeterValue((manager.special.isCharging) ? 0f : (overcharged.overchargeDuration - overcharged.timer), overcharged.overchargeDuration);
            UIManager.setIcon(overclockedIcon);
        }
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
