using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GhostMenuItemUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] public TextMeshProUGUI textComponent;
    [SerializeField] public Image imageComponent;
    [SerializeField] public Image borderComponent;
    [SerializeField] public GameObject inPartyIndicator;

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
        inPartyIndicator.SetActive(partyManager.IsGhostInParty(identity));
        borderComponent.gameObject.SetActive(partyUI.GetSelectedGhost() == this);
    }

    public void Visualize(GhostIdentity ghost)
    {
        identity = ghost;
        CharacterSO info = ghost.GetCharacterInfo();
        textComponent.text = info.name;
        imageComponent.sprite = info.characterIcon;
        borderComponent.color = info.primaryColor;
        inPartyIndicator.GetComponent<Image>().color = info.primaryColor;
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
