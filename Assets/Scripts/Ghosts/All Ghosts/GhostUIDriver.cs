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
    protected PlayerAbilityUIManager skill1UIManager;
    protected PlayerAbilityUIManager skill2UIManager;
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
        skill1UIManager = selectedGhostUIManager.skill1UIManager;
        skill2UIManager = selectedGhostUIManager.skill2UIManager;
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
        skill1UIManager = (selected) ? selectedGhostUIManager.skill1UIManager : deselectedGhostUIManager.skill1UIManager;
        skill2UIManager = (selected) ? selectedGhostUIManager.skill2UIManager : deselectedGhostUIManager.skill2UIManager;

        Color ghostColor = ghostIdentity.GetCharacterInfo().primaryColor;
        basicAbilityUIManager.gameObject.SetActive(true);
        basicAbilityUIManager.updateIcon(ghostIdentity.GetCharacterInfo().basicAbilityIcon);
        basicAbilityUIManager.updateFrameColor(ghostColor);
        setDefaultAbilityUI(basicAbilityUIManager, true);

        specialAbilityUIManager.gameObject.SetActive(true);
        specialAbilityUIManager.updateIcon(ghostIdentity.GetCharacterInfo().specialAbilityIcon);
        specialAbilityUIManager.updateFrameColor(ghostColor);
        setDefaultAbilityUI(specialAbilityUIManager, true);

        skill1UIManager.gameObject.SetActive(false);
        skill1UIManager.updateIcon(ghostIdentity.GetCharacterInfo().basicAbilityIcon);
        skill1UIManager.updateFrameColor(ghostColor);
        setDefaultAbilityUI(skill1UIManager, true);

        skill2UIManager.gameObject.SetActive(false);
        skill2UIManager.updateIcon(ghostIdentity.GetCharacterInfo().basicAbilityIcon);
        skill2UIManager.updateFrameColor(ghostColor);
        setDefaultAbilityUI(skill2UIManager, true);
    }

    /// <summary>
    /// Sets the specified ability UI manager to its default active state, with the charge widget turned off.
    /// </summary>
    /// <param name="abilityUIManager">The ability UI manager to modify.</param>
    /// <param name="resetHighlight">If true, the ability's highlighted state will be set false. If false, the ability's highlighted state will not be affected.
    /// NOTE: Do NOT reset the highlight state if the state will be immedately turned on again after this function call.</param>
    protected void setDefaultAbilityUI(PlayerAbilityUIManager abilityUIManager, bool resetHighlight)
    {
        abilityUIManager.gameObject.SetActive(true);
        abilityUIManager.setAbilityEnabled(true);
        abilityUIManager.updateMeterValue(1f, 1f);
        abilityUIManager.setNumberActive(false);
        if (resetHighlight) abilityUIManager.setAbilityHighlighted(false);
        abilityUIManager.setChargesWidgetActive(false);
    }

}
