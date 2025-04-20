using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class WorldInteract : MonoBehaviour
{
    // ==============================
    //       Serialized Fields
    // ==============================

    [SerializeField]
    GameObject canvasTemplate;

    [SerializeField]
    GameObject buttonTemplate;

    // ==============================
    //        Other Variables
    // ==============================


    // ==============================
    //        Unity Functions
    // ==============================


    // ==============================
    //       Private Functions
    // ==============================


    // ==============================
    //        Public Functions
    // ==============================

    /// <summary>
    /// Creates a popup interaction menu in world space centered at given point
    /// Creates a button with name and action corresponding to each interaction option
    /// </summary>
    /// <returns> Returns the popup interaction menu game object </returns>
    public GameObject CreateInteractMenu(Vector3 centerPoint, params InteractOption[] options)
    {
        GameObject menu = Instantiate(canvasTemplate, centerPoint, Quaternion.identity);
        foreach (var option in options)
        {
            GameObject optionBtn = Instantiate(buttonTemplate, menu.transform);
            optionBtn.GetComponentInChildren<TextMeshProUGUI>().SetText(option.OptionName);
            optionBtn.GetComponent<Button>().onClick.AddListener(option.OptionAction);
        }

        return menu;
    }
}

/// <summary>
/// Struct defining an interaction option which consists of a name and action to take when option is selected
/// </summary>
public struct InteractOption
{
    public InteractOption(string optionName, UnityAction optionAction)
    {
        this.OptionName = optionName;
        this.OptionAction = optionAction;
    }

    public string OptionName { get; }
    public UnityAction OptionAction { get; }
}

