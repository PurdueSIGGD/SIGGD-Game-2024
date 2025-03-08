using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class SeamstressBasicSpiritLoom : MonoBehaviour
{

    private static int MAX_SPOOLS = 4;
    private static float SPOOL_GENERATION_INITIAL_BUFFER_TIME = 1.0f;
    private static float SPOOL_GENERATION_SUBSEQUENT_BUFFER_TIME 0.5f;
    private static float HEAVY_ATTACK_STUN_TIME = 1.0f;
    private static int HEAVY_ATTACK_SPOOL_COST = 10.0f;

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
        if (timer > 0.0f)
        {

            timer -= Time.deltaTime;

            if (timer < 0.0f && curr_spools < MAX_SPOOLS)
            {
                curr_spools++;
                Debug.Log(curr_spools);
                timer = SPOOL_GENERATION_SUBSEQUENT_BUFFER_TIME;
                // TODO timer is up
                // playerStateMachine.OffCooldown("c_specialNO");
            }

            // Cancel if not on ground TODO or S is released
            // or any other action other than jump/move occurs
            


        }




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
