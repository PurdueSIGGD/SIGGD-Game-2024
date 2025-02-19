using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class GhostInteract : MonoBehaviour
{
    [SerializeField]
    private float interactRange;

    [SerializeField]
    private Vector3 menuOffset;

    private GameObject interactMenu;
    private GhostIdentity identity;

    void Start()
    {
        identity = GetComponent<GhostIdentity>();
    }


    public void Update()
    {
        if (!identity.IsInParty())
        {
           CheckInteractMenu();
        }
    }

    private void CheckInteractMenu()
    {
        if (PlayerInRange() && interactMenu == null)
        {
            CreateInteractMenu();
        }
        else if (!PlayerInRange() && interactMenu != null)
        {
            Destroy(interactMenu);
            interactMenu = null;
        }
    }

    private bool PlayerInRange()
    {
        float dist = Vector3.Distance(PlayerID.instance.transform.position, this.transform.position);

        return (dist < interactRange);
    }

    private void CreateInteractMenu()
    {
        WorldInteract WI = FindAnyObjectByType<WorldInteract>();
        WorldInteract.InteractOption opt1 = new WorldInteract.InteractOption("Talk", null);
        WorldInteract.InteractOption opt2 = new WorldInteract.InteractOption("Add to Party", AddGhostToParty);

        Vector3 menuPos = this.transform.position + menuOffset;

        interactMenu = WI.CreateInteractMenu(menuPos, opt1, opt2);
    }

    private void AddGhostToParty()
    {
        Destroy(interactMenu);
        interactMenu = null;

        PartyManager partyManger = PlayerID.instance.GetComponent<PartyManager>();
        partyManger.AddGhostToParty(this.GetComponent<GhostIdentity>());
    }
}