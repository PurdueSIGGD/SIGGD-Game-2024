using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GhostUIDriver : MonoBehaviour, ISelectable
{
    protected StatManager stats;
    protected PartyManager partyManager;
    protected GhostIdentity ghostIdentity;
    protected PlayerSelectedGhostUIManager selectedGhostUIManager;
    protected PlayerGhostUIManager deselectedGhostUIManager;
    protected PlayerAbilityUIManager basicAbilityUIManager;
    protected PlayerAbilityUIManager specialAbilityUIManager;
    protected PlayerInWorldMeterUIManager meterUIManager;

    protected bool isInParty;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        stats = GetComponent<StatManager>();
        partyManager = PartyManager.instance;
        ghostIdentity = GetComponent<GhostIdentity>();
        selectedGhostUIManager = PlayerSelectedGhostUIManager.instance;
        deselectedGhostUIManager = PlayerGhost1UIManager.instance;
        basicAbilityUIManager = selectedGhostUIManager.basicAbilityUIManager;
        specialAbilityUIManager = selectedGhostUIManager.specialAbilityUIManager;
        meterUIManager = PlayerInWorldMeterUIManager.instance;
        isInParty = false;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        updatePartyStatus();
    }

    private void updatePartyStatus()
    {
        if (!isInParty && partyManager.GetGhostMajorList().Contains(ghostIdentity))
        {
            isInParty = true;
            Color ghostColor = ghostIdentity.GetCharacterInfo().primaryColor;
            if (partyManager.GetGhostMajorList().Count > 0 && partyManager.GetGhostMajorList()[0].Equals(ghostIdentity)) deselectedGhostUIManager = PlayerGhost1UIManager.instance;
            if (partyManager.GetGhostMajorList().Count > 1 && partyManager.GetGhostMajorList()[1].Equals(ghostIdentity)) deselectedGhostUIManager = PlayerGhost2UIManager.instance;
            deselectedGhostUIManager.gameObject.SetActive(true);
            deselectedGhostUIManager.updateBackgroundColor(ghostColor);
            deselectedGhostUIManager.updateIcon(ghostIdentity.GetCharacterInfo().characterIcon);
            deselectedGhostUIManager.updateIconFrameColor(ghostColor);
            updateAbilityUI(false);
        }
    }

    public virtual void Select(GameObject player)
    {
        Color ghostColor = ghostIdentity.GetCharacterInfo().primaryColor;
        selectedGhostUIManager.updateBackgroundColor(ghostColor);
        selectedGhostUIManager.updateHealthBarFrameColor(ghostColor);
        selectedGhostUIManager.updateIcon(ghostIdentity.GetCharacterInfo().characterIcon);
        selectedGhostUIManager.updateIconFrameColor(ghostColor);
        meterUIManager.updateBackgroundColor(ghostColor);
        updateAbilityUI(true);
    }

    public virtual void DeSelect(GameObject player)
    {
        Color ghostColor = ghostIdentity.GetCharacterInfo().primaryColor;
        deselectedGhostUIManager.updateBackgroundColor(ghostColor);
        deselectedGhostUIManager.updateIcon(ghostIdentity.GetCharacterInfo().characterIcon);
        deselectedGhostUIManager.updateIconFrameColor(ghostColor);
        meterUIManager.deactivateWidget();
        updateAbilityUI(false);
    }

    private void updateAbilityUI(bool selected)
    {
        basicAbilityUIManager = (selected) ? selectedGhostUIManager.basicAbilityUIManager : deselectedGhostUIManager.basicAbilityUIManager;
        specialAbilityUIManager = (selected) ? selectedGhostUIManager.specialAbilityUIManager : deselectedGhostUIManager.specialAbilityUIManager;

        Color ghostColor = ghostIdentity.GetCharacterInfo().primaryColor;
        basicAbilityUIManager.gameObject.SetActive(true);
        basicAbilityUIManager.updateIcon(ghostIdentity.GetCharacterInfo().basicAbilityIcon);
        basicAbilityUIManager.updateFrameColor(ghostColor);

        specialAbilityUIManager.gameObject.SetActive(true);
        specialAbilityUIManager.updateIcon(ghostIdentity.GetCharacterInfo().specialAbilityIcon);
        specialAbilityUIManager.updateFrameColor(ghostColor);
    }

    protected void setDefaultBasicAbilityUI()
    {
        basicAbilityUIManager.setAbilityEnabled(true);
        basicAbilityUIManager.updateMeterValue(1f, 1f);
        basicAbilityUIManager.setNumberActive(false);
        basicAbilityUIManager.setAbilityHighlighted(false);
        basicAbilityUIManager.setChargesWidgetActive(false);
    }

    protected void setDefaultSpecialAbilityUI()
    {
        specialAbilityUIManager.setAbilityEnabled(true);
        specialAbilityUIManager.updateMeterValue(1f, 1f);
        specialAbilityUIManager.setNumberActive(false);
        specialAbilityUIManager.setAbilityHighlighted(false);
        specialAbilityUIManager.setChargesWidgetActive(false);
    }

}
