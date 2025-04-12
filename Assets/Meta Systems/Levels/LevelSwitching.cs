using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSwitching : MonoBehaviour
{
    [SerializeField] Level[] levels;
    [SerializeField] SpecificLevelPool[] specificLevels;

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
            totalChance += levelsUsing[i].GetSceneChance();
        }

        float rng = Random.Range(0, totalChance);
        float counter = 0.0f;
        for (int i = 0; i < levelsUsing.Length; i++)
        {
            counter += levelsUsing[i].GetSceneChance();
            if (counter > rng)
            {
                return levelsUsing[i];
            }
        }
        return null;
    }


    public void SwitchLevel()
    {
        SceneManager.LoadScene(GetNextLevel().GetSceneName());
    }

    public void ResetLevelCount(DamageContext damageContext)
    {
        if (damageContext.victim == PlayerID.instance)
        {
            levelCount = 0;
        }
    }
}
