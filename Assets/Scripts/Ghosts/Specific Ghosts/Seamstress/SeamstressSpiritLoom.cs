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

    [SerializeField]
    public StatManager.Stat[] statList;

    private StatManager stats;

    [SerializeField]
    DamageContext damageContext;
    [SerializeField] 
    float offsetX;
    [SerializeField]
    GameObject indicator;

    private PlayerStateMachine playerStateMachine;
    private Camera mainCamera;

    private float timer = 0.0f;
    private int curr_spools = 0; // 0 to num spools


    // Start is called before the first frame update
    void Start()
    {
        indicator.SetActive(false);
        playerStateMachine = GetComponent<PlayerStateMachine>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        stats = this.GetComponent<StatManager>();
    }

    // Update is called once per frame
    void Update()
    {

        Debug.Log("SPOOLS " + curr_spools + " time " + timer);

        // timer has started
        if (timer > 0.0f)
        {

            timer -= Time.deltaTime;

            if (timer <= 0.0f)
            {
                if (curr_spools < stats.ComputeValue("Maximum Spools"))
                {
                    curr_spools++;

                }

                timer = stats.ComputeValue("Spool Generation Subsequent Buffer Time");
            }

            // Cancel if not on ground or S is released
            if (!SpiritLoomEligible())
            {
                StopSpiritLoom();
                indicator.SetActive(false);


            }

        }
        // timer has not started
        else if (SpiritLoomEligible())
        {
            StartSpiritLoom();
        }

        // Copied from HeavyAttack
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        if (mousePos.x < transform.position.x)
        {
            indicator.transform.localPosition = new Vector3(-offsetX, indicator.transform.localPosition.y, 0);
        }
        else
        {
            indicator.transform.localPosition = new Vector3(offsetX, indicator.transform.localPosition.y, 0);
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
        indicator.SetActive(true);
        if (curr_spools > 0) { 
         
            curr_spools -= (int) stats.ComputeValue("Heavy Attack Spool Cost");
            Debug.Log(curr_spools);

            RaycastHit2D[] hits = Physics2D.BoxCastAll(indicator.transform.position, indicator.transform.localScale, 0, new Vector2(0, 0));
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.transform.gameObject.tag == "Enemy")
                {
                    Debug.Log("we have stunned");
                    EnemyStateManager esm = hit.transform.gameObject.GetComponent<EnemyStateManager>();
                    esm.Stun(damageContext, stats.ComputeValue("Heavy Attack Stun Time"));
                }
            }

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
