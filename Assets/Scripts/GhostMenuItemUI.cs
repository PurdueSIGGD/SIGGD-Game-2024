using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GhostMenuItemUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] public Image levelCircle;
    [SerializeField] public TextMeshProUGUI levelComponent;
    [SerializeField] public Image imageComponent;
    [SerializeField] public GameObject unusedIndicator;
    [SerializeField] public Image expBar;
    [SerializeField] public Slider expSlider;

    public GhostIdentity identity;
    private PartyManagerUI partyUI;
    private PartyManager partyManager;
    private SkillTree skillTree;

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
        imageComponent.sprite = info.hudIconNoFire;
        //imageComponent.sprite = (ghost.GetComponent<SkillTree>().GetLevel() >= 11) ? info.hudIcon : info.hudIconNoFire;
        Color levelBackgroundC = info.primaryColor;
        levelBackgroundC.r -= 0.2f;
        levelBackgroundC.g -= 0.2f;
        levelBackgroundC.b -= 0.2f;
        levelCircle.color = levelBackgroundC;
        int ghostLevel = ghost.GetComponent<SkillTree>().GetLevel();
        levelComponent.text = ghostLevel.ToString();

        expBar.color = info.primaryColor;
        expSlider.value = (ghostLevel >= 14) ? (1f) : (ghost.GetExp() / (float)ghost.GetRequiredExp());

        skillTree = ghost.GetComponent<SkillTree>();

        unusedIndicator.GetComponent<Image>().color = info.highlightColor;
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

    public void FixedUpdate()
    {
        imageComponent.sprite = (PartyManager.instance.GetGhostPartyList().Contains(identity)) ? identity.GetCharacterInfo().hudIcon : identity.GetCharacterInfo().hudIconNoFire;

        for (int i = 0; i < 3; i++)
        {
            if (skillTree.GetTierPoints(i) > 0)
            {
                unusedIndicator.SetActive(true);
                return;
            }
        }
        unusedIndicator.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        /*if (partyUI.GetSelectedGhost() == this)
        {
            partyUI.VisualizeOrion();
        }
        else
        {
            partyUI.VisualizeDetails(this, identity);
        }*/
    }

    public void Selected()
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
