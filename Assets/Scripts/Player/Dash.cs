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
public class Dash : MonoBehaviour
{
    [SerializeField] float maxDistance; // Maximum distance the player can dash
    [SerializeField] float dashTime; // Time it takes for the player to dash
    [SerializeField] float cooldown; // Time it takes for the player to dash again
    [SerializeField] float postDashMomentumFraction; // fraction of x-velocity reduced per run of FixedUpdate
    
    private Camera mainCamera;
    private Rigidbody2D rb;

    [SerializeField] private Vector2 velocity = Vector2.zero;
    [SerializeField] private bool canDash = true;
    [SerializeField] private bool isDashing = false;
    [SerializeField] private bool isSlowing = false;
    private void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rb = GetComponent<Rigidbody2D>();
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
        Vector2 displacement = Vector2.ClampMagnitude((Vector2)mousePos - (Vector2)transform.position, maxDistance);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, displacement.normalized, displacement.magnitude, LayerMask.GetMask("Ground"));
        if (hit.collider != null)
        {
            displacement = hit.point - (Vector2)transform.position - displacement.normalized * rb.GetComponent<Collider2D>().bounds.extents.magnitude;
        }
        this.velocity = displacement / dashTime;
        StartCoroutine(DashCoroutine());
    }

    private IEnumerator DashCoroutine()
    {
        isDashing = true;
        PlayerStateMachine psm = this.GetComponent<PlayerStateMachine>();
        
        Debug.Log("Starting wait: " + dashTime);
        yield return new WaitForSeconds(dashTime);
        Debug.Log("Done waiting: " + dashTime);

        rb.velocity *= postDashMomentumFraction;
        psm.EnableTrigger("OPT");
        psm.OnCooldown("c_special");

        isDashing = false;
        yield return new WaitForSeconds(cooldown);
        psm.OffCooldown("c_special");
    }
}
