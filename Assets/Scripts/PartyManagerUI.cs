using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PartyManagerUI : MonoBehaviour
{

    public static readonly string ADD_PARTY_LABEL = "Add (Party)";
    public static readonly string REMOVE_PARTY_LABEL = "Remove (Party)";

    public static PartyManagerUI instance = null;

    [Header("Selected Ghost Details")]

    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] Image lvlBackground;
    [SerializeField] TextMeshProUGUI lvlText;
    [SerializeField] Image expBar;
    [SerializeField] Slider expSlider;
    [SerializeField] TextMeshProUGUI expText;
    [SerializeField] Image posterImage;

    [Header("Ghost Ability - Basic")]
    [SerializeField] Image basicAbilityIcon;
    [SerializeField] GameObject basicAbility;
    [SerializeField] TextMeshProUGUI basicAbilityText;
    [SerializeField] TextMeshProUGUI basicAbilityDesc;

    [Header("Ghost Ability - Special")]
    [SerializeField] Image specialAbilityIcon;
    [SerializeField] GameObject specialAbility;
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

    private void Start()
    {
        instance = this;
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
                ghost.AddExp(0);
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

    public void OpenSkillTree()
    {
        SkillTreeUI skillTreeUI = FindFirstObjectByType<SkillTreeUI>(FindObjectsInactive.Include);
        skillTreeUI.OpenSkillTree(selectedItem.identity.gameObject);
    }

    public void VisualizeDetails(GhostMenuItemUI item, GhostIdentity ghost)
    {
        selectedItem = item;
        CharacterSO character = ghost.GetCharacterInfo();
        nameText.text = character.displayName;
        posterImage.sprite = character.fullImage;

        basicAbility.gameObject.SetActive(true);
        basicAbilityIcon.sprite = character.basicAbilityIcon;
        basicAbilityIcon.color = character.primaryColor;
        basicAbilityText.text = character.basicAbilityName;
        basicAbilityDesc.text = character.basicAbilityDescription;

        specialAbilityIcon.sprite = character.specialAbilityIcon;
        specialAbilityIcon.color = character.primaryColor;
        specialAbilityText.text = character.specialAbilityName;
        specialAbilityDesc.text = character.specialAbilityDescription;

        lvlText.text = ghost.GetComponent<SkillTree>().GetLevel().ToString();
        Color nameBackgroundC = ghost.GetCharacterInfo().primaryColor;
        nameBackgroundC.a = 0.45f;
        lvlBackground.color = nameBackgroundC;
        expText.text = Mathf.Min(ghost.GetExp(), ghost.GetRequiredExp()) + " / " + ghost.GetRequiredExp();
        expBar.color = ghost.GetCharacterInfo().primaryColor;
        expSlider.value = ghost.GetExp() / (float)ghost.GetRequiredExp();
        Debug.Log(ghost.name + ": " + ghost.GetExp() / (float)ghost.GetRequiredExp());
        Debug.Log(ghost.name + ": " + expSlider.value);
    }

    public void VisualizeOrion()
    {
        selectedItem = null;
        nameText.text = orionSO.displayName;
        posterImage.sprite = orionSO.fullImage;

        basicAbility.gameObject.SetActive(false);

        basicAbilityIcon.sprite = orionSO.basicAbilityIcon;
        basicAbilityIcon.color = orionSO.primaryColor;
        basicAbilityText.text = orionSO.basicAbilityName;
        basicAbilityDesc.text = orionSO.basicAbilityDescription;

        specialAbilityIcon.sprite = orionSO.specialAbilityIcon;
        specialAbilityIcon.color = orionSO.primaryColor;
        specialAbilityText.text = orionSO.specialAbilityName;
        specialAbilityDesc.text = orionSO.specialAbilityDescription;

        lvlText.text = "";
        expText.text = "";
        expSlider.value = 0;
    }

    public GhostMenuItemUI GetSelectedGhost()
    {
        return selectedItem;
    }
}