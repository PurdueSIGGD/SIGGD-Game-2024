using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PartyManagerUI : MonoBehaviour
{
    //[SerializeField] GameObject menuItemPrefab;
    //[SerializeField] GameObject partyBar;
    //[SerializeField] GameObject bankBar;

    [Header("Selected Ghost Details")]

    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI lvlText;
    [SerializeField] TextMeshProUGUI expText;
    [SerializeField] Image posterImage;
    [SerializeField] Slider expSlider;

    [Header("All Unlocked Ghosts")]
    [SerializeField] GhostMenuItemUI[] ghostUis;

    private GhostMenuItemUI selectedItem = null;

    private void Awake()
    {
        gameObject.SetActive(false);
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
                Debug.Log("AAA: " + ghost.name);
                ghostUis[i].gameObject.SetActive(true);
                ghostUis[i].Visualize(ghost);
            }
            else
            {
                ghostUis[i].gameObject.SetActive(false);
            }
        }
    }

    public void ClosePartyMenu()
    {
        gameObject.SetActive(false);
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
        //ghost.GetComponent<Skill>
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