using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossExitDoor : Door
{
    [SerializeField] ConvoSO north;
    [SerializeField] ConvoSO eva;
    [SerializeField] ConvoSO akihito;
    [SerializeField] ConvoSO yume;

    private ConvoSO convo; // dialogue between orion and ghost
    private string doorLock;

    void OnEnable()
    {
        DialogueManager.onFinishDialogue += OnFinishGhostDialogue;
    }
    void OnDisable()
    {
        DialogueManager.onFinishDialogue -= OnFinishGhostDialogue;
    }

    public void SetConvo(string ghost)
    {
        switch (ghost)
        {
            case "North":
                convo = north;
                break;
            case "Eva":
                convo = eva;
                break;
            case "Akihito":
                convo = akihito;
                break;
            case "Yume":
                convo = yume;
                break;
            default:
                Debug.LogWarning("Cannot recognize ghost " + ghost);
                break;
        }
    }


    protected override void CallDoorOpened()
    {
        if (convo)
        {
            DialogueManager dialogueManager = FindAnyObjectByType<DialogueManager>(FindObjectsInactive.Include);
            dialogueManager.StartDialogue(convo);
            doorLock = convo.data.convoName;
        }
    }

    private void OnFinishGhostDialogue(string key)
    {
        if (key.Equals(doorLock)) 
        {
            StartCoroutine(FadeToHub());
        }
    }

    private IEnumerator FadeToHub()
    {
        ScreenFader.instance.FadeOut();
        yield return new WaitForSeconds(ScreenFader.instance.fadeOutDuration + 0.1f);
        SceneManager.LoadScene("HubWorld");
    }
}
