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
        imageComponent.sprite = (ghost.GetComponent<SkillTree>().GetLevel() >= 11) ? info.hudIcon : info.hudIconNoFire;
        //levelCircle.color = info.primaryColor;
        Color levelBackgroundC = info.primaryColor;
        //levelBackgroundC.a = 0.45f;
        levelBackgroundC.r -= 25f;
        levelBackgroundC.g -= 25f;
        levelBackgroundC.b -= 25f;
        levelCircle.color = levelBackgroundC;
        levelComponent.text = ghost.GetComponent<SkillTree>().GetLevel().ToString();

        expBar.color = info.primaryColor;
        expSlider.value = ghost.GetExp() / (float)ghost.GetRequiredExp();

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
