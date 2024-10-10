using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBuf : MonoBehaviour
{

    [SerializeField]
    int buff;

    bool inParty;

    public void EnterParty(GameObject player)  {
        Stat stat = player.GetComponent<Stat>();
        stat.ModifyStat(buff);
    }

    public void ExitParty(GameObject player) {
        Stat stat = player.GetComponent<Stat>();
        stat.ModifyStat(-1 * buff);
    }

    // TEMPORARY - SHOULD DELETE LATER
    public void SwithPartyStatus(GameObject player) {
        if (inParty) {
            ExitParty(player);
        } else {
            EnterParty(player);
        }
        inParty = !inParty;
    }
}
