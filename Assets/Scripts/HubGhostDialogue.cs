using UnityEngine;

public class HubGhostDialogue : MonoBehaviour
{
    [SerializeField]
    private ConversationSO firstArrival;

    [SerializeField]
    private ConversationSO general;

    // Private
    private bool firstTime = true;

    public void TriggerHubDialogue()
    {
        DialogueManager dialogueManager = FindAnyObjectByType<DialogueManager>();
        if (firstTime)
        {
            dialogueManager.StartDialogue(firstArrival);
            firstTime = false;
        }
        else
        {
            dialogueManager.StartDialogue(general);
        }
    }
}
