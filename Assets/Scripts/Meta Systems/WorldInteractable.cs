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
    [SerializeField] private Canvas canvas;
    [SerializeField] private RectTransform rectTransform;

    protected List<WorldInteractableOption> options;

    // These two methods are to be implemented by each specific menu instance.
    public abstract void InitializeOptions();
    public abstract void UpdateOptions(); // Updates the options that are available to the player and will be displayed when the menu spawns (or greyed out?)

    private void OnEnable()
    {
        options = new List<WorldInteractableOption> ();
        canvas.enabled = false;
        InitializeOptions();
    }

    // Update function checks if player is in range, then updates and displays the options list.
    private void Update()
    {
        canvas.enabled = false;

        Vector3 dist = player.transform.position - transform.position;
        if (dist.magnitude > activationRange) return;

        canvas.enabled = true;
        rectTransform.position = cam.WorldToScreenPoint(transform.position); // TODO check if menu is in cam bounds
        Debug.Log("Player is in menu range");

        UpdateOptions();


        /*
        foreach (WorldInteractableOption opt in options) // TODO enumerate all options available
        {
        
        }
        */
    }
}
