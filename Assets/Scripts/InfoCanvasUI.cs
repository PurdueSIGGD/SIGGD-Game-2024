using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InfoCanvasUI : MonoBehaviour
{

    public static readonly string ADD_PARTY_LABEL = "Add to Party";
    public static readonly string REMOVE_PARTY_LABEL = "Remove from Party";

    public static InfoCanvasUI instance = null;

    [Header("Selected Ghost Details")]

    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] Image lvlBackground;
    [SerializeField] TextMeshProUGUI lvlText;
    [SerializeField] Image expBar;
    [SerializeField] Slider expSlider;
    [SerializeField] TextMeshProUGUI expText;
    [SerializeField] Image posterImage;
    [SerializeField] Image posterShadowImage;
    [SerializeField] Image borderHiglight;
    [SerializeField] GhostIdentity ghostToShow;

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

    [Header("Miscellaneous")]
    [SerializeField] private Button addToPartyBtn;
    [SerializeField] private TextMeshProUGUI addToPartyLabel;

    private GhostMenuItemUI selectedItem = null;

    public delegate void OnAddParty();
    public OnAddParty onAddParty;

    private void Awake()
    {
        instance = this;
        gameObject.SetActive(false);
    }

    private void Start()
    {
        instance = this;
    }

    private void Update()
    {
        /*if (selectedItem == null)
        {
            addToPartyBtn.gameObject.SetActive(false);
        }
        else
        {
            addToPartyBtn.gameObject.SetActive(true);

            if (PartyManager.instance.IsGhostInParty(selectedItem.identity))
            {
                addToPartyLabel.text = REMOVE_PARTY_LABEL;
            }
            else
            {
                addToPartyLabel.text = ADD_PARTY_LABEL;
            }
        }*/
    }

    public void OpenPartyMenu()
    {
        addToPartyBtn.gameObject.SetActive(true);

        gameObject.SetActive(true);

        VisualizeDetails(ghostToShow);

        if (PlayerUIVisibility.instance) PlayerUIVisibility.instance.HidePlayerUI();
        PlayerID.instance.FreezePlayerMouse();
    }

    public void ClosePartyMenu()
    {
        gameObject.SetActive(false);
        if (PlayerUIVisibility.instance) PlayerUIVisibility.instance.ShowPlayerUI();
        PlayerID.instance.UnfreezePlayerMouse();
    }

    public void TryAddToParty()
    {
        onAddParty?.Invoke();
        ClosePartyMenu();
    }

    public void VisualizeDetails(GhostIdentity ghost)
    {
        CharacterSO character = ghost.GetCharacterInfo();
        nameText.text = character.displayName;
        posterImage.sprite = character.fullImage;
        posterShadowImage.sprite = character.fullImage;

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
        //borderHiglight.color = ghost.GetCharacterInfo().primaryColor;

        Color backgroundGradientColor = ghost.GetCharacterInfo().primaryColor;
        backgroundGradientColor.a = 0.04f;
        borderHiglight.color = backgroundGradientColor;

        Debug.Log(ghost.name + ": " + ghost.GetExp() / (float)ghost.GetRequiredExp());
        Debug.Log(ghost.name + ": " + expSlider.value);
    }
}