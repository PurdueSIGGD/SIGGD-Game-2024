using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider masterVolumeSlider;
    public Button exitButton;

    public float masterVolumeValue;

    void Start()
    {
        masterVolumeSlider.onValueChanged.AddListener(delegate { TaskOnValueChanged(masterVolumeSlider.name, masterVolumeSlider.value); });
        exitButton.onClick.AddListener(delegate { TaskOnClick(exitButton.name); });

        masterVolumeValue = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void TaskOnClick(string buttonName)
    {
        //Task runs when a button is clicked
        if (buttonName == "ExitButton")
        {
            Debug.Log("You have moved button " + buttonName);
        }
    }

    void TaskOnValueChanged(string sliderName, float sliderValue)
    {
        if (sliderName == "MasterVolumeSlider")
        {
            Debug.Log("You have moved Slider " + sliderName + " to value " + sliderValue);
            masterVolumeValue = sliderValue;
        }
        
    }
}


