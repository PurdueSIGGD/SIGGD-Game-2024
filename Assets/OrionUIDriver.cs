using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrionUIDriver : MonoBehaviour
{
    [SerializeField] private CharacterSO orionCharacterInfo;
    private StatManager stats;
    private PartyManager partyManager;
    private OrionManager manager;

    private PlayerSelectedGhostUIManager selectedGhostUIManager;
    private PlayerGhostUIManager deselectedGhostUIManager;
    private PlayerAbilityUIManager basicAbilityUIManager;
    private PlayerAbilityUIManager specialAbilityUIManager;
    private PlayerAbilityUIManager skill1UIManager;
    private PlayerAbilityUIManager skill2UIManager;
    private PlayerInWorldMeterUIManager meterUIManager;

    private bool isSelected;

    // Start is called before the first frame update
    void Start()
    {
        stats = GetComponent<StatManager>();
        partyManager = PartyManager.instance;
        manager = GetComponent<OrionManager>();

        selectedGhostUIManager = PlayerSelectedGhostUIManager.instance;
        deselectedGhostUIManager = PlayerGhost1UIManager.instance;
        basicAbilityUIManager = selectedGhostUIManager.basicAbilityUIManager;
        specialAbilityUIManager = selectedGhostUIManager.specialAbilityUIManager;
        skill1UIManager = selectedGhostUIManager.skill1UIManager;
        skill2UIManager = selectedGhostUIManager.skill2UIManager;
        meterUIManager = PlayerInWorldMeterUIManager.instance;

        PlayerGhost1UIManager.instance.gameObject.SetActive(false);
        PlayerGhost2UIManager.instance.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        updateSelected();
        updateGhostUI();
        updateAbilityUI();
    }

    private void updateSelected()
    {
        isSelected = (partyManager.GetSelectedGhost() == null);
        if (partyManager.GetGhostMajorList().Count > 0 && partyManager.GetGhostMajorList()[0].IsSelected()) deselectedGhostUIManager = PlayerGhost1UIManager.instance;
        if (partyManager.GetGhostMajorList().Count > 1 && partyManager.GetGhostMajorList()[1].IsSelected()) deselectedGhostUIManager = PlayerGhost2UIManager.instance;
    }

    private void updateGhostUI()
    {
        Color ghostColor = orionCharacterInfo.primaryColor;
        if (isSelected)
        {
            selectedGhostUIManager.updateBackgroundColor(ghostColor);
            selectedGhostUIManager.updateHealthBarFrameColor(ghostColor);
            selectedGhostUIManager.updateIcon(orionCharacterInfo.characterIcon);
            selectedGhostUIManager.updateIconFrameColor(ghostColor);
            meterUIManager.updateBackgroundColor(ghostColor);
        }
        else
        {
            deselectedGhostUIManager.updateBackgroundColor(ghostColor);
            deselectedGhostUIManager.updateIcon(orionCharacterInfo.characterIcon);
            deselectedGhostUIManager.updateIconFrameColor(ghostColor);
        }
    }

    private void updateAbilityUI()
    {
        basicAbilityUIManager = (isSelected) ? selectedGhostUIManager.basicAbilityUIManager : deselectedGhostUIManager.basicAbilityUIManager;
        specialAbilityUIManager = (isSelected) ? selectedGhostUIManager.specialAbilityUIManager : deselectedGhostUIManager.specialAbilityUIManager;
        skill1UIManager = (isSelected) ? selectedGhostUIManager.skill1UIManager : deselectedGhostUIManager.skill1UIManager;
        skill2UIManager = (isSelected) ? selectedGhostUIManager.skill2UIManager : deselectedGhostUIManager.skill2UIManager;

        basicAbilityUIManager.gameObject.SetActive(true);
        basicAbilityUIManager.updateIcon(orionCharacterInfo.specialAbilityIcon);
        basicAbilityUIManager.updateFrameColor(orionCharacterInfo.primaryColor);
        basicAbilityUIManager.updateAbilityCooldownTime(manager.getSpecialCooldown(), stats.ComputeValue("Dash Cooldown"));
        basicAbilityUIManager.setAbilityHighlighted(manager.getSpecialCooldown() <= 0f);
        basicAbilityUIManager.setChargesWidgetActive(false);

        specialAbilityUIManager.gameObject.SetActive(false);

        skill1UIManager.gameObject.SetActive(true);
        skill1UIManager.setAbilityEnabled(true);
        skill1UIManager.updateIcon(orionCharacterInfo.basicAbilityIcon);
        skill1UIManager.updateFrameColor(orionCharacterInfo.primaryColor);
        skill1UIManager.updateMeterValue(1f, 1f);
        skill1UIManager.setNumberActive(false);
        skill1UIManager.setChargesWidgetActive(false);

        skill2UIManager.gameObject.SetActive(true);
        skill2UIManager.setAbilityEnabled(true);
        skill2UIManager.updateIcon(orionCharacterInfo.basicAbilityIcon);
        skill2UIManager.updateFrameColor(orionCharacterInfo.primaryColor);
        skill2UIManager.updateMeterValue(stats.ComputeValue("Dash Cooldown") - manager.getSpecialCooldown(), stats.ComputeValue("Dash Cooldown"));
        skill2UIManager.setNumberActive(false);
        skill2UIManager.setChargesWidgetActive(false);
        skill2UIManager.setAbilityHighlighted(manager.getSpecialCooldown() <= 0f);
    }
}
