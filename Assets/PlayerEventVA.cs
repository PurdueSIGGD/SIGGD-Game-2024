using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEventVA : MonoBehaviour
{
    void OnEnable()
    {
        GameplayEventHolder.OnDeath += PlayOnKillVA;
    }

    private void OnDisable()
    {
        GameplayEventHolder.OnDeath -= PlayOnKillVA;
    }

    private void PlayOnKillVA(DamageContext context)
    {
        // if player killed entity
        if (context.attacker != null && context.attacker.CompareTag("Player"))
        {
            AudioManager.Instance.VABranch.PlayVATrack(PartyManager.instance.selectedGhost + " On Kill");
        }
    }
}
