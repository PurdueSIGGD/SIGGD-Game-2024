using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

/// <summary>
/// 
/// Dialogue Format: TODO
/// TODO: What should happen at the end?
/// 
/// DialogueBox contains:
/// 
/// When should the text file be loaded?
/// 
/// </summary>
public class DialogueManager : MonoBehaviour
{

    private const string DELIMITER = "//"; // Delimiter between the dialogue of two characters is //

    private const string DEFAULT_TEXT = "..."; // Empty text box displays this

    [SerializeField]
    private TextAsset dialogueTextAsset; // Dialogue text asset

    private GameObject dialogueBox; // PANEL where the text is displayed

    private TMP_Text dialogueText; // Set this to change dialogue text on screen

    private string dialogueToRead = ""; // Dialogue text, will be altered

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
            dialogueToRead = dialogueTextAsset.text;
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

        int ind = dialogueToRead.IndexOf('\n');
        bool doAgain = false; // if character name was read, this flag tells the program to read the next line

        if (ind < 0)
        {
            EndDialogue();
            return;
        }

        // display next line and remove from text to read

        string line = dialogueToRead.Substring(0, ind - 1);

        if (line.Equals(DELIMITER))
        {
            // Debug.Log("Character changed");

            // Set name panel TODO

            // Read next line 
            doAgain = true;
        }
        else
        {
            // set text (TODO: better code)
            dialogueText.text = line;
        }

        // chop dialogue for next read
        dialogueToRead = dialogueToRead.Substring(ind + 1);

        if (doAgain) {
            NextDialogue();
        }

    }

    /// <summary>
    /// 
    /// </summary>
    void EndDialogue() {
        nextButton.gameObject.SetActive(false);
        isPlaying = false;
        dialogueText.text = DEFAULT_TEXT;
    }

    // Start is called before the first frame update
    void Start()
    {

        // Set next button to disabled and add action listener
        nextButton = transform.Find("NextButton").gameObject.GetComponent<Button>();
        nextButton.gameObject.SetActive(false);
        nextButton.onClick.AddListener(NextDialogue);

        // Initialize start button
        startButton = transform.Find("StartDialogueButton").gameObject.GetComponent<Button>();
        startButton.gameObject.SetActive(true);
        startButton.onClick.AddListener(StartDialogue);

        // Find dialogue box Game Object and text
        dialogueBox = this.transform.Find("DialogueBox").gameObject;
        dialogueText = dialogueBox.transform.Find("DialogueText").gameObject.GetComponent<TMP_Text>();
        dialogueText.text = DEFAULT_TEXT;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
