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
        updateBasicAbility();
        updateSpecialAbility();
        if (ghostIdentity.IsSelected()) updateMeter();

    }

    private void updateBasicAbility()
    {
        basicAbilityUIManager.setMeterValue(manager.wrathPercent, 1f);
        basicAbilityUIManager.setAbilityEnabled(manager.wrathPercent > 0f);
    }

    private void updateSpecialAbility()
    {
        specialAbilityUIManager.setAbilityCooldownTime(manager.getSpecialCooldown(), stats.ComputeValue("Special Cooldown"));
    }

    private void updateMeter()
    {
        meterUIManager.setMeterValue(manager.wrathPercent, 1f);
        meterUIManager.setMeterColor(ghostIdentity.GetCharacterInfo().primaryColor);
        meterUIManager.setSubMeterValue(0f, 1f);
        if (manager.wrathPercent >= 1f)
        {
            meterUIManager.setMeterColor(ghostIdentity.GetCharacterInfo().highlightColor);
        }
        WrathHeavyAttack wrath = PlayerID.instance.GetComponent<WrathHeavyAttack>();
        if (wrath != null && (wrath.isCharging || wrath.isPrimed))
        {
            meterUIManager.setMeterColor(ghostIdentity.GetCharacterInfo().highlightColor);
        }
        if (manager.wrathPercent > 0f)
        {
            meterUIManager.activateWidget();
            return;
        }
        meterUIManager.deactivateWidget(0.3f);
    }
}