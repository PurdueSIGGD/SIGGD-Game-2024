using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpecificLevelPool
{
    [SerializeField] Level[] levels;
    [SerializeField] int levelNum;

    public int GetLevelNum()
    {
        return levelNum;
    }

    public Level[] GetLevels()
    {
        return levels;
    }
}
