using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSwitching : MonoBehaviour
{
    public static Level[] levels;
    public static SpecificLevelPool[] specificLevels;
    [SerializeField] int maxLevels;
    [SerializeField] string homeWorld;

    private string nextScene = "";

    private int levelCount = 0;

    public static LevelSwitching instance;

    public float GetProgress()
    {
        return ((float)levelCount - 1.0f) / (float)maxLevels;
    }

    private void Start()
    {
        instance = this;
        GameplayEventHolder.OnDeath += ResetLevelCount;
        Door.OnDoorOpened += SwitchLevel;
    }

    private Level GetNextLevel()
    {
        Level[] levelsUsing = levels;
        foreach (SpecificLevelPool levelPool in specificLevels)
        {
            if (levelCount == levelPool.GetLevelNum())
            {
                levelsUsing = levelPool.GetLevels();
            }
        }

        float totalChance = 0.0f;
        for (int i = 0; i < levelsUsing.Length; i++)
        {
            if (levelsUsing[i].GetSceneName() != nextScene)
            {
                totalChance += levelsUsing[i].GetSceneChance();
            }
        }

        float rng = Random.Range(0, totalChance);
        float counter = 0.0f;
        for (int i = 0; i < levelsUsing.Length; i++)
        {
            if (levelsUsing[i].GetSceneName() != nextScene)
            {
                counter += levelsUsing[i].GetSceneChance();
                if (counter > rng)
                {
                    return levelsUsing[i];
                }
            }
        }
        return levelsUsing[0];
    }


    public void SwitchLevel()
    {
        if (levelCount >= maxLevels)
        {
            SceneManager.LoadScene(homeWorld);
            levelCount = 0;
            nextScene = "";
        }
        else
        {
            if (nextScene == "")
            {
                SceneManager.LoadScene(GetNextLevel().GetSceneName());
            }
            else
            {
                //Debug.Log("Fast switch");
                Scene scene = SceneManager.GetSceneByName(nextScene);
                if (!scene.IsValid())
                {
                    SceneManager.LoadScene(GetNextLevel().GetSceneName());
                }
                else
                {
                    SceneManager.SetActiveScene(scene);
                }
            }
            levelCount++;
            Debug.Log("Next room is #: " + levelCount);
            /*string sceneName = GetNextLevel().GetSceneName();
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
            nextScene = sceneName;*/
            StartCoroutine(LoadAsync(GetNextLevel().GetSceneName()));
            //Debug.Log("Scene Loaded: " + SceneManager.GetSceneByName(nextScene).isLoaded);
        }
    }

    IEnumerator LoadAsync(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        if (asyncLoad.isDone)
        {
            //Debug.Log("Done Loading");
            nextScene = sceneName;
            GetComponent<EnemySpawning>().StartLevel(nextScene);
            //Debug.Log("Scene Loaded: " + SceneManager.GetSceneByName(nextScene).isLoaded);
        }
    }

    public void ResetLevelCount(DamageContext damageContext)
    {
        if (damageContext.victim.CompareTag("Player"))
        {
            //SceneManager.UnloadSceneAsync(nextScene);
            levelCount = 0;
        }
    }
    public string GetHomeWorld()
    {
        return homeWorld;
    }

    public int GetMaxLevels()
    {
        return this.maxLevels;
    }

    public void SetMaxLevels(int maxLevels)
    {
        this.maxLevels = maxLevels;
    }
}
