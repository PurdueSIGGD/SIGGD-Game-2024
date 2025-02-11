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
/// Runs conversations.
/// 
/// DialogueBox should contain (this script uses)
/// CharacterNameText
/// DialogueText
///  
/// </summary>
public class DialogueManager : MonoBehaviour
{

    private const string DEFAULT_TEXT = "..."; // Empty text box displays this

    public ConversationTemp conversation; // Conversation Scriptable Object

    private GameObject dialogueBox; // PANEL where the text is displayed

    private TMP_Text dialogueText; // Set this to change dialogue text on screen

    private TMP_Text characterNameText; // Set this to character name (who is speaking?)

    private Button nextButton; // When clicked, causes the next line of dialogue to be displayed

    bool isRunning = false; // Whether a dialogue is currently being run.

    int currentLine = 0; // which line is currently being read

    /// <summary>
    /// Pass in a ConversationTemp scriptable object to start dialogue.
    /// Updates buttons, starts first line of dialogue
    /// </summary>
    public void StartDialogue(ConversationTemp conversationToRun) {

        if (!isRunning) {
            conversation = conversationToRun;
            this.gameObject.SetActive(true);
            NextDialogue();
            isRunning = true;
        }

    }

    /// <summary>
    /// Called when the start or next button is clicked.
    /// </summary>
    void NextDialogue()
    {
        // check if we are at end if dialogue

        if (currentLine == conversation.dialogueLines.Count)
        {
            EndDialogue();
            return;
        }

        // Display next line
        dialogueText.text = conversation.dialogueLines[currentLine].line;

        // Set name
        characterNameText.text = conversation.dialogueLines[currentLine].character.characterName;

        // TODO: set image

        currentLine++;

    }

    /// <summary>
    /// Called when the last line of dialogue is read.
    /// </summary>
    void EndDialogue() {
        // Reset for next play
        characterNameText.text = "";
        dialogueText.text = DEFAULT_TEXT;
        this.gameObject.SetActive(false);
        isRunning = false;
    }

    // Start is called before the first frame update
    void Start()
    {

        this.gameObject.SetActive(false);

        // Set next button to disabled and add action listener
        nextButton = transform.Find("NextButton").gameObject.GetComponent<Button>();
        nextButton.gameObject.SetActive(false);
        nextButton.onClick.AddListener(NextDialogue);

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
