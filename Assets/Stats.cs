using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    [SerializeField]
    Stat[] stats;

    [Serializable]
    public struct Stat {
        public string name;
        public int value;
        public int modifier;
    }


    public void ModifyStat(string statName, int statBuff) {
        for (int i = 0; i < stats.Length; i++) {
            if (stats[i].name.Equals(statName)) {
                stats[i].modifier += statBuff;
                break;
            }
        }
    }

    public float ComputeValue(string statName) {
        for (int i = 0; i < stats.Length; i++) {
            if (stats[i].name.Equals(statName)) {
                return stats[i].value * (stats[i].modifier / 100f);
            }
        }
        return -1;
    }

    public float ComputeValue(int statIndex) {
        return stats[statIndex].value * (stats[statIndex].modifier / 100f);
    }

    public int GetStatIndex(string statName) {
        for (int i = 0; i < stats.Length; i++) {
            if (stats[i].name.Equals(statName)) {
                return i;
            }
        }
        return -1;
    }
    
}
