using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GhostMenuItemUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] public TextMeshProUGUI levelComponent;
    [SerializeField] public Image imageComponent;
    [SerializeField] public GameObject unusedIndicator;
    [SerializeField] public TextMeshProUGUI expText;
    [SerializeField] public Image expBar;
    [SerializeField] public Slider expSlider;

    public GhostIdentity identity;
    private PartyManagerUI partyUI;
    private PartyManager partyManager;

    private void Start()
    {
        partyManager = FindFirstObjectByType<PartyManager>();
        partyUI = FindFirstObjectByType<PartyManagerUI>();
    }

    void Update()
    {
        /*inPartyIndicator.SetActive(partyManager.IsGhostInParty(identity));
        borderComponent.gameObject.SetActive(partyUI.GetSelectedGhost() == this);*/
    }

    public void Visualize(GhostIdentity ghost)
    {
        identity = ghost;
        CharacterSO info = ghost.GetCharacterInfo();
        imageComponent.sprite = info.hudIcon;
        levelComponent.text = ghost.GetComponent<SkillTree>().GetLevel().ToString();

        expText.text = Mathf.Min(ghost.GetExp(), ghost.GetRequiredExp()) + " / " + ghost.GetRequiredExp();
        expBar.color = info.primaryColor;
        expSlider.value = ghost.GetExp() / (float)ghost.GetRequiredExp();

        SkillTree skillTree = ghost.GetComponent<SkillTree>();

        for (int i = 0; i < 3; i++)
        {
            Debug.Log(info.displayName + "'s Tier " + i + " points: " + skillTree.GetTierPoints(i));
            if (skillTree.GetTierPoints(i) > 0)
            {
                unusedIndicator.SetActive(true);
                return;
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (partyUI.GetSelectedGhost() == this)
        {
            partyUI.VisualizeOrion();
        }
        else
        {
            partyUI.VisualizeDetails(this, identity);
        }
    }

}
