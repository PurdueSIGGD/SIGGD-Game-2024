using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/*
 *  Class for spawning a menu in the world. Expects to be fed a list of WorldInteractableOptions
 *  through InstantiateButtons, then will display a column of copies of buttonTemplate whenever
 *  the player is in range.
 */
public class WorldInteractable : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Camera cam;
    [SerializeField] private float activationRange = 15.0f;

    // Expects a hierarchy of canvas > panel > buttons
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject buttonTemplate;

    [SerializeField] private float buttonSpacing = 30.0f; // TODO change this to be calculated based on buttonTemplate size?

    // Buttons are created after feeding list of options to InstantiateButtons()
    private Dictionary<WorldInteractableOption, GameObject> buttons;

    // Invisible on instantiation
    void OnEnable()
    {
        canvas.enabled = false;
        buttonTemplate.SetActive(false);
    }

    // Enable/disable the entire canvas if the player is in range, and position it in world space 
    void Update()
    {
        Vector3 dist = player.transform.position - transform.position;
        if (dist.magnitude > activationRange)
        {
            canvas.enabled = false;
        }
        else {
            canvas.enabled = true;
            panel.GetComponent<RectTransform>().position = cam.WorldToScreenPoint(transform.position);
        }
    }

    // Instantiate all the buttons which are to display the available options, and arrange them
    public void InstantiateButtons(List<WorldInteractableOption> opts)
    {
        // Clean up prior buttons if they exist
        if (buttons != null) {
            foreach (KeyValuePair<WorldInteractableOption, GameObject> p in buttons) {
                Destroy(p.Value);
            }
        }
        buttons = new Dictionary<WorldInteractableOption, GameObject>();

        // For each option, create button and set up 
        foreach (WorldInteractableOption opt in opts)
        {
            GameObject button = Instantiate(buttonTemplate, panel.transform);
            button.SetActive(false);
            buttons[opt] = button;

            // Fill button label
            GameObject labelObject = button.transform.Find("Label").gameObject;
            TextMeshProUGUI labelText = labelObject.GetComponent<TextMeshProUGUI>();
            labelText.text = opt.GetName();

            // Connect callback
            button.GetComponent<Button>().onClick.AddListener(opt.OnClick);
        }
    }

    // Update the arrangement and visibility of the buttons
    public void UpdateButtons()
    {
        int count = buttons.Count;
        float currentY = count * buttonSpacing * 0.5f;

        foreach (KeyValuePair<WorldInteractableOption, GameObject> p in buttons)
        {
            WorldInteractableOption opt = p.Key;
            GameObject button = p.Value;
            if (opt.isVisible)
            {
                // Show button
                button.SetActive(true);
                // Arrange button
                RectTransform buttonT = button.GetComponent<RectTransform>();
                buttonT.anchoredPosition = new Vector2(0, currentY);
                currentY -= buttonSpacing;
            }
            else
            {
                // Hide button
                button.SetActive(false);
            }
        }

    }
}