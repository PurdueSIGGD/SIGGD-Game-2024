using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Level : MonoBehaviour
{

    [SerializeField] string sceneName;
    [SerializeField] int sceneNumber;
    [SerializeField] float chance;

    public string GetSceneName()
    {
        return sceneName;
    }

    public int GetSceneNumber()
    {
        return sceneNumber;
    }

    public float GetSceneChance()
    {
        return chance;
    }
}
