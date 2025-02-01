using System;
using System.Collections;
using System.Threading;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

//<summary>
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
    [SerializeField] private bool canDash = true;
    [SerializeField] private bool isDashing = false;
    [SerializeField] private bool isSlowing = false;

    private StatManager stats;

    private void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<StatManager>();
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            rb.velocity = velocity;
        }
    }

    //<summary>
    // Function called through the animation state machine when player is meant to "Dash"
    // Calculates the displacement vector between the player and the mouse and starts the dash
    //</summary>
    public void StartDash()
    {
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 displacement = Vector2.ClampMagnitude((Vector2)mousePos - (Vector2)transform.position, stats.ComputeValue("Max Dash Distance"));

        RaycastHit2D hit = Physics2D.Raycast(transform.position, displacement.normalized, displacement.magnitude, LayerMask.GetMask("Ground"));
        if (hit.collider != null)
        {
            displacement = hit.point - (Vector2)transform.position - displacement.normalized * rb.GetComponent<Collider2D>().bounds.extents.magnitude;
        }
        this.velocity = displacement / stats.ComputeValue("Dash Time");
        StartCoroutine(DashCoroutine());
    }

    private IEnumerator DashCoroutine()
    {
        isDashing = true;
        PlayerStateMachine psm = this.GetComponent<PlayerStateMachine>();
        
        Debug.Log("Starting wait: " + stats.ComputeValue("Dash Time"));
        yield return new WaitForSeconds(stats.ComputeValue("Dash Time"));
        Debug.Log("Done waiting: " + stats.ComputeValue("Dash Time"));

        rb.velocity *= stats.ComputeValue("Post Dash Momentum Fraction");
        psm.EnableTrigger("OPT");
        psm.OnCooldown("c_special");

        isDashing = false;
        yield return new WaitForSeconds(stats.ComputeValue("Dash Cooldown"));
        psm.OffCooldown("c_special");
    }

    public StatManager.Stat[] GetStatList()
    {
        return statList;
    }
}
