using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static GhostBuff;

public class Stats : MonoBehaviour
{
    [SerializeField]
    private Stat[] stats;

    [Serializable]
    public struct Stat {
        public string name;
        public float value;
        public int modifier;
    }

    private bool EmptyCheck()
    {
        if (stats.Length == 0)
        {
            Debug.LogWarning("no stats to modify or compute value of");
            return true;
        }
        return false;
    }

    public void ModifyStat(string statName, int statBuff) {
        if (EmptyCheck()) return;

        for (int i = 0; i < stats.Length; i++) {
            if (stats[i].name.Equals(statName)) {
                stats[i].modifier += statBuff;
                return;
            }
        }
        Debug.LogError(String.Format("'{0}' stat not found", statName));
    }

    public void ModifyStat(int statIndex, int statBuff)
    {
        if (EmptyCheck()) return;

        if (statIndex <= stats.Length-1)
        {
            stats[statIndex].modifier += statBuff;
            return;
        }
        Debug.LogError(String.Format("'{0}' stat index out of bounds", statIndex));
    }

    public float ComputeValue(string statName) {
        if (EmptyCheck()) return -1;

        for (int i = 0; i < stats.Length; i++) {
            if (stats[i].name.Equals(statName)) {
                return stats[i].value * (stats[i].modifier / 100f);
            }
        }
        Debug.LogError(String.Format("'{0}' stat not found", statName));
        return -1;
    }

    public float ComputeValue(int statIndex) {
        if (EmptyCheck()) return -1;
        
        if (statIndex <= stats.Length - 1)
        {
            return stats[statIndex].value * (stats[statIndex].modifier / 100f);
        }
        Debug.LogError(String.Format("'{0}' stat index out of bounds", statIndex));
        return -1;
    }

    public int GetStatIndex(string statName) {
        if (EmptyCheck()) return -1;

        for (int i = 0; i < stats.Length; i++) {
            if (stats[i].name.Equals(statName)) {
                return i;
            }
        }
        return -1;
    }
    
}
