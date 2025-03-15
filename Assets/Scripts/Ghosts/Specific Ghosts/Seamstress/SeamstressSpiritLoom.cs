using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.PackageManager;
using UnityEngine;

/// <summary>
///   // TODO: stop if  any other action other than jump/move occurs
///   // current: instant wind-up and recovery time
/// </summary>
public class SeamstressBasicSpiritLoom : MonoBehaviour, IStatList
{

    [SerializeField] public StatManager.Stat[] statList;

    public StatManager stats;

    [SerializeField]
    DamageContext damageContext;

    private PlayerStateMachine playerStateMachine;
    private EnemyStateManager enemyStateManager;

    private float timer = 0.0f;
    private int curr_spools = 0; // 0 to num spools


    // Start is called before the first frame update
    void Start()
    {
        playerStateMachine = GetComponent<PlayerStateMachine>();
        enemyStateManager = GetComponent<EnemyStateManager>();
    }

    // Update is called once per frame
    void Update()
    {

        // timer has started
        if (timer > 0.0f)
        {

            timer -= Time.deltaTime;

            if (timer <= 0.0f)
            {
                if (curr_spools < stats.ComputeValue("Max Spools"))
                {
                    curr_spools++;
                }

                timer = stats.ComputeValue("Spool Generation Subsequent Buffer Time");
            }

            // Cancel if not on ground or S is released
            if (!SpiritLoomEligible())
            {
                StopSpiritLoom();

            }

        }
        // timer has not started
        else if (SpiritLoomEligible())
        {
            StartSpiritLoom();
        }
    }

    private bool SpiritLoomEligible()
    {
        Animator anim = playerStateMachine.GetComponent<Animator>();

        return anim.GetBool("i_down") &&
               anim.GetBool("p_grounded");
    }

    public void StartSpiritLoom()
    {
        timer = stats.ComputeValue("Spool Generation Initial Buffer Time");
    }
    
    public void StopSpiritLoom()
    {
        curr_spools = 0;
        timer = 0.0f;

    }

    /// <summary>
    /// This function gets called by message from animator when player enters heavy attack state
    /// </summary>

    public void StartHeavyAttack()
    {
        if (curr_spools > 0) { 
        
            curr_spools -= (int) stats.ComputeValue("Heavy Attack Spool Cost");
            Debug.Log(curr_spools);

            enemyStateManager.Stun(damageContext, stats.ComputeValue("Heavy Attack Stun Time"));


        }
    }

    /// <summary>
    /// This function gets called by a message from the animator when you exit the Heavy Attack State
    /// </summary>
    public void StopHeavyAttack()
    {

    }

    public StatManager.Stat[] GetStatList()
    {
        return statList;
    }
}
