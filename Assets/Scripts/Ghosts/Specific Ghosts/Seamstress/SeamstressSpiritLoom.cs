using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.PackageManager;
using UnityEngine;

/// <summary>
///   // TODO: stop if  any other action other than jump/move occurs
///   // current: instant wind-up and recovery time
/// </summary>
public class SeamstressBasicSpiritLoom : MonoBehaviour
{

    private static int MAX_SPOOLS = 4;
    private static float SPOOL_GENERATION_INITIAL_BUFFER_TIME = 2.0f;
    private static float SPOOL_GENERATION_SUBSEQUENT_BUFFER_TIME = 1.0f;
    private static float HEAVY_ATTACK_STUN_TIME = 1.0f;
    private static int HEAVY_ATTACK_SPOOL_COST = 1;

    private PlayerStateMachine playerStateMachine;

    private float timer = 0.0f;
    private int curr_spools = 0; // 0 to num spools


    // Start is called before the first frame update
    void Start()
    {
        playerStateMachine = GetComponent<PlayerStateMachine>();
    }

    // Update is called once per frame
    void Update()
    {
        // timer has started
        if (timer > 0.0f)
        {

            timer -= Time.deltaTime;

            if (timer <= 0.0f && curr_spools < MAX_SPOOLS)
            {
                if (curr_spools < MAX_SPOOLS) {
                    curr_spools++;
                    Debug.Log("added " + curr_spools);
                }
                else {
                    Debug.Log("max spools reached");
                }
                timer = SPOOL_GENERATION_SUBSEQUENT_BUFFER_TIME;

            }

            // Cancel if not on ground or S is released
            if (!SpiritLoomEligible()) {
                StopSpiritLoom();
                Debug.Log("We canceled " + curr_spools);

            }

        }
        // timer has not started
        else if (SpiritLoomEligible())
        {
            timer = SPOOL_GENERATION_INITIAL_BUFFER_TIME;
            Debug.Log("We started " + timer + " spools " + curr_spools);
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
        timer = SPOOL_GENERATION_INITIAL_BUFFER_TIME;
    }
    
    public void StopSpiritLoom()
    {
        curr_spools = 0;
        timer = 0;

    }

    /// <summary>
    /// This function gets called by message from animator when player enters heavy attack state
    /// </summary>

    public void StartHeavyAttack()
    {
        if (curr_spools > 0) { 
        
            curr_spools -= HEAVY_ATTACK_SPOOL_COST;
            Debug.Log(curr_spools);

            // TODO enemies hit by attack will be stunned


        }
    }

    /// <summary>
    /// This function gets called by a message from the animator when you exit the Heavy Attack State
    /// </summary>
    public void StopHeavyAttack()
    {

    }

    

    
}
