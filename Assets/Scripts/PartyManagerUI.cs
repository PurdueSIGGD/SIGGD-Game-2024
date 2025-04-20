using System.Collections.Generic;
using UnityEngine;

public class PartyManagerUI : MonoBehaviour
{
    //[SerializeField] GameObject menuItemPrefab;
    //[SerializeField] GameObject partyBar;
    //[SerializeField] GameObject bankBar;

    [SerializeField] GameObject[] ghostBankBar;
    [SerializeField] GameObject[] ghostPartyBar;

    private PartyManager partyManager;
    GhostIdentity[] identities;

    public List<GhostMenuItemUI> menuItems = new();
    public GhostMenuItemUI selectedMenuItem;

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
}