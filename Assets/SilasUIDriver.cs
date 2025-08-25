using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilasUIDriver : GhostUIDriver
{
    private SilasManager manager;

    [SerializeField] private Sprite selfMedicatedIcon;
    
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        manager = GetComponent<SilasManager>();
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
        basicAbilityUIManager.setNumberActive(false);

        // Apothecary casting
        if (manager.basic != null && manager.basic.isHealing)
        {
            basicAbilityUIManager.setAbilityEnabled(true);
            basicAbilityUIManager.setMeterValue((stats.ComputeValue("Basic Cast Time") - manager.basic.timer), stats.ComputeValue("Basic Cast Time"));
            basicAbilityUIManager.setChargeWidgetActive(false);
            return;
        }

        // Apothecary ready
        if (manager.healReady)
        {
            basicAbilityUIManager.setAbilityEnabled(true);
            basicAbilityUIManager.setMeterValue(1f, 1f);
            basicAbilityUIManager.setChargeWidgetActive(false);
            return;
        }

        // Apothecary charging
        basicAbilityUIManager.setAbilityEnabled(false);
        basicAbilityUIManager.setMeterValue(manager.ingredientsCollected, stats.ComputeValue("Basic Ingredient Cost"));
        basicAbilityUIManager.setChargeWidgetActive(true);
        basicAbilityUIManager.setChargeValue(stats.ComputeValue("Basic Ingredient Cost") - manager.ingredientsCollected, stats.ComputeValue("Basic Ingredient Cost") - manager.ingredientsCollected);
    }

    private void updateSpecialAbility()
    {
        specialAbilityUIManager.setAbilityCooldownTime(manager.getSpecialCooldown(), stats.ComputeValue("Special Cooldown"));
        specialAbilityUIManager.setAbilityEnabled(manager.specialCharges > 0);
        specialAbilityUIManager.setNumberActive(manager.specialCharges <= 0);
        specialAbilityUIManager.setChargeWidgetActive(true);
        specialAbilityUIManager.setChargeValue(manager.specialCharges, stats.ComputeValue("Special Max Charges"));
    }

    private void updateSkill1()
    {
        SelfMedicated selfMedicated = GetComponent<SelfMedicated>();
        if (!selfMedicated.isBuffed)
        {
            skill1UIManager.setUIActive(false);
            return;
        }
        skill1UIManager.setUIActive(true);
        skill1UIManager.setIcon(selfMedicatedIcon);
        skill1UIManager.setAbilityEnabled(true);
        skill1UIManager.setNumberActive(false);
        skill1UIManager.setMeterValue(selfMedicated.timer, selfMedicated.buffDuration);
        skill1UIManager.setChargeWidgetActive(false);
    }

    private void updateSkill2()
    {

    }

    private void updateMeter()
    {
        meterUIManager.setMeterValue(manager.ingredientsCollected, stats.ComputeValue("Basic Ingredient Cost"));
        meterUIManager.setMeterColor(ghostIdentity.GetCharacterInfo().primaryColor);
        if (manager.ingredientsCollected >= stats.ComputeValue("Basic Ingredient Cost"))
        {
            meterUIManager.setMeterColor(ghostIdentity.GetCharacterInfo().highlightColor);
        }
        meterUIManager.setSubMeterValue(0f, 1f);
        meterUIManager.resetSubMeterColor();
        meterUIManager.activateWidget();

        if (manager.ingredientsCollected > 0)
        {
            meterUIManager.activateWidget();
            return;
        }
        meterUIManager.deactivateWidget(0.3f);
    }
}