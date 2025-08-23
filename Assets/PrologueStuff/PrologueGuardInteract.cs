using TMPro;
using UnityEngine;

public class PrologueGuardInteract : MonoBehaviour
{
    [SerializeField] ConvoSO prologueGuardConvo;
    [SerializeField] FadeInUI attackTutorialPrompt;


    void OnEnable()
    {
        PlayerID.instance.GetComponent<Animator>().SetBool("CanAttack", false);
        DialogueManager.onFinishDialogue += EnablePlayerAttack;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
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
