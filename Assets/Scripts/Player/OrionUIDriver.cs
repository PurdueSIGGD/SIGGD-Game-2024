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
        if (PlayerSelectedGhostUIManager.instance == null)
        {
            this.enabled = false;
            return;
        }

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
        if (partyManager.GetGhostPartyList().Count > 0 && partyManager.GetGhostPartyList()[0].IsSelected()) deselectedGhostUIManager = PlayerGhost1UIManager.instance;
        if (partyManager.GetGhostPartyList().Count > 1 && partyManager.GetGhostPartyList()[1].IsSelected()) deselectedGhostUIManager = PlayerGhost2UIManager.instance;
    }

    private void updateGhostUI()
    {
        Color ghostColor = orionCharacterInfo.primaryColor;
        if (isSelected)
        {
            selectedGhostUIManager.setBackgroundColor(ghostColor);
            selectedGhostUIManager.setHealthBarFrameColor(ghostColor);
            selectedGhostUIManager.setIcon(orionCharacterInfo.hudIcon);
            selectedGhostUIManager.setIconFrameColor(ghostColor);
            meterUIManager.setBackgroundColor(ghostColor);
        }
        else
        {
            deselectedGhostUIManager.setBackgroundColor(ghostColor);
            deselectedGhostUIManager.setIcon(orionCharacterInfo.hudIcon);
            deselectedGhostUIManager.setIconFrameColor(ghostColor);
        }
    }

    private void updateAbilityUI()
    {
        basicAbilityUIManager = (isSelected) ? selectedGhostUIManager.basicAbilityUIManager : deselectedGhostUIManager.basicAbilityUIManager;
        specialAbilityUIManager = (isSelected) ? selectedGhostUIManager.specialAbilityUIManager : deselectedGhostUIManager.specialAbilityUIManager;
        skill1UIManager = (isSelected) ? selectedGhostUIManager.skill1UIManager : deselectedGhostUIManager.skill1UIManager;
        skill2UIManager = (isSelected) ? selectedGhostUIManager.skill2UIManager : deselectedGhostUIManager.skill2UIManager;



        // BASIC ABILITY
        basicAbilityUIManager.setUIActive(true);
        basicAbilityUIManager.setIcon(orionCharacterInfo.specialAbilityIcon);
        basicAbilityUIManager.setFrameColor(orionCharacterInfo.primaryColor);
        basicAbilityUIManager.setAbilityHighlighted(false);
        basicAbilityUIManager.setChargeWidgetActive(false);
        basicAbilityUIManager.setNumberActive(false);

        basicAbilityUIManager.setAbilityEnabled(manager.isDashEnabled, true);
        basicAbilityUIManager.setMeterValue((stats.ComputeValue("Dash Cooldown") - manager.getSpecialCooldown()), stats.ComputeValue("Dash Cooldown"));



        // SPECIAL ABILITY
        specialAbilityUIManager.setUIActive(false);

        // SKILL 1
        skill1UIManager.setUIActive(false);

        // SKILL 2
        skill2UIManager.setUIActive(false);
    }
}
