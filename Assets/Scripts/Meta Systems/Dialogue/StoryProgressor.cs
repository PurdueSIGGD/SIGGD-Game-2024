using UnityEngine;

public class StoryProgresser : MonoBehaviour
{
    [SerializeField] string requiredDialogue;
    [SerializeField] string ghost;
    [SerializeField] int progressTo;
    [SerializeField] bool autoProgress;
    [SerializeField] BossController bossTrigger; // the boss to activate after dialogue, if there is one

    void OnEnable()
    {
        DialogueManager.onFinishDialogue += ProgressStory;
        DialogueManager.onFinishDialogue += StartBossAI;
    }

    void OnDisable()
    {
        DialogueManager.onFinishDialogue -= ProgressStory;
        DialogueManager.onFinishDialogue -= StartBossAI;
    }

    public void Init(string requiredDialogue, string ghost, int progressTo, bool autoProgress = false)
    {
        this.requiredDialogue = requiredDialogue;
        this.ghost = ghost;
        this.progressTo = progressTo;
        this.autoProgress = autoProgress;
        if (autoProgress)
        {
            Door.OnDoorOpened += AutoProgressStory;
        }
    }

    private void AutoProgressStory()
    {
        ProgressStory(requiredDialogue);
    }

    private void ProgressStory(string key)
    {
        if (!key.Equals(requiredDialogue) && !autoProgress) return;

        switch (ghost.ToLower())
        {
            case "death":
                SaveManager.data.death = progressTo;
                break;
            case "orion":
                SaveManager.data.orion = progressTo;
                break;
            case "north":
                SaveManager.data.north.storyProgress = progressTo;
                break;
            case "north boss":
                SaveManager.data.north.bossProgress = progressTo;
                break;
            case "eva":
                SaveManager.data.eva.storyProgress = progressTo;
                break;
            case "eva boss":
                SaveManager.data.eva.bossProgress = progressTo;
                break;
            case "yume":
                SaveManager.data.yume.storyProgress = progressTo;
                break;
            case "yume boss":
                SaveManager.data.yume.bossProgress = progressTo;
                break;
            case "akihito":
                SaveManager.data.akihito.storyProgress = progressTo;
                break;
            case "akihito boss":
                SaveManager.data.akihito.bossProgress = progressTo;
                break;
            case "silas":
                SaveManager.data.silas.storyProgress = progressTo;
                break;
            case "silas boss":
                SaveManager.data.silas.bossProgress = progressTo;
                break;
            case "aegis":
                SaveManager.data.aegis.storyProgress = progressTo;
                break;
            case "aegis boss":
                SaveManager.data.aegis.bossProgress = progressTo;
                break;
            default:
                Debug.LogError("Can not recognize ghost: " + ghost.ToLower() + " when attempting to progress story");
                break;
        }
        SaveManager.instance.Save();
    }

    private void StartBossAI(string key)
    {
        if (bossTrigger == null || !key.Equals(requiredDialogue)) return;

        bossTrigger.EnableAI();
    }
}
