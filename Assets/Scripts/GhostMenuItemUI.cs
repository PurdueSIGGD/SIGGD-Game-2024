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
    public PartyManagerUI partyUI;
    private PartyManager partyManager;

    private bool initialized;

    private void Start()
    {
        partyManager = FindFirstObjectByType<PartyManager>();
        partyUI = FindFirstObjectByType<PartyManagerUI>();
    }

    void Update()
    {
        inPartyIndicator.SetActive(partyManager.IsGhostInParty(identity.gameObject));
    }

    public void Visualize(GhostIdentity ghost, bool inParty, bool isSelected)
    {
        CharacterSO info = ghost.GetCharacterInfo();
        textComponent.text = info.name;
        imageComponent.sprite = info.characterIcon;

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //skillTreeUI.TryAddPointUI(this.skill);
    }

}
