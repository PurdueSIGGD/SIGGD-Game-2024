using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MidPrologue : MonoBehaviour
{
    [SerializeField] Image bg;
    [SerializeField] ConvoSO convo1;
    [SerializeField] ConvoSO convo2;
    [SerializeField] Sprite bg2;
    [SerializeField] ConvoSO convo3;
    [SerializeField] Sprite bg3;
    [SerializeField] ConvoSO convo4;
    [SerializeField] Sprite bg4;

    DialogueManager dialogueManager;

    private void Start()
    {
        dialogueManager = FindAnyObjectByType<DialogueManager>(FindObjectsInactive.Include);
        dialogueManager.StartDialogue(convo1);

        DialogueManager.onFinishDialogue += DoTheThing;
    }

    private void OnDisable()
    {
        DialogueManager.onFinishDialogue -= DoTheThing;
    }

    private void DoTheThing(string key)
    {
        if (key == convo1.data.convoName)
        {
            dialogueManager.StartDialogue(convo2);
            bg.sprite = bg2;
        }
        if (key == convo2.data.convoName)
        {
            dialogueManager.StartDialogue(convo3);
            bg.sprite = bg3;
        }
        if (key == convo3.data.convoName)
        {
            dialogueManager.StartDialogue(convo4);
            bg.sprite = bg4;
        }
        if (key == convo4.data.convoName)
        {
            StartCoroutine(ThenTheThing());
        }
    }

    private IEnumerator ThenTheThing()
    {
        ScreenFader.instance.FadeOut(1, 5);
        yield return new WaitForSeconds(6.1f);
        SceneManager.LoadScene("Prologue_HubWorld");
    }
}
