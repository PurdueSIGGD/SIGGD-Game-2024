using System;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class StatManager : MonoBehaviour, IStatList
{

    [SerializeField]
    private Stat[] statList;

    private Dictionary<string, Stat> statMap = new Dictionary<string, Stat>();

    [Serializable]
    public struct Stat
    {
        public string name;
        public float value;
        [NonSerialized] public int modifier;
    }

    private void Awake()
    {
        IStatList[] scripts = this.GetComponents<IStatList>();
        foreach (IStatList script in scripts)
        {
            Stat[] list = script.GetStatList();
            for (int i = 0; i < list.Length; i++)
            {
                Stat stat = list[i];
                try
                {
                    stat.modifier = 100;
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
    public void ModifyStat(string statName, int delta)
    {
        if (statMap.TryGetValue(statName, out Stat stat))
        {
            stat.modifier += delta;
            statMap[statName] = stat;
            //Debug.Log(statName + ": " + stat.modifier);
        }
        else
        {
            Debug.LogError(String.Format("'{0}' stat not found", statName));
        }
    }

    /// <summary>
    /// Set the modifier of the stat, affecting its current value
    /// </summary>
    /// <param name="statName"> name of stat to modify </param>
    /// <param name="modifier"> amount to alter stat modifer by </param>
    public void SetStat(string statName, int modifier)
    {
        if (statMap.TryGetValue(statName, out Stat stat))
        {
            stat.modifier = modifier;
            statMap[statName] = stat;
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
    public float ComputeValue(string statName)
    {
        if (statMap.TryGetValue(statName, out Stat stat))
        {
            return stat.value * (stat.modifier / 100f);
        }
        else
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
