using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseManagerScript : MonoBehaviour
{
    //Instantiate buttons on pause menu as Button objects
    public Button resumeButton, settingsButton, mainMenuButton;
    void Start()
    {
        //Listeners watch buttons on screen and run "TaskOnClick", can be changed to other functions based on buttons.
        resumeButton.onClick.AddListener(delegate { TaskOnClick(resumeButton.name);} );
        settingsButton.onClick.AddListener(delegate { TaskOnClick(settingsButton.name); });
        mainMenuButton.onClick.AddListener(delegate { TaskOnClick(mainMenuButton.name); });
    }

    void Update()
    {
        
    }

    //Generic function for button press
    void TaskOnClick(string buttonName)
    {
        //Output this to console when Button1 or Button3 is clicked
        Debug.Log("You have clicked button " + buttonName);
    }
}
