using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script is only used to clear specific values on entering hub world
/// </summary>
public class EnterHub : MonoBehaviour
{
    void Awake()
    {
        SaveManager.data.eva.tempoCount = 0;
        SaveManager.data.eva.remainingTempoDuration = 0f;
        SaveManager.data.yume.spoolCount = 0;
        SaveManager.data.aegis.damageDealtTillSmite = 0.0f;
        SaveManager.data.aegis.damageBlockTillSmite = 0.0f;
    }
}
