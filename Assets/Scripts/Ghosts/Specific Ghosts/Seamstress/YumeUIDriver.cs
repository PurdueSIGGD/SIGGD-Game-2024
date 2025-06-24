using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YumeUIDriver : GhostUIDriver
{
    private SeamstressManager manager;

    protected override void Start()
    {
        base.Start();
        manager = GetComponent<SeamstressManager>();
    }

    protected override void Update()
    {
        base.Update();
        if (!isInParty)
        {
            basicAbilityUIManager.setChargeWidgetActive(false);
            return;
        }
        basicAbilityUIManager.setChargeWidgetActive(true);
        UpdateBasicAbility();
        UpdateSpecialAbility();
    }

    private void UpdateBasicAbility()
    {
        int spoolCount = manager.GetSpools();
        basicAbilityUIManager.setChargeValue(spoolCount, manager.GetStats().ComputeValue("Max Spools"));
        if (spoolCount > 0)
        {
            basicAbilityUIManager.setAbilityEnabled(true);
            basicAbilityUIManager.setMeterValue(manager.GetWeaveTimer(), manager.GetStats().ComputeValue("Concurrent Spool Buffer"));
        }
        else
        {
            basicAbilityUIManager.setAbilityEnabled(false);
            basicAbilityUIManager.setMeterValue(manager.GetWeaveTimer(), manager.GetStats().ComputeValue("Initial Spool Buffer"));
        }
    }

    private void UpdateSpecialAbility()
    {
        specialAbilityUIManager.setAbilityCooldownTime(manager.getSpecialCooldown(), stats.ComputeValue("Special Cooldown"));
    }
}
