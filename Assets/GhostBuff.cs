using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GhostBuff : MonoBehaviour
{

    [SerializeField]
    int buff;
    [SerializeField]
    String statName;
    Stat[] statsList;
    
    
    bool inParty;


    public void EnterParty(GameObject player)  {
        Stat statToModify = statsList[0];
        for (int i = 0; i < statsList.Length; i++) {
            if (statsList[i].GetName().Equals(statName)) {
                statToModify = statsList[i];
            }
        }
        statToModify.ModifyStat(buff);
    }
    void Start() {
        statsList = GameObject.FindGameObjectWithTag("Player")
.GetComponents<Stat>();
    }
    public void ExitParty(GameObject player) {
        Stat statToModify = statsList[0];
        for (int i = 0; i < statsList.Length; i++) {
            if (statsList[i].GetName().Equals(statName)) {
                statToModify = statsList[i];
            }
        }
        statToModify.ModifyStat(-1 * buff);
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
