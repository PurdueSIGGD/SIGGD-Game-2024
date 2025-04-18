using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdolUIDriver : GhostUIDriver
{
    private IdolManager manager;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        manager = GetComponent<IdolManager>();
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
        basicAbilityUIManager.setMeterValue(manager.passive.tempoStacks, stats.ComputeValue("TEMPO_MAX_STACKS"));
        basicAbilityUIManager.setAbilityEnabled(manager.passive.tempoStacks > 0);
        basicAbilityUIManager.setChargeWidgetActive(true);
        basicAbilityUIManager.setChargeValue(manager.passive.tempoStacks, stats.ComputeValue("TEMPO_MAX_STACKS"));
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
        meterUIManager.setMeterValue(manager.passive.tempoStacks, stats.ComputeValue("TEMPO_MAX_STACKS"));
        meterUIManager.setMeterColor(ghostIdentity.GetCharacterInfo().primaryColor);
        meterUIManager.setSubMeterValue(1f, 1f);
        meterUIManager.setSubMeterColor(ghostIdentity.GetCharacterInfo().primaryColor);
        if (manager.passive.tempoStacks > 0)
        {
            meterUIManager.activateWidget();
            return;
        }
        meterUIManager.deactivateWidget(0.3f);
    }
}
