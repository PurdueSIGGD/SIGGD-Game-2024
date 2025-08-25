using System.Collections.Generic;
using UnityEngine;

public class GhostUIDriver : MonoBehaviour, ISelectable
{
    protected StatManager stats;
    protected PartyManager partyManager;
    protected GhostIdentity ghostIdentity;
    [HideInInspector] public PlayerSelectedGhostUIManager selectedGhostUIManager;
    [HideInInspector] public PlayerGhostUIManager deselectedGhostUIManager;
    [HideInInspector] public PlayerAbilityUIManager basicAbilityUIManager;
    [HideInInspector] public PlayerAbilityUIManager specialAbilityUIManager;
    [HideInInspector] public PlayerAbilityUIManager skill1UIManager;
    [HideInInspector] public PlayerAbilityUIManager skill2UIManager;
    [HideInInspector] public PlayerInWorldMeterUIManager meterUIManager;

    protected bool isInParty;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        if (PlayerSelectedGhostUIManager.instance == null)
        {
            this.enabled = false;
            return;
        }

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
        UpdatePartyStatus();
    }

    public void UpdatePartyStatus()
    {

        if (!isInParty && partyManager.IsGhostInParty(ghostIdentity))
        {
            List<GhostIdentity> ghostPartyList = partyManager.GetGhostPartyList();
            int count = ghostPartyList.Count;

            isInParty = true;
            Color ghostColor = ghostIdentity.GetCharacterInfo().primaryColor;

            if (count > 0 && ghostPartyList[0].Equals(ghostIdentity)) deselectedGhostUIManager = PlayerGhost1UIManager.instance;
            if (count > 1 && ghostPartyList[1].Equals(ghostIdentity)) deselectedGhostUIManager = PlayerGhost2UIManager.instance;

            deselectedGhostUIManager.gameObject.SetActive(true);
            deselectedGhostUIManager.setBackgroundColor(ghostColor);
            deselectedGhostUIManager.setIcon(ghostIdentity.GetCharacterInfo().hudIcon);
            deselectedGhostUIManager.setIconFrameColor(ghostColor);
            updateAbilityUI(false);
        }
        else if (isInParty && !partyManager.IsGhostInParty(ghostIdentity))
        {
            List<GhostIdentity> ghostPartyList = partyManager.GetGhostPartyList();
            int count = ghostPartyList.Count;

            isInParty = false;

            ghostIdentity.TriggerExitPartyBehavior();

            // switch to orion
            PartyManager.instance.SwitchGhostToIndex(-1);

            // remove from pedestal
            deselectedGhostUIManager.gameObject.SetActive(false);

            if (count == 1)
            {
                // For the other ghost, redraw
                GhostUIDriver driver = ghostPartyList[0].gameObject.GetComponent<GhostUIDriver>();
                driver.isInParty = false;
                driver.deselectedGhostUIManager.gameObject.SetActive(false);
                driver.UpdatePartyStatus();
            }

            // visualize orion
            PartyManagerUI.instance.VisualizeOrion();

        }
    }

    public virtual void Select(GameObject player)
    {
        Color ghostColor = ghostIdentity.GetCharacterInfo().primaryColor;
        selectedGhostUIManager.setBackgroundColor(ghostColor);
        selectedGhostUIManager.setBackground2Color(ghostColor);
        selectedGhostUIManager.setHealthBarFrameColor(ghostColor);
        selectedGhostUIManager.setIcon(ghostIdentity.GetCharacterInfo().hudIcon);
        selectedGhostUIManager.setIconFrameColor(ghostColor);
        meterUIManager.setBackgroundColor(ghostColor);
        updateAbilityUI(true);
    }

    public virtual void DeSelect(GameObject player)
    {
        Color ghostColor = ghostIdentity.GetCharacterInfo().primaryColor;
        deselectedGhostUIManager.setBackgroundColor(ghostColor);
        deselectedGhostUIManager.setIcon(ghostIdentity.GetCharacterInfo().hudIcon);
        deselectedGhostUIManager.setIconFrameColor(ghostColor);
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

        setDefaultAbilityUI(basicAbilityUIManager, true);
        basicAbilityUIManager.setIcon(ghostIdentity.GetCharacterInfo().basicAbilityIcon);
        basicAbilityUIManager.setFrameColor(ghostColor);

        setDefaultAbilityUI(specialAbilityUIManager, true);
        specialAbilityUIManager.setIcon(ghostIdentity.GetCharacterInfo().specialAbilityIcon);
        specialAbilityUIManager.setFrameColor(ghostColor);

        setDefaultAbilityUI(skill1UIManager, true);
        skill1UIManager.setUIActive(false);
        skill1UIManager.setIcon(ghostIdentity.GetCharacterInfo().basicAbilityIcon);
        skill1UIManager.setFrameColor(ghostColor);

        setDefaultAbilityUI(skill2UIManager, true);
        skill2UIManager.setUIActive(false);
        skill2UIManager.setIcon(ghostIdentity.GetCharacterInfo().basicAbilityIcon);
        skill2UIManager.setFrameColor(ghostColor);
    }

    /// <summary>
    /// Sets the specified ability UI manager to its default active state, with the charge widget turned off.
    /// </summary>
    /// <param name="abilityUIManager">The ability UI manager to modify.</param>
    /// <param name="resetHighlight">If true, the ability's highlighted state will be set false. If false, the ability's highlighted state will not be affected.
    /// NOTE: Do NOT reset the highlight state if the state will be immedately turned on again after this function call.</param>
    protected void setDefaultAbilityUI(PlayerAbilityUIManager abilityUIManager, bool resetHighlight)
    {
        abilityUIManager.setUIActive(true);
        abilityUIManager.setAbilityEnabled(true);
        abilityUIManager.setMeterValue(1f, 1f);
        abilityUIManager.setNumberActive(false);
        if (resetHighlight) abilityUIManager.setAbilityHighlighted(false);
        abilityUIManager.setChargeWidgetActive(false);
    }

}
