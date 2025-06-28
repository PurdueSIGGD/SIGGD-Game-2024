using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingUIDriver : GhostUIDriver
{
    private KingManager manager;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        manager = GetComponent<KingManager>();
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
        basicAbilityUIManager.setAbilityEnabled(manager.getBasicCooldown() <= 0f && manager.hasShield, true);
        basicAbilityUIManager.setNumberActive(manager.getBasicCooldown() > 0f);
        basicAbilityUIManager.setNumberValue(manager.getBasicCooldown());
        basicAbilityUIManager.setMeterValue(manager.currentShieldHealth, stats.ComputeValue("Shield Max Health"));
        basicAbilityUIManager.setChargeWidgetActive(true);
        basicAbilityUIManager.setChargeValue(manager.currentShieldHealth, stats.ComputeValue("Shield Max Health"));
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
        //Sub meter
        float maxCooldown = (manager.GetStats().ComputeValue("Shield Max Health") - stats.ComputeValue("Shield Health Cooldown Threshold")) / manager.GetStats().ComputeValue("Shield Health Regeneration Rate");
        meterUIManager.setSubMeterValue((maxCooldown) - manager.getBasicCooldown(), (maxCooldown));
        meterUIManager.setSubMeterColor(ghostIdentity.GetCharacterInfo().primaryColor);
        if (manager.getBasicCooldown() > 0f) meterUIManager.resetSubMeterColor();

        //Meter
        meterUIManager.setMeterValue(manager.endShieldHealth, stats.ComputeValue("Shield Max Health"));
        if (manager.getBasicCooldown() <= 0f) meterUIManager.setMeterValue(manager.currentShieldHealth, stats.ComputeValue("Shield Max Health"));
        meterUIManager.setMeterColor(ghostIdentity.GetCharacterInfo().primaryColor);

        //Widget active
        if (manager.basic == null) return;
        if (manager.basic.isShielding || manager.currentShieldHealth < stats.ComputeValue("Shield Max Health"))
        {
            meterUIManager.activateWidget();
            return;
        }
        meterUIManager.deactivateWidget(0.3f);
    }

}
