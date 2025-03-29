using System;
using System.Collections;
using System.Threading;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

//<summary>
// Special ability script for player - if a ghost is currently posessing and 
// overrides the special move delegate, that delegate will run. Else, the player
// dash function will run like normal.
// Base player action: player dashes towards the mouse location in a fixed time
//</summary>
[DisallowMultipleComponent]
public class Dash : MonoBehaviour, IStatList
{
    [SerializeField]
    private StatManager.Stat[] statList;

    
    private Camera mainCamera;
    private Rigidbody2D rb;

    [SerializeField] private Vector2 velocity = Vector2.zero;
    [SerializeField] private bool isDashing = false;

    private StatManager stats;
    private OrionManager orionManager;
    private PlayerStateMachine psm;

    [Header("Delegate Override Variables")]
    public SpecialAction specialAction;
    public delegate void SpecialAction(); // delegate to contain any ghost overrides

    private void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<StatManager>();
        orionManager = GetComponent<OrionManager>();
        psm = GetComponent<PlayerStateMachine>();
        specialAction = null;
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            rb.velocity = velocity;
        }
        if (orionManager != null)
        {
            if (orionManager.getSpecialCooldown() > 0)
            {
                psm.OnCooldown("c_special");
            }
            else
            {
                psm.OffCooldown("c_special");
            }
        }
    }

    //<summary>
    // Function called through the animation state machine when player is meant to "Dash"
    // Calculates the displacement vector between the player and the mouse and starts the dash
    // Can be overriden by any posesssing ghost's start special ability function.
    //</summary>
    public void StartDash()
    {
        if (specialAction != null)
        {
            specialAction();
        }
        else
        {
            Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            //Vector2 displacement = Vector2.ClampMagnitude((Vector2)mousePos - (Vector2)transform.position, stats.ComputeValue("Max Dash Distance"));

            Vector2 displacement = ((Vector2)mousePos - (Vector2)transform.position).normalized * stats.ComputeValue("Max Dash Distance");
            
            /*
            RaycastHit2D hit = Physics2D.Raycast(transform.position, displacement.normalized, displacement.magnitude, LayerMask.GetMask("Ground"));
            if (hit.collider != null)
            {
                displacement = hit.point - (Vector2)transform.position - displacement.normalized * rb.GetComponent<Collider2D>().bounds.extents.magnitude;
            }
            displacement = (displacement.magnitude < 5f) ? displacement.normalized * 5f : displacement;
            */

            this.velocity = displacement / stats.ComputeValue("Dash Time");
            StartCoroutine(DashCoroutine());
        }
    }

    private IEnumerator DashCoroutine()
    {
        isDashing = true;
        PlayerStateMachine psm = this.GetComponent<PlayerStateMachine>();

        yield return new WaitForSeconds(stats.ComputeValue("Dash Time"));

        rb.velocity *= stats.ComputeValue("Post Dash Momentum Fraction");
        psm.EnableTrigger("OPT");
        //psm.OnCooldown("c_special");

        isDashing = false;
        //yield return new WaitForSeconds(stats.ComputeValue("Dash Cooldown"));
        //psm.OffCooldown("c_special");
        orionManager.setSpecialCooldown(stats.ComputeValue("Dash Cooldown"));
        //psm.EnableTrigger("OPT");
    }

    public StatManager.Stat[] GetStatList()
    {
        return statList;
    }
}
