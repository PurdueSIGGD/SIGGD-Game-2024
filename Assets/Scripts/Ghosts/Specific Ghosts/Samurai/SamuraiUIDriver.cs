using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamuraiUIDriver : GhostUIDriver
{
    private SamuraiManager manager;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        manager = GetComponent<SamuraiManager>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (!isInParty) return;
        if (manager.selected) updateBasicAbility();
        updateSpecialAbility();

    }

    private void updateBasicAbility()
    {
        basicAbilityUIManager.setMeterValue(manager.basic.GetWrathPercent(), 1f);
        basicAbilityUIManager.setAbilityEnabled(manager.basic.GetWrathPercent() > 0);
    }

    private void updateSpecialAbility()
    {
        specialAbilityUIManager.setAbilityCooldownTime(manager.getSpecialCooldown(), stats.ComputeValue("Success Cooldown Time"));
    }

    private void updateMeter()
    {
        if (GetComponent<PoliceChiefLethalForce>() != null && GetComponent<PoliceChiefLethalForce>().GetTotalHits() != -1)
        {
            meterUIManager.setMeterColor(Color.red);
            meterUIManager.setMeterValue(GetComponent<PoliceChiefLethalForce>().GetConsecutiveHits(), GetComponent<PoliceChiefLethalForce>().GetTotalHits());
            meterUIManager.setBackgroundColor(Color.grey);
            meterUIManager.setSubMeterValue(0f, 0f);
            meterUIManager.activateWidget();
        }
    }
}