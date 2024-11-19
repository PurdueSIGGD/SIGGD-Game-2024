using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GhostBuff : MonoBehaviour, IParty
{
    [SerializeField]
    private StatBuff[] buffs;
    
    [Serializable]
    public struct StatBuff {
        public string name;
        public int buff;
    }
    
    bool inParty;
    [SerializeField]

    public void EnterParty(GameObject player)  {

        Stats stats = player.GetComponent<Stats>();
        foreach (StatBuff statBuff in buffs){
            stats.ModifyStat(statBuff.name, statBuff.buff);
        }
    }

    public void ExitParty(GameObject player) {
        Stats stats = player.GetComponent<Stats>();
        foreach (StatBuff statBuff in buffs){
            stats.ModifyStat(statBuff.name, -1 * statBuff.buff);
        }
    }

    public void SwitchPartyStatus(GameObject player) {
        if (inParty) {
            ExitParty(player);
        } else {
            EnterParty(player);
        }
        inParty = !inParty;
    }
}

