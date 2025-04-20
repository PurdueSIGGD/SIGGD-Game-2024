using TMPro;
using UnityEngine;
using UnityEngine.Events;
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
public class DialogueManager : MonoBehaviour, IScreenUI
{
    // ==============================
    //       Serialized Fields
    // ==============================


    // ==============================
    //        Other Variables
    // ==============================

    private const string DEFAULT_TEXT = "..."; // Empty text box displays this

    private ConversationTemp conversation; // Conversation Scriptable Object

    private GameObject dialogueBox; // PANEL where the text is displayed

    private TMP_Text dialogueText; // Set this to change dialogue text on screen

    private TMP_Text characterNameText; // Set this to character name (who is speaking?)

    private Button nextButton; // When clicked, causes the next line of dialogue to be displayed

    private bool isRunning = false; // Whether a dialogue is currently being run.

    private int currentLine = 0; // which line is currently being read

    private UnityAction actionOnDialogueEnd = null;

    // ==============================
    //        Unity Functions
    // ==============================

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

        ToggleVisibility();
    }

    // ==============================
    //       Private Functions
    // ==============================

    /// <summary>
    /// Called when the start or next button is clicked.
    /// </summary>
    private void NextDialogue()
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
        characterNameText.text = conversation.dialogueLines[currentLine].character.displayName;

        // TODO: set image

        currentLine++;

    }

    /// <summary>
    /// Called when the last line of dialogue is read.
    /// </summary>
    private void EndDialogue()
    {
        // Reset for next play
        characterNameText.text = "";
        dialogueText.text = DEFAULT_TEXT;
        isRunning = false;
        ToggleVisibility();
        currentLine = 0;

        actionOnDialogueEnd?.Invoke();
        actionOnDialogueEnd = null;

        PlayerID.instance.UnfreezePlayer();
    }

    /// <summary>
    /// Toggles the visibility of all dialogue components.
    /// Call when isRunning is changed.
    /// </summary>
    private void ToggleVisibility()
    {
        dialogueBox.SetActive(isRunning);
        nextButton.gameObject.SetActive(isRunning);
    }

    // ==============================
    //        Other Functions
    // ==============================

    /// <summary>
    /// Pass in a ConversationTemp scriptable object to start dialogue.
    /// Sets visibility and starts first line of dialogue
    /// </summary>
    public void StartDialogue(ConversationTemp conversationToRun)
    {

        if (!isRunning)
        {
            conversation = conversationToRun;
            isRunning = true;
            ToggleVisibility();
            NextDialogue();

            PlayerID.instance.FreezePlayer();
        }
    }


    /// <summary>
    /// Calls given action the next time dialogue manager UI closes (i.e. finishes dialogue)
    /// </summary>
    public void OnNextCloseCall(UnityAction action)
    {
        actionOnDialogueEnd = action;
    }

}
