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

    [SerializeField] float slowRate; // fraction of x-velocity reduced per run of FixedUpdate
    private Camera mainCamera;

    private Rigidbody2D rb;

    private InputAction playerActionMovement;
    private Vector2 velocity = Vector2.zero;

    private bool canDash = true;
    private bool isDashing = false;
    private bool isSlowing = false;
    private void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rb = GetComponent<Rigidbody2D>();
        playerActionMovement = GetComponent<PlayerInput>().actions.FindAction("Movement");
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            rb.velocity = velocity;
        }
        else if (isSlowing)
        {
            if (playerActionMovement.ReadValue<float>() != 0)
            {
                velocity = Vector2.zero;
                rb.velocity = new Vector2(0, rb.velocity.y);
                isSlowing = false;
                GetComponent<MoveCharacter>().enabled = true;
                return;
            }

            rb.velocity = new Vector2(Mathf.MoveTowards(rb.velocity.x, 0, Mathf.Abs(velocity.x * slowRate)), rb.velocity.y);
            if (Mathf.Abs(rb.velocity.x) < 3f)
            {
                velocity = Vector2.zero;
                rb.velocity = new Vector2(0, rb.velocity.y);
                isSlowing = false;
                GetComponent<MoveCharacter>().enabled = true;
            }
        }
    }

    //<summary>
    // Function called when the player presses the "Dash" keybind in Player Actions
    // Calculates the displacement vector between the player and the mouse and starts the dash
    //</summary>
    void OnSpecial()
    {
        if (!canDash || isDashing) return;
        canDash = false;
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
        GetComponent<MoveCharacter>().enabled = false;
        //GetComponent<GrappleBehavioiur>().enabled = false;
        GetComponent<PlayerGroundAtack>().enabled = false;
        GetComponent<PartyManager>().enabled = false;

        yield return new WaitForSeconds(dashTime);

        isDashing = false;
        isSlowing = true;
        //GetComponent<GrappleBehavioiur>().enabled = true;
        GetComponent<PlayerGroundAtack>().enabled = true;
        GetComponent<PartyManager>().enabled = true;
        yield return new WaitForSeconds(cooldown);
        canDash = true;
    }
}
