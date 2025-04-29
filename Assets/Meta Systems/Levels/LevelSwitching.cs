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

    public float GetProgress()
    {
        return ((float)levelCount - 1.0f) / (float)maxLevels;
    }

    private void Start()
    {
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
        return null;
    }


    public void SwitchLevel()
    {
        if(levelCount >= maxLevels)
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
                //Debug.Log("Fast Scene Loaded: " + scene.isLoaded);
                //Debug.Log("set active: " + SceneManager.SetActiveScene(scene));
                SceneManager.SetActiveScene(scene);
                // SceneManager.LoadScene(nextScene);
            }
            levelCount++;
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
            GetComponent<EnemySpawning>().StartLevel();
            //Debug.Log("Scene Loaded: " + SceneManager.GetSceneByName(nextScene).isLoaded);
        }
    }

    public void ResetLevelCount(DamageContext damageContext)
    {
        if (damageContext.victim == PlayerID.instance)
        {
            levelCount = 0;
        }
    }
}
