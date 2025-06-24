using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class YumePassive : MonoBehaviour, IStatList
{
    [SerializeField]
    private StatManager.Stat[] statList;

    private bool crouching = false;
    private float timer = 0.0f;
    private int concurrentSpools = 0;

    public SeamstressManager manager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void StartHeavyCheck()
    {
        GameplayEventHolder.OnDamageDealt += OnPlayerDamage;
        Debug.Log("Added");
    }

    // Update is called once per frame
    void Update()
    {
        if (crouching && manager.GetSpools() < manager.GetStats().ComputeValue("Max Spools")) {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0;
            concurrentSpools = 0;
        }

        if ((concurrentSpools == 0 && timer > manager.GetStats().ComputeValue("Initial Spool Buffer")) || (concurrentSpools > 0 && timer > GetComponent<StatManager>().ComputeValue("Concurrent Spool Buffer")))
        {
            manager.AddSpools(1);
            concurrentSpools++;
            Debug.Log("spool Gain " + manager.GetSpools());
            timer = 0.0f;
        }
    }

    void StartCrouch()
    {
        GetComponent<Move>().PlayerStop();
        crouching = true;
    }

    void StopCrouch()
    {
        GetComponent<Move>().PlayerGo();
        crouching = false;
        Debug.Log("Exiting Yume");
    }

    public StatManager.Stat[] GetStatList()
    {
        return statList;
    }

    private int GetSpoolManager()
    {
        return manager.GetSpools();
    }

    void OnPlayerDamage(DamageContext damageContext)
    {
        Debug.Log("attacker correct: " + (damageContext.attacker == PlayerID.instance.gameObject));
        Debug.Log("actionID correct: " + (damageContext.actionID == ActionID.PLAYER_HEAVY_ATTACK));
        Debug.Log("manager nullcheck: " + manager == null);
        Debug.Log("manager: " + GetSpoolManager());
        Debug.Log("actionID: " + damageContext.actionID);
        if(damageContext.attacker == PlayerID.instance.gameObject && damageContext.actionID == ActionID.PLAYER_HEAVY_ATTACK && manager.GetSpools() >= GetComponent<StatManager>().ComputeValue("Heavy Attack Spools Needed"))
        {
            damageContext.victim.GetComponent<EnemyStateManager>().Stun(damageContext, GetComponent<StatManager>().ComputeValue("Spool Heavy Attack Stun"));
            Debug.Log("Subtracted " + -(int)GetComponent<StatManager>().ComputeValue("Heavy Attack Spools Needed") + " Spools");
            manager.AddSpools(-(int)GetComponent<StatManager>().ComputeValue("Heavy Attack Spools Needed"));
        }
    }
}
