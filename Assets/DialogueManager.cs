using System;
using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

/// <summary>
/// 
/// Dialogue Format: TODO
/// 
/// DialogueBox contains:
/// 
/// When should the text file be loaded?
/// 
/// </summary>
public class DialogueManager : MonoBehaviour
{
    [SerializeField]
    private TextAsset dialogueText; // Dialogue text asset

    [SerializeField]
    private GameObject DialogueBox; // PANEL where the text is displayed

    private const String DELIMITER = "//"; // Delimiter between the dialogue of two characters is //

    private String dialogue = ""; // Dialogue text, will be altered

    private Boolean isPlaying = false; // whether the dialogue is currently being played;

    private Button startButton; // When clicked, causes the dialogue to start
    private Button nextButton; // When clicked, causes the next line of dialogue to be displayed

    //private GameObject nextButton; // button to go to next line of dialogue
    //private GameObject startButton; // button to initiate dialogue

    /// <summary>
    /// Called when the Start Button is clicked
    /// </summary>
    void StartDialogue() {
        if (!isPlaying) {
            isPlaying = true;
            nextButton.gameObject.SetActive(true);
            startButton.gameObject.SetActive(false);
            NextDialogue();
        }
    }

    /// <summary>
    /// Called when the start button is clicked.
    /// </summary>
    void NextDialogue()
    {
        // get & check next line
        // FIXME: currently doesn't read last line if there is no \n

        int ind = dialogue.IndexOf('\n');

        if (ind < 0)
        {
            EndDialogue();
            return;
        }

        // display next line and remove from text to read

        String line = dialogue.Substring(0, ind);

        if (line.Equals(DELIMITER)) {
            Debug.Log("Character changed");
        }

        dialogue = dialogue.Substring(ind + 1);

        Debug.Log(line);
        //DialogueBox.transform.Find("DialogueText").gameObject.GetComponent<Text>().text = line;

    }

    /// <summary>
    /// 
    /// </summary>
    void EndDialogue() {
        nextButton.gameObject.SetActive(false);
        isPlaying = false;
        // TODO - how to stop?
    }

    // Start is called before the first frame update
    void Start()
    {
        dialogue = dialogueText.text;

        // Set next button to disabled and add action listener
        nextButton = transform.Find("NextButton").gameObject.GetComponent<Button>();
        nextButton.gameObject.SetActive(false);
        nextButton.onClick.AddListener(NextDialogue);

        // Initialize start button
        startButton = transform.Find("StartDialogueButton").gameObject.GetComponent<Button>();
        startButton.gameObject.SetActive(true);
        startButton.onClick.AddListener(StartDialogue);


    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
