using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpiritBuff : MonoBehaviour, IPossession
{
    [SerializeField]
    private StatBuff[] buffs;
    
    [Serializable]
    public struct StatBuff {
        public string name;
        public int buff;
    }
    
    [SerializeField]
    bool possessed;

    public void DeSelect (GameObject player) {
         Stats stats = player.GetComponent<Stats>();
        foreach (StatBuff statBuff in buffs){
            stats.ModifyStat(statBuff.name, -1 * statBuff.buff);
        }
    }
    public void Select (GameObject player) {
        Stats stats = player.GetComponent<Stats>();
        foreach (StatBuff statBuff in buffs){
            stats.ModifyStat(statBuff.name, statBuff.buff);
        }
    }
    public void SwitchPossessionStatus(GameObject player) {
         if (possessed) {
            DeSelect(player);
        } else {
              Select(player);
        }
        possessed = !possessed;
    }
}

