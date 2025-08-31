using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingUIDriver : GhostUIDriver
{
    private KingManager manager;
    private DivineSmite smite;

    [SerializeField] private Sprite divineSmiteIcon;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        manager = GetComponent<KingManager>();
        smite = GetComponent<DivineSmite>();
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
        specialAbilityUIManager.setAbilityHighlighted(GetComponent<DivineSmite>().isSpecialPowered());
    }

    private void updateSkill1()
    {
        if (smite.pointIndex <= 0)
        {
            skill1UIManager.setUIActive(false);
            return;
        }
        skill1UIManager.setUIActive(true);
        skill1UIManager.setAbilityEnabled(smite.isSpecialPowered());
        skill1UIManager.setMeterValue(smite.damageProgress, smite.dmgNeeded[smite.pointIndex]);
        skill1UIManager.setNumberActive(!smite.isSpecialPowered());
        //skill1UIManager.setNumberValue(smite.damageProgress);
        skill1UIManager.setNumberValue(Mathf.FloorToInt((smite.damageProgress / smite.dmgNeeded[smite.pointIndex]) * 100f));
        skill1UIManager.setChargeWidgetActive(false);
        skill1UIManager.setAbilityHighlighted(smite.isSpecialPowered());
    }

    private void updateSkill2()
    {

    }

    private void updateMeter()
    {
        //Sub meter
        /*
        float maxCooldown = (manager.GetStats().ComputeValue("Shield Max Health") - stats.ComputeValue("Shield Health Cooldown Threshold")) / manager.GetStats().ComputeValue("Shield Health Regeneration Rate");
        meterUIManager.setSubMeterValue((maxCooldown) - manager.getBasicCooldown(), (maxCooldown));
        meterUIManager.setSubMeterColor(ghostIdentity.GetCharacterInfo().primaryColor);
        if (manager.getBasicCooldown() > 0f) meterUIManager.resetSubMeterColor();
        */
        meterUIManager.setSubMeterValue(0f, 1f);

        //Meter
        meterUIManager.setMeterValue(manager.currentShieldHealth, stats.ComputeValue("Shield Max Health"));
        //if (manager.getBasicCooldown() <= 0f) meterUIManager.setMeterValue(manager.currentShieldHealth, stats.ComputeValue("Shield Max Health"));
        meterUIManager.setMeterColor((manager.getBasicCooldown() <= 0f && manager.hasShield) ? ghostIdentity.GetCharacterInfo().primaryColor : ghostIdentity.GetCharacterInfo().whiteColor);

        //Widget active
        if (manager.basic == null) return;
        if (manager.basic.isShielding || manager.currentShieldHealth < stats.ComputeValue("Shield Max Health") || manager.getBasicCooldown() > 0f)
        {
            meterUIManager.activateWidget();
            return;
        }
        meterUIManager.deactivateWidget(0.3f);
    }

}
