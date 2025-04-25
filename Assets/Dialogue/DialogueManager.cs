using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

/// <summary>
/// 
/// Runs conversations.
/// 
/// DialogueBox should contain
/// CharacterNameText
/// DialogueText
/// 
/// TODO: set character profile image based on who is speaking
///  
/// </summary>
public class DialogueManager : MonoBehaviour
{

    private const string DEFAULT_TEXT = "..."; // Empty text box displays this

    private ConversationTemp conversation; // Conversation Scriptable Object
    private ConversationName conversationName;

    private GameObject dialogueBox; // PANEL where the text is displayed

    private TMP_Text dialogueText; // Set this to change dialogue text on screen

    private TMP_Text characterNameText; // Set this to character name (who is speaking?)

    private Button nextButton; // When clicked, causes the next line of dialogue to be displayed

    private bool isRunning = false; // Whether a dialogue is currently being run.

    private int currentLine = 0; // which line is currently being read

    /// <summary>
    /// Pass in a ConversationTemp scriptable object to start dialogue.
    /// Sets visibility and starts first line of dialogue
    /// </summary>
    public void StartDialogue(ConversationTemp conversationToRun) {

        if (!isRunning) {
            conversation = conversationToRun;
            isRunning = true;
            toggleVisibility();
            NextDialogue();
        }
        AudioManager.Instance.VABranch.PlayConversationLine(conversationName, 0);
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
        isRunning = false;
        toggleVisibility();
        currentLine = 0;
    }

    /// <summary>
    /// Toggles the visibility of all dialogue components.
    /// Call when isRunning is changed.
    /// </summary>
    void toggleVisibility() {
        dialogueBox.SetActive(isRunning);
        nextButton.gameObject.SetActive(isRunning);
    }

    // Start is called before the first frame update
    void Start()
    {

        // Set next button to disabled and add action listener
        nextButton = transform.Find("NextButton").gameObject.GetComponent<Button>();
        nextButton.onClick.AddListener(NextDialogue);

        // Find dialogue box Game Object and text
        dialogueBox = this.transform.Find("DialogueBox").gameObject;
        dialogueText = dialogueBox.transform.Find("DialogueText").gameObject.GetComponent<TMP_Text>();
        dialogueText.text = DEFAULT_TEXT;

        // Do the same for character name
        characterNameText = dialogueBox.transform.Find("CharacterNameText").gameObject.GetComponent<TMP_Text>();
        characterNameText.text = "";

        toggleVisibility();

    }

    // Update is called once per frame
    void Update()
    {
    }

}
