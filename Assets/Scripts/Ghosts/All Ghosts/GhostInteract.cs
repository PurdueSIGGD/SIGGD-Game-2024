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


    public void Update()
    {
        if (PlayerInRange() && interactMenu == null)
        {
            CreateInteractMenu();
        } else if (!PlayerInRange() && interactMenu != null)
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
        WorldInteract.InteractOption opt2 = new WorldInteract.InteractOption("Add to Party", null);

        Vector3 menuPos = this.transform.position + menuOffset;

        interactMenu = WI.CreateInteractMenu(menuPos, opt1, opt2);
    }
}