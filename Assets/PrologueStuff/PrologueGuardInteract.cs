using TMPro;
using UnityEngine;

public class PrologueGuardInteract : InRangeInteract
{
    [SerializeField] ConvoSO prologueGuardConvo;
    [SerializeField] FadeInUI attackTutorialPrompt;


    void OnEnable()
    {
        PlayerID.instance.GetComponent<Animator>().SetBool("CanAttack", false);
        DialogueManager.onFinishDialogue += EnablePlayerAttack;
    }

    protected override InteractOption[] GetMenuOptions()
    {
        InteractOption opt = new InteractOption("Talk", InteractWithGuard);
        return new InteractOption[] { opt };
    }

    private void InteractWithGuard()
    {
        CloseMenu();
        DialogueManager dialogueManager = FindAnyObjectByType<DialogueManager>(FindObjectsInactive.Include);
        dialogueManager.StartDialogue(prologueGuardConvo);
    }

    private void EnablePlayerAttack(string key)
    {
        attackTutorialPrompt.EnableFadeIn(true);
        PlayerID.instance.GetComponent<Animator>().SetBool("CanAttack", true);
        Destroy(this);
    }
}
