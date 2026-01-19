using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class YumeHeavy : MonoBehaviour
{
    private bool weaving = false;
    public int concurrentSpools = 0;

    public bool isWavePrimed = false;

    public SeamstressManager manager;

    void OnEnable()
    {
        //GameplayEventHolder.OnDamageDealt += OnPlayerDamage;
    }

    void OnDisable()
    {
        //GameplayEventHolder.OnDamageDealt -= OnPlayerDamage;
    }

    // Update is called once per frame
    void Update()
    {
        if (weaving && manager.GetSpools() < manager.GetStats().ComputeValue("Max Spools"))
        {
            manager.SetWeaveTimer(manager.GetWeaveTimer() + Time.deltaTime);
        }
        else
        {
            manager.SetWeaveTimer(0);
            concurrentSpools = 0;
        }

        if ((concurrentSpools == 0 && manager.GetWeaveTimer() > manager.GetStats().ComputeValue("Initial Spool Buffer")) || 
            (concurrentSpools > 0 && manager.GetWeaveTimer() > manager.GetStats().ComputeValue("Concurrent Spool Buffer")))
        {
            manager.AddSpools(1);
            concurrentSpools++;
            Debug.Log("spool Gain " + manager.GetSpools());
            manager.SetWeaveTimer(0);
            if (manager.GetSpools() >= manager.GetStats().ComputeValue("Max Spools"))
            {
                AudioManager.Instance.VABranch.PlayVATrack("Yume-Seamstress Spirit Loom Max Spools");
                AudioManager.Instance.SFXBranch.PlaySFXTrack("Yume-Max Spools");
                GetComponent<PlayerStateMachine>().EnableTrigger("OPT");
            }
            else
            {
                AudioManager.Instance.SFXBranch.GetSFXTrack("Yume-Gained Spool").SetPitch(manager.GetSpools(), manager.GetStats().ComputeValue("Max Spools"));
                AudioManager.Instance.SFXBranch.PlaySFXTrack("Yume-Gained Spool");
            }
        }
    }

    void StartCrouch()
    {
        GetComponent<Move>().PlayerStop();
        GetComponent<PartyManager>().SetSwappingEnabled(false);
        weaving = true;

        AudioManager.Instance.VABranch.PlayVATrack("Yume-Seamstress Spirit Loom Activation");
    }

    void StopCrouch()
    {
        GetComponent<Move>().PlayerGo();
        GetComponent<PartyManager>().SetSwappingEnabled(true);
        weaving = false;
        Debug.Log("Exiting Yume");
    }

    private int GetSpoolManager()
    {
        return manager.GetSpools();
    }

    /*
    void OnPlayerDamage(DamageContext damageContext)
    {
        Debug.Log("attacker correct: " + (damageContext.attacker == PlayerID.instance.gameObject));
        Debug.Log("actionID correct: " + (damageContext.actionID == ActionID.PLAYER_HEAVY_ATTACK));
        Debug.Log("manager nullcheck: " + manager == null);
        Debug.Log("manager: " + GetSpoolManager());
        Debug.Log("actionID: " + damageContext.actionID);
        if (damageContext.attacker == gameObject && 
            damageContext.actionID == ActionID.PLAYER_HEAVY_ATTACK && 
            manager.GetSpools() >= manager.GetStats().ComputeValue("Heavy Attack Spools Needed"))
        {
            damageContext.victim.GetComponent<EnemyStateManager>().Stun(damageContext, manager.GetStats().ComputeValue("Spool Heavy Attack Stun"));
            Debug.Log("Subtracted " + -(int)manager.GetStats().ComputeValue("Heavy Attack Spools Needed") + " Spools");
            manager.AddSpools(-(int)manager.GetStats().ComputeValue("Heavy Attack Spools Needed"));
        }
    }
    */

    public void PrimeStunWave()
    {
        if (manager.GetSpools() <= 0) return;
        //manager.AddSpools(-(int)manager.GetStats().ComputeValue("Heavy Attack Spools Needed"));
        isWavePrimed = true;
    }

    public void TriggerStunWave(List<GameObject> enemiesHit)
    {
        if (!isWavePrimed) return;

        manager.AddSpools(-(int)manager.GetStats().ComputeValue("Heavy Attack Spools Needed"));
        Vector3 placementOffset = new Vector3(transform.position.x + (manager.GetStats().ComputeValue("Spool Heavy Attack Placement Distance") * Mathf.Sign(gameObject.transform.rotation.y)),
                                              transform.position.y,
                                              transform.position.z);
        GameObject stunWave = Instantiate(manager.heavyWave, placementOffset, transform.rotation);
        stunWave.GetComponent<YumeStunWave>().StartWave(manager.GetStats().ComputeValue("Spool Heavy Attack Speed"),
                                                        manager.GetStats().ComputeValue("Spool Heavy Attack Duration"),
                                                        manager.GetStats().ComputeValue("Spool Heavy Attack Stun"),
                                                        manager.GetStats().ComputeValue("Spool Heavy Attack Damage"),
                                                        enemiesHit);
        isWavePrimed = false;
    }
}
