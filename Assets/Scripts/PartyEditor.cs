using System.Collections.Generic;
using UnityEngine;

public class PartyEditor : MonoBehaviour
{
    [SerializeField] GameObject menuItemPrefab;
    [SerializeField] GameObject partyBar;
    [SerializeField] GameObject bankBar;

    PartyManager partyManager;

    GhostIdentity[] identities;
    List<GhostMenuItem> menuItems = new();
    GhostMenuItem selectedMenuItem = new();

    void Start()
    {
        partyManager = FindObjectOfType<PartyManager>();
        identities = FindObjectsOfType<GhostIdentity>();
    }

    void OnEnable()
    {
        menuItems.Clear();
        foreach (GhostIdentity identity in identities)
        {
            GameObject menuItemObject = Instantiate(menuItemPrefab);
            GhostMenuItem ghostMenuItem = menuItemObject.GetComponent<GhostMenuItem>();
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

    public void Select(GhostMenuItem menuItem)
    {
        selectedMenuItem.SetSelected(false);
        menuItem.SetSelected(true);
    }

    public void UIAdd()
    {

    }

    public void UIRemove()
    {

    }
}
