using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class WorldInteract : MonoBehaviour
{

    [SerializeField]
    GameObject canvasTemplate;

    [SerializeField]
    GameObject buttonTemplate;

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
