using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Level
{
    [SerializeField] string sceneName;
    [SerializeField] float chance;

    public Level(string sceneName, float chance)
    {
        this.sceneName = sceneName;
        this.chance = chance;
    }

    public string GetSceneName()
    {
        return sceneName;
    }

    public float GetSceneChance()
    {
        return chance;
    }
}
