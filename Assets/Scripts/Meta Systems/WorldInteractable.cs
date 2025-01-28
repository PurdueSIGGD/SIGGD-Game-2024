using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class WorldInteractable : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float activationRange;
    [SerializeField] private Camera cam;
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject buttonTemplate;

    protected List<WorldInteractableOption> options;
    private List<GameObject> buttons;

    // These two methods are to be implemented by each specific menu instance.
    public abstract void InitializeOptions(); // Initializes the list of options which are managed by the menu
    public abstract void UpdateOptions(); // Updates the options that are available to the player and will be displayed when the menu spawns (or greyed out?)


    private void OnEnable()
    {
        canvas.enabled = false;

        options = new List<WorldInteractableOption> ();
        InitializeOptions();

        buttons = new List<GameObject>();
        InstantiateButtons();
        buttonTemplate.SetActive(false);
    }

    // Update function checks if player is in range, then updates and displays the options list.
    private void Update()
    {
        canvas.enabled = false;
        Vector3 dist = player.transform.position - transform.position;
        if (dist.magnitude > activationRange) return;

        canvas.enabled = true;

        RectTransform panelTransform = panel.GetComponent<RectTransform> ();
        panelTransform.position = cam.WorldToScreenPoint(transform.position); // TODO check if menu is in cam bounds
        Debug.Log("Player is in menu range");

        UpdateOptions();
        UpdateButtons();
    }

    private void InstantiateButtons() // Creates all button objects which are to be displayed in the menu
    {
        foreach (WorldInteractableOption opt in options) // TODO enumerate all options available in the UI
        {
            GameObject newButton = GameObject.Instantiate(buttonTemplate, panel.transform); // TODO change this to use a preset prefab to ensure the presence of text object???
            
            // Button Label Setup
            GameObject label = newButton.transform.Find("Label").gameObject;
            TextMeshProUGUI labelText = label.GetComponent<TextMeshProUGUI>();
            labelText.fontSize = 14;
            labelText.text = opt.GetName();
            
            buttons.Add(newButton);
        }
    }

    private void UpdateButtons() // Updates the visibility and layout of all buttons in the menu
    {
        float currentY = 0;
        float buttonHeight = 30;

        // Place each new visible button at the bottom of the column of visible buttons, and then position the list where it needs to be in the panel.
        foreach (GameObject button in buttons)
        {
            RectTransform buttonTransform = button.GetComponent<RectTransform>();
            buttonTransform.anchoredPosition = new Vector2(0,currentY);
            currentY += buttonHeight;
        }
    }
}
