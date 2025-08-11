using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamuraiUIDriver : GhostUIDriver
{
    [SerializeField] private Sprite relentlessFuryIcon;

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
        updateSkill1();
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

    private void updateSkill1()
    {
        skill1UIManager.setUIActive(false);
        RelentlessFury relentlessFury = GetComponent<RelentlessFury>();
        if (relentlessFury.buffStacks > 0)
        {
            skill1UIManager.setUIActive(true);
            skill1UIManager.setAbilityEnabled(true);
            skill1UIManager.setNumberActive(false);
            skill1UIManager.setChargeWidgetActive(true);
            skill1UIManager.setChargeValue(relentlessFury.buffStacks, relentlessFury.maxStacks);
            skill1UIManager.setMeterValue(relentlessFury.buffStacks, relentlessFury.maxStacks);
            skill1UIManager.setIcon(relentlessFuryIcon);
        }
    }

    private void updateMeter()
    {
        // Wrath meter
        meterUIManager.setMeterValue(manager.wrathPercent, 1f);
        meterUIManager.setMeterColor(ghostIdentity.GetCharacterInfo().primaryColor);
        if (manager.wrathPercent >= 1f)
        {
            meterUIManager.setMeterColor(ghostIdentity.GetCharacterInfo().highlightColor);
        }
        WrathHeavyAttack wrath = PlayerID.instance.GetComponent<WrathHeavyAttack>();
        if (wrath != null && (wrath.isCharging || wrath.isPrimed))
        {
            meterUIManager.setMeterColor(ghostIdentity.GetCharacterInfo().highlightColor);
        }

        // Honed Strike submeter
        meterUIManager.setSubMeterValue(0f, 1f);
        HonedStrike honedStrike = GetComponent<HonedStrike>();
        meterUIManager.setSubMeterValue(((honedStrike.buffApplied) ? 1f : 0f), 1f);

        if (manager.wrathPercent > 0f || honedStrike.buffApplied)
        {
            meterUIManager.activateWidget();
            return;
        }
        meterUIManager.deactivateWidget(0.3f);
    }
}