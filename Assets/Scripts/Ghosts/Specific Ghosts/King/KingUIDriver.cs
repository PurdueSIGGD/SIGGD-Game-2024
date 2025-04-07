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
        updateBasicAbility();
        updateSpecialAbility();
        if (!ghostIdentity.IsSelected()) return;
        updateMeterValue();
        updateMeterColor();
        updateMeterActive();
    }

    private void updateMeterValue()
    {
        float value = manager.currentShieldHealth;
        float maxValue = stats.ComputeValue("Shield Max Health");
        meterUIManager.updateMeterValue(value, maxValue);
    }

    private void updateMeterColor()
    {
        meterUIManager.updateMeterColor(ghostIdentity.GetCharacterInfo().primaryColor);
        if (manager.getBasicCooldown() > 0f) meterUIManager.resetMeterColor();
    }

    private void updateMeterActive()
    {
        if (manager.basic == null) return;
        if (manager.basic.isShielding || manager.currentShieldHealth < stats.ComputeValue("Shield Max Health"))
        {
            meterUIManager.activateWidget();
            return;
        }
        meterUIManager.deactivateWidget(0.3f);
    }

    private void updateBasicAbility()
    {
        basicAbilityUIManager.setAbilityEnabled(manager.getBasicCooldown() <= 0f);
        basicAbilityUIManager.setNumberActive(manager.getBasicCooldown() > 0f);
        basicAbilityUIManager.updateNumberValue(manager.getBasicCooldown());
        basicAbilityUIManager.updateMeterValue(manager.currentShieldHealth, stats.ComputeValue("Shield Max Health"));
    }

    private void updateSpecialAbility()
    {
        specialAbilityUIManager.updateAbilityCooldownTime(manager.getSpecialCooldown(), stats.ComputeValue("Special Cooldown"));
    }

}
