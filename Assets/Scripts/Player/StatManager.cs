using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static GhostBuff;

public class StatManager : MonoBehaviour, IStatList
{
    [SerializeField]
    private Stat[] statList;

    private Dictionary<string, Stat> statMap = new Dictionary<string, Stat>();

    [Serializable]
    public struct Stat {
        public string name;
        public float value;
        public int modifier;
    }

    private void Start()
    {
        IStatList[] scripts = this.GetComponents<IStatList>();
        foreach (IStatList script in scripts)
        {
            foreach (Stat stat in script.GetStatList())
            {
                try
                {
                    statMap.Add(stat.name, stat);
                }
                catch (ArgumentException)
                {
                    Debug.LogWarning(String.Format("'{0}' stat already exists", stat.name));
                }
            }
        }
    }

    /// <summary>
    /// Alters the modifier of the stat, affecting its current value
    /// </summary>
    /// <param name="statName"> name of stat to modify </param>
    /// <param name="delta"> amount to alter stat modifer by </param>
    public void ModifyStat(string statName, int delta) {
        if (statMap.TryGetValue(statName, out Stat stat))
        {
            stat.modifier += delta;
        }
        else
        {
            Debug.LogError(String.Format("'{0}' stat not found", statName));
        }
    }

    /// <summary>
    /// Returns current computed value of a stat
    /// </summary>
    /// <param name="statName"> name of stat to compute </param>
    /// <returns></returns>
    public float ComputeValue(string statName) {
        if (statMap.TryGetValue(statName, out Stat stat))
        {
            return stat.value * (stat.modifier / 100f);
        } else
        {
            Debug.LogError(String.Format("'{0}' stat not found", statName));
            return Mathf.NegativeInfinity;
        }
    }

    public Stat[] GetStatList()
    {
        return statList;
    }
}
