using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStatList
{
    /// <summary>
    /// Returns an array of stats under this script
    /// </summary>
    /// <returns> array of stats </returns>
    public StatManager.Stat[] GetStatList();
}
