using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

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
    [SerializeField] Image posterShadowImage;
    [SerializeField] Image posterGradient;

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

    [Header("All Ghosts")]
    [SerializeField] GhostIdentity[] ghostIdentitys;

    [Header("Party Slots")]
    [SerializeField] GhostSlotVisualizer partySlot1;
    [SerializeField] GhostSlotVisualizer partySlot2;

    [Header("Miscellaneous")]
    [SerializeField] private CharacterSO orionSO;
    [SerializeField] private Button addToPartyBtn;
    [SerializeField] private Button viewSkillsBtn;
    [SerializeField] private TextMeshProUGUI addToPartyLabel;

    private GhostMenuItemUI selectedItem = null;

    public delegate void OnGhostSelected();
    public OnGhostSelected onGhostSelected;

    public delegate void OnMenuClose();
    public OnMenuClose onMenuClose;

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

        List<GhostIdentity> identities = PartyManager.instance.GetGhostPartyList();

        if (identities.Count >= 1)
            partySlot1.Visualize(identities[0]);
        else
            partySlot1.ClearVisuals();
        if (identities.Count >= 2)
            partySlot2.Visualize(identities[1]);
        else
            partySlot2.ClearVisuals();

        PlayerGhost1UIManager.instance.gameObject.SetActive(false);
        PlayerGhost2UIManager.instance.gameObject.SetActive(false);
    }

    public void OpenPartyMenu()
    {
        gameObject.SetActive(true);

        GhostIdentity[] ghosts = ghostIdentitys;

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

        if (PlayerUIVisibility.instance) PlayerUIVisibility.instance.HidePlayerUI();
        PlayerID.instance.FreezePlayerMouse();
    }

    public void ClosePartyMenu()
    {
        gameObject.SetActive(false);
        if (PlayerUIVisibility.instance) PlayerUIVisibility.instance.ShowPlayerUI();
        PlayerID.instance.UnfreezePlayerMouse();
        onMenuClose?.Invoke();
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
        posterShadowImage.sprite = character.fullImage;
        
        Color backgroundGradientColor = ghost.GetCharacterInfo().primaryColor;
        backgroundGradientColor.a = 0.04f;
        posterGradient.color = backgroundGradientColor;


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
        posterShadowImage.sprite = orionSO.fullImage;

        Color backgroundGradientColor = orionSO.primaryColor;
        backgroundGradientColor.a = 0.04f;
        posterGradient.color = backgroundGradientColor;

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

        Color nameBackgroundC = orionSO.primaryColor;
        nameBackgroundC.a = 0.45f;
        lvlBackground.color = nameBackgroundC;
    }

    public void StartRun()
    {
        onGhostSelected?.Invoke();
    }

    public GhostMenuItemUI GetSelectedGhost()
    {
        return selectedItem;
    }
}