using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpecificLevelPool
{
    [SerializeField] Level[] levels;
    [SerializeField] int levelNum;

    public SpecificLevelPool(Level[] levels, int levelNum)
    {
        this.levels = levels;
        this.levelNum = levelNum;
    }

    public int GetLevelNum()
    {
        return levelNum;
    }

    public void SetLevelNum(int levelNum)
    {
        this.levelNum = levelNum;
    }

    public Level[] GetLevels()
    {
        return levels;
    }
}
