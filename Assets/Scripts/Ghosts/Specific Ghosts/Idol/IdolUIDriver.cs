using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdolUIDriver : GhostUIDriver
{
    [SerializeField] private Sprite cloneIcon;

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
        basicAbilityUIManager.setMeterValue(manager.passive.duration, stats.ComputeValue("TEMPO_BASE_DURATION"));
        basicAbilityUIManager.setAbilityEnabled(manager.passive.tempoStacks > 0);
        basicAbilityUIManager.setChargeWidgetActive(true);
        basicAbilityUIManager.setChargeValue(manager.passive.tempoStacks, stats.ComputeValue("TEMPO_MAX_STACKS"));
    }

    private void updateSpecialAbility()
    {
        specialAbilityUIManager.setAbilityCooldownTime(manager.getSpecialCooldown(), stats.ComputeValue("Special Cooldown"));
        if (manager.clones.Count > 0 && manager.clones[0] != null)
        {
            specialAbilityUIManager.setMeterValue(manager.clones[0].GetComponent<IdolClone>().duration, stats.ComputeValue("HOLOJUMP_DURATION_SECONDS"));
            specialAbilityUIManager.setAbilityHighlighted(true);
        }
        else
        {
            specialAbilityUIManager.setAbilityHighlighted(false);
        }
    }

    private void updateSkill1()
    {
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
    }

    private void updateSkill2()
    {
        if (manager.clones.Count > 1 && manager.clones[1] != null)
        {
            skill2UIManager.setUIActive(true);
            skill2UIManager.setNumberActive(false);
            //skill2UIManager.setIcon(manager.GetComponent<GhostIdentity>().GetCharacterInfo().specialAbilityIcon);
            skill2UIManager.setIcon(cloneIcon);
            skill2UIManager.setMeterValue(manager.clones[1].GetComponent<Health>().currentHealth, manager.clones[1].GetComponent<StatManager>().ComputeValue("Max Health"));
            skill2UIManager.setChargeWidgetActive(true);
            skill2UIManager.setChargeValue(manager.clones[1].GetComponent<Health>().currentHealth, manager.clones[1].GetComponent<StatManager>().ComputeValue("Max Health"));
        }
        else
        {
            skill2UIManager.setUIActive(false);
        }
    }

    private void updateMeter()
    {
        meterUIManager.setMeterValue(manager.passive.tempoStacks, stats.ComputeValue("TEMPO_MAX_STACKS"));
        meterUIManager.setMeterColor(ghostIdentity.GetCharacterInfo().primaryColor);
        meterUIManager.setSubMeterValue(manager.passive.duration, stats.ComputeValue("TEMPO_BASE_DURATION"));
        meterUIManager.resetSubMeterColor();
        if (manager.passive.tempoStacks > 0)
        {
            meterUIManager.activateWidget();
            return;
        }
        meterUIManager.deactivateWidget(0.3f);
    }
}
