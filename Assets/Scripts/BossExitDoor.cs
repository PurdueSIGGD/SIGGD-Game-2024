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
                SaveManager.data.north.bossProgress = 1;
                break;
            case "Eva":
                convo = eva;
                SaveManager.data.eva.bossProgress = 1;
                break;
            case "Akihito":
                convo = akihito; 
                SaveManager.data.akihito.bossProgress = 1;
                break;
            case "Yume":
                convo = yume;
                SaveManager.data.yume.bossProgress = 1;
                break;
            case "Silas":
                SaveManager.data.silas.bossProgress = 1;
                break;
            case "Aegis":
                SaveManager.data.aegis.bossProgress = 1;
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
        else
        {
            SaveManager.instance.Save();
            StartCoroutine(FadeToHub());
        }
    }

    private void OnFinishGhostDialogue(string key)
    {
        if (key.Equals(doorLock)) 
        {
            SaveManager.instance.Save();
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
