using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GhostBuff : MonoBehaviour
{
    [SerializeField]
    StatToBuff[] buffs;

    Stat[] statsList;
    
    [Serializable]
    public struct StatToBuff {
        public string name;
        public int buff;
    }
    
    bool inParty;


    public void EnterParty(GameObject player)  {
        Stat statToModify = statsList[0];
        for (int i = 0; i < statsList.Length; i++) {
            foreach (StatToBuff statBuff in buffs){
                if (statsList[i].GetName().Equals(statBuff.name)) {
                    statsList[i].ModifyStat(statBuff.buff);
                }
            }
        }
    }
    void Start() {
        statsList = GameObject.FindGameObjectWithTag("Player")
.GetComponents<Stat>();
    }
    public void ExitParty(GameObject player) {
        Stat statToModify = statsList[0];
        for (int i = 0; i < statsList.Length; i++) {
            foreach (StatToBuff statBuff in buffs){
                if (statsList[i].GetName().Equals(statBuff.name)) {
                    statsList[i].ModifyStat(-1 * statBuff.buff);
                }
            }
        }
    }

    // TEMPORARY - SHOULD DELETE LATER
    public void SwitchPartyStatus(GameObject player) {
        if (inParty) {
            ExitParty(player);
        } else {
            EnterParty(player);
        }
        inParty = !inParty;
    }
}
