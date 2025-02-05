using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class WorldInteractable : MonoBehaviour
{
    private Dictionary<WorldInteractableOption, GameObject> buttons;

    [SerializeField] private GameObject player;
    [SerializeField] private Camera cam;
    [SerializeField] private float activationRange;

    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject buttonTemplate;

    void OnEnable()
    {
        canvas.enabled = false;
    }

    // Enable/disable the entire canvas if the player is in range 
    void Update()
    {
        Vector3 dist = player.transform.position - transform.position;
        if (dist.magnitude > activationRange)
        {
            canvas.enabled = false;
        }
        canvas.enabled = true;
    }

    // Instantiate all the buttons which are to display the available options
    private void InstantiateButtons(List<WorldInteractableOption> opts)
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
            labelText.text = opt.name;

            // Connect callback
            button.GetComponent<Button>().onClick.AddListener(opt.OnClick);
        }
    }

    // Rearrange the buttons to reflect the visibility of each option
    public void UpdateButtons()
    {
        float currentY = 0.0f;
        float spacing = 30.0f;

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
                currentY += spacing;
            }
            else
            {
                // Hide button
                button.SetActive(false);
            }
        }

    }
}