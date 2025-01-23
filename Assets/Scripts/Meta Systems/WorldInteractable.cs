using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class WorldInteractable : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float activationRange;
    [SerializeField] private Camera cam;
    [SerializeField] private UIBehaviour menu;
    private CanvasGroup menuGroup;

    protected List<WorldInteractableOption> options;

    // These two methods are to be implemented by each specific menu instance.
    public abstract void InitializeOptions();
    public abstract void UpdateOptions();

    private void OnEnable()
    {
        options = new List<WorldInteractableOption> ();
        menuGroup = menu.GetComponent<CanvasGroup> ();
        InitializeOptions();
    }

    // Update function checks if player is in range, then updates and displays the options list.
    private void Update()
    {
        menuGroup.alpha = 0.0f;

        Vector3 dist = player.transform.position - transform.position;
        if (dist.magnitude > activationRange) return;

        menu.transform.position = cam.WorldToScreenPoint(transform.position);
        menuGroup.alpha = 1.0f;
        UpdateOptions();


        /*
        foreach (WorldInteractableOption opt in options)
        {
        
        }
        */
    }
}
