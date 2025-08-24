using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Combo
{
    [SerializeField] public List<string> actionList = new List<string>();
    [SerializeField] public int index = 0;
    public Combo(Combo combo)
    {
        this.actionList = combo.actionList;
        this.index = combo.index;
    }
    public string NextActionName()
    {
        if (index >= actionList.Count)
            return null;
        string result = actionList[index];
        index++;
        return result;
    }
}
