using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YumeUIDriver : GhostUIDriver
{
    private SeamstressManager manager;

    [SerializeField] private Sprite scrapSaverIcon;

    protected override void Start()
    {
        base.Start();
        manager = GetComponent<SeamstressManager>();
    }

    protected override void Update()
    {
        /*
        base.Update();
        if (!isInParty)
        {
            basicAbilityUIManager.setChargeWidgetActive(false);
            return;
        }
        basicAbilityUIManager.setChargeWidgetActive(true);
        UpdateBasicAbility();
        UpdateSpecialAbility();
        */

        base.Update();
        if (!isInParty) return;
        UpdateBasicAbility();
        UpdateSpecialAbility();
        UpdateSkill1();
        //UpdateSkill2();
        if (ghostIdentity.IsSelected()) updateMeter();
    }

    private void UpdateBasicAbility()
    {
        int spoolCount = manager.GetSpools();
        basicAbilityUIManager.setChargeWidgetActive(true);
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
        int spoolCount = manager.GetSpools();
        if (spoolCount < 0)
        {
            specialAbilityUIManager.setAbilityEnabled(false);
            specialAbilityUIManager.setMeterValue(0f, 1f);
        }
        else
        {
            specialAbilityUIManager.setAbilityCooldownTime(manager.getSpecialCooldown(), stats.ComputeValue("Special Cooldown"));
        }
    }

    private void UpdateSkill1()
    {
        /*
        if (manager.clones.Count > 0 && manager.clones[0] != null)
        {
            skill1UIManager.setUIActive(true);
            skill1UIManager.setNumberActive(false);
            //skill1UIManager.setIcon(manager.GetComponent<GhostIdentity>().GetCharacterInfo().specialAbilityIcon);
            skill1UIManager.setIcon(cloneIcon);
            skill1UIManager.setMeterValue(manager.clones[0].GetComponent<Health>().currentHealth, manager.clones[0].GetComponent<StatManager>().ComputeValue("Max Health"));
            skill1UIManager.setChargeWidgetActive(true);
            skill1UIManager.setChargeValue(manager.clones[0].GetComponent<Health>().currentHealth, manager.clones[0].GetComponent<StatManager>().ComputeValue("Max Health"));
        }
        else
        {
            skill1UIManager.setUIActive(false);
        }
        */

        ScrapSaver scrapSaver = GetComponent<ScrapSaver>();
        if (scrapSaver.pointIndex <= 0)
        {
            skill1UIManager.setUIActive(false);
            return;
        }

        skill1UIManager.setUIActive(true);
        skill1UIManager.setAbilityEnabled(scrapSaver.isSaving);
        skill1UIManager.setNumberActive(!scrapSaver.isSaving);
        skill1UIManager.setNumberValue(scrapSaver.CalculateNumEnemiesNeeded() - scrapSaver.numEnemiesDefeated);
        skill1UIManager.setIcon(scrapSaverIcon);
        skill1UIManager.setMeterValue(((scrapSaver.isSaving) ? scrapSaver.CalculateNumEnemiesNeeded() : scrapSaver.numEnemiesDefeated), scrapSaver.CalculateNumEnemiesNeeded());
        skill1UIManager.setChargeWidgetActive(false);
    }

    private void updateMeter()
    {
        // Scrap Saver Sub Meter
        meterUIManager.resetSubMeterColor();
        ScrapSaver scrapSaver = GetComponent<ScrapSaver>();
        if (scrapSaver.pointIndex <= 0)
        {
            meterUIManager.setSubMeterValue(0f, 1f);
        }
        else
        {
            meterUIManager.setSubMeterValue(((scrapSaver.isSaving) ? scrapSaver.CalculateNumEnemiesNeeded() : scrapSaver.numEnemiesDefeated), scrapSaver.CalculateNumEnemiesNeeded());
        }

        // Spools Meter
        int spoolCount = manager.GetSpools();
        meterUIManager.setMeterValue(spoolCount, stats.ComputeValue("Max Spools"));
        meterUIManager.setMeterColor(ghostIdentity.GetCharacterInfo().highlightColor);
        if (spoolCount >= stats.ComputeValue("Max Spools"))
        {
            meterUIManager.setMeterColor(ghostIdentity.GetCharacterInfo().highlightColor);
        }

        if (spoolCount > 0 || manager.GetWeaveTimer() > 0f || scrapSaver.numEnemiesDefeated > 0f || scrapSaver.isSaving)
        {
            meterUIManager.activateWidget();
            return;
        }
        meterUIManager.deactivateWidget(0.3f);
    }
}
