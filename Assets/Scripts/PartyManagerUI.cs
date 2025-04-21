using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PartyManagerUI : MonoBehaviour
{
    public static readonly string ADD_PARTY_LABEL = "Add to Party";
    public static readonly string REMOVE_PARTY_LABEL = "Remove from Party";

    [Header("Selected Ghost Details")]

    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI lvlText;
    [SerializeField] TextMeshProUGUI expText;
    [SerializeField] Image posterImage;
    [SerializeField] Slider expSlider;

    [Header("Ghost Ability - Basic")]
    [SerializeField] Image basicAbilityIcon;
    [SerializeField] TextMeshProUGUI basicAbilityText;
    [SerializeField] TextMeshProUGUI basicAbilityDesc;

    [Header("Ghost Ability - Special")]
    [SerializeField] Image specialAbilityIcon;
    [SerializeField] TextMeshProUGUI specialAbilityText;
    [SerializeField] TextMeshProUGUI specialAbilityDesc;

    [Header("All Unlocked Ghosts")]
    [SerializeField] GhostMenuItemUI[] ghostUis;

    [Header("Miscellaneous")]
    [SerializeField] private CharacterSO orionSO;
    [SerializeField] private Button addToPartyBtn;
    [SerializeField] private Button viewSkillsBtn;
    [SerializeField] private TextMeshProUGUI addToPartyLabel;

    private GhostMenuItemUI selectedItem = null;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (selectedItem == null)
        {
            addToPartyBtn.gameObject.SetActive(false);
            viewSkillsBtn.gameObject.SetActive(false);
        }
        else
        {
            addToPartyBtn.gameObject.SetActive(true);
            viewSkillsBtn.gameObject.SetActive(true);

            if (PartyManager.instance.IsGhostInParty(selectedItem.identity))
            {
                addToPartyLabel.text = REMOVE_PARTY_LABEL;
            }
            else
            {
                addToPartyLabel.text = ADD_PARTY_LABEL;
            }
        }
    }

    public void OpenPartyMenu()
    {
        gameObject.SetActive(true);

        GhostIdentity[] ghosts = FindObjectsOfType<GhostIdentity>();

        for (int i = 0; i < ghostUis.Length; i++)
        {
            GhostIdentity ghost = (i < ghosts.Length) ? ghosts[i] : null;
            if (ghost != null && ghost.IsUnlocked())
            {
                ghostUis[i].gameObject.SetActive(true);
                ghostUis[i].Visualize(ghost);
            }
            else
            {
                ghostUis[i].gameObject.SetActive(false);
            }
        }

        VisualizeOrion();
    }

    public void ClosePartyMenu()
    {
        gameObject.SetActive(false);
    }

    public void SwitchGhostPartyStatus()
    {
        if (PartyManager.instance.IsGhostInParty(selectedItem.identity))
        {
            PartyManager.instance.RemoveGhostFromParty(selectedItem.identity);
        }
        else
        {
            PartyManager.instance.TryAddGhostToParty(selectedItem.identity);
        }
    }

    public void AddGhostToParty(GhostIdentity ghost)
    {
        PartyManager.instance.TryAddGhostToParty(ghost);
    }

    public void RemoveGhostFromParty(GhostIdentity ghost)
    {
        PartyManager.instance.RemoveGhostFromParty(ghost);
    }

    public void VisualizeDetails(GhostMenuItemUI item, GhostIdentity ghost)
    {
        selectedItem = item;
        CharacterSO character = ghost.GetCharacterInfo();
        nameText.text = character.name;
        posterImage.sprite = character.fullImage;

        basicAbilityIcon.gameObject.SetActive(true);
        basicAbilityIcon.sprite = character.basicAbilityIcon;
        basicAbilityIcon.color = character.primaryColor;
        basicAbilityText.text = character.basicAbilityName;
        basicAbilityDesc.text = character.basicAbilityDescription;

        specialAbilityIcon.sprite = character.specialAbilityIcon;
        specialAbilityIcon.color = character.primaryColor;
        specialAbilityText.text = character.specialAbilityName;
        specialAbilityDesc.text = character.specialAbilityDescription;

        lvlText.text = ghost.GetComponent<SkillTree>().GetLevel().ToString();
        Debug.Log($"AAAA: is {ghost == null}");
        string exp = ghost.GetExp() + " / " + ghost.GetRequiredExp();
        expText.text = exp;
        expSlider.value = ghost.GetExp() / (float)ghost.GetRequiredExp();
        //ghost.GetComponent<Skill>
    }

    public void VisualizeOrion()
    {
        selectedItem = null;
        nameText.text = orionSO.name;
        posterImage.sprite = orionSO.fullImage;

        basicAbilityIcon.gameObject.SetActive(false);

        basicAbilityIcon.sprite = orionSO.basicAbilityIcon;
        basicAbilityIcon.color = orionSO.primaryColor;
        basicAbilityText.text = orionSO.basicAbilityName;
        basicAbilityDesc.text = orionSO.basicAbilityDescription;

        specialAbilityIcon.sprite = orionSO.specialAbilityIcon;
        specialAbilityIcon.color = orionSO.primaryColor;
        specialAbilityText.text = orionSO.specialAbilityName;
        specialAbilityDesc.text = orionSO.specialAbilityDescription;

        lvlText.text = "";
    }

    public GhostMenuItemUI GetSelectedGhost()
    {
        return selectedItem;
    }

    /*void Awake()
    {
        partyManager = FindObjectOfType<PartyManager>();
        identities = FindObjectsOfType<GhostIdentity>();
        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        menuItems.Clear();
        partyBar.transform.DetachChildren();
        bankBar.transform.DetachChildren();

        foreach (GhostIdentity identity in identities)
        {
            GameObject menuItemObject = Instantiate(menuItemPrefab);
            GhostMenuItemUI ghostMenuItem = menuItemObject.GetComponent<GhostMenuItemUI>();
            ghostMenuItem.identity = identity;
            ghostMenuItem.menu = this;
            menuItems.Add(ghostMenuItem);

            if (ghostMenuItem.identity.IsInParty())
            {
                menuItemObject.transform.SetParent(partyBar.transform);
            }
            else
            {
                menuItemObject.transform.SetParent(bankBar.transform);
            }
        }
    }

    public void Select(GhostMenuItemUI menuItem)
    {
        if (selectedMenuItem != null)
        {
            selectedMenuItem.SetSelected(false);
        }

        selectedMenuItem = menuItem;
        menuItem.SetSelected(true);
    }

    public void UIAdd()
    {
        if (!selectedMenuItem || selectedMenuItem.identity.IsInParty()) return;

        partyManager.TryAddGhostToParty(selectedMenuItem.identity);
        selectedMenuItem.transform.SetParent(partyBar.transform);
    }

    public void UIRemove()
    {
        if (!selectedMenuItem || !selectedMenuItem.identity.IsInParty()) return;

        partyManager.RemoveGhostFromParty(selectedMenuItem.identity);
        selectedMenuItem.transform.SetParent(bankBar.transform);
    }*/

    public void ChooseGhost(GhostIdentity ghost)
    {

    }

    public void AddChosenGhost()
    {

    }
}