using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSwitching : MonoBehaviour
{
    [SerializeField] Level[] levels;
    [SerializeField] SpecificLevelPool[] specificLevels;

    private string nextScene = "";

    private int levelCount = 0;

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
        if (nextScene == "")
        {
            SceneManager.LoadScene(GetNextLevel().GetSceneName());
        }
        else
        {
            Debug.Log("Fast switch");
            Debug.Log("set active: " + SceneManager.SetActiveScene(SceneManager.GetSceneByName(nextScene)));
            // SceneManager.LoadScene(nextScene);
        }
        levelCount++;
        string sceneName = GetNextLevel().GetSceneName();
        /*SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        nextScene = sceneName;*/
        StartCoroutine(LoadAsync(GetNextLevel().GetSceneName()));
    }

    IEnumerator LoadAsync(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        Debug.Log("Done Loading");
        nextScene = sceneName;
    }

    public void ResetLevelCount(DamageContext damageContext)
    {
        if (damageContext.victim == PlayerID.instance)
        {
            levelCount = 0;
        }
    }
}
