using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class WorldInteractable2 : MonoBehaviour
{
    // Set this field after instantiation and before enabling
    public List<WorldInteractableOption> options;
    
    [SerializeField] private GameObject player;
    [SerializeField] private Camera cam;
    [SerializeField] private float activationRange;

    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject buttonTemplate;
    private List<GameObject> buttons;

    void OnEnable()
    {
        canvas.enabled = false;
        InstantiateButtons();    
    }

    void Update()
    {
        Vector3 dist = player.transform.position - transform.position;
        if (dist.magnitude > activationRange)
        {
            canvas.enabled = false;
        }
        canvas.enabled = true;
    }

    private void InstantiateButtons()
    {
        buttons = new List<GameObject>();
        float currentY = 0.0f;
        float spacing = 30.0f;
        
        // For each option, create button and arrange in vertical column
        foreach (WorldInteractableOption opt in options)
        {
            GameObject button = Instantiate(buttonTemplate, panel.transform);

            // Fill button label
            GameObject labelObject = button.transform.Find("Label").gameObject;
            TextMeshProUGUI labelText = labelObject.GetComponent<TextMeshProUGUI>();
            labelText.text = opt.name;

            // Arrange button
            RectTransform buttonT = button.GetComponent<RectTransform>();
            buttonT.anchoredPosition = new Vector2(0, currentY);
            currentY += spacing;

            buttons.Add(button);
        }
    }
}
