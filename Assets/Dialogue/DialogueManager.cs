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
/// DialogueBox should contain (this script uses)
/// CharacterNameText
/// DialogueText
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

    private TMP_Text characterNameText; // Set this to character name (who is speaking?)

    private string dialogueToRead = ""; // Dialogue text, will be altered

    private Button startButton; // When clicked, causes the dialogue to start
    private Button nextButton; // When clicked, causes the next line of dialogue to be displayed

    //private GameObject nextButton; // button to go to next line of dialogue
    //private GameObject startButton; // button to initiate dialogue

    /// <summary>
    /// Called when the Start Button is clicked
    /// Updates buttons, starts first line of dialogue
    /// </summary>
    void StartDialogue() {
        dialogueToRead = dialogueTextAsset.text;
        nextButton.gameObject.SetActive(true);
        startButton.gameObject.SetActive(false);
        NextDialogue();
    }

    /// <summary>
    /// Called when the start or next button is clicked.
    /// </summary>
    void NextDialogue()
    {
        // get & check next line

        int ind = dialogueToRead.IndexOf('\n');
        bool doAgain = false; // if character name was read, this flag tells the program to read the next line

        if (ind < 0)
        {
            EndDialogue();
            return;
        }

        // display next line and remove from text to read

        string line = dialogueToRead.Substring(0, ind - 1);

        if (line.Contains(DELIMITER))
        {
            // Set name
            characterNameText.text = line.Substring(DELIMITER.Length); // chop off first two characters

            // Flag to read first line of next character's dialogue
            doAgain = true;
        }
        else
        {
            // set text
            dialogueText.text = line;
        }

        // chop dialogue for next read
        dialogueToRead = dialogueToRead.Substring(ind + 1);

        if (doAgain) {
            NextDialogue();
        }

    }

    /// <summary>
    /// Called when the last line of dialogue is read. TODO: Figure out how to reset for next play.
    /// </summary>
    void EndDialogue() {
        nextButton.gameObject.SetActive(false);
        dialogueText.text = DEFAULT_TEXT;

        // Reset for next play
        dialogueToRead = dialogueTextAsset.text;
        characterNameText.text = "";

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

        // Do the same for character name
        characterNameText = dialogueBox.transform.Find("CharacterNameText").gameObject.GetComponent<TMP_Text>();
        characterNameText.text = "";

    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
