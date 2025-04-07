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
        meterUIManager = PlayerInWorldMeterUIManager.instance;
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

        basicAbilityUIManager.updateIcon(orionCharacterInfo.specialAbilityIcon);
        basicAbilityUIManager.updateFrameColor(orionCharacterInfo.primaryColor);
        basicAbilityUIManager.updateAbilityCooldownTime(manager.getSpecialCooldown(), stats.ComputeValue("Dash Cooldown"));
        specialAbilityUIManager.gameObject.SetActive(false);
    }
}
