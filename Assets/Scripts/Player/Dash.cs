using System;
using System.Collections;
using System.Threading;
using UnityEditor;
using UnityEngine;

//<summary>
// Base player action: player dashes towards the mouse location in a fixed time
//</summary>
public class Dash : MonoBehaviour
{
    [SerializeField] float maxDistance; // Maximum distance the player can dash

    [SerializeField] float dashTime; // Time it takes for the player to dash
    private Camera mainCamera;

    private Rigidbody2D rb;

    private Vector2 velocity;
    private Boolean isDashing;

    private void Start() {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rb = GetComponent<Rigidbody2D>();
        isDashing = false;
    }

    private void FixedUpdate() {
        if (!isDashing) return;

        rb.velocity = velocity;
    }

    //<summary>
    // Function called when the player presses the "Dash" keybind in Player Actions
    // Calculates the displacement vector between the player and the mouse and starts the dash
    //</summary>
    void OnDash() {
        if (isDashing) return;
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 displacement = Vector2.ClampMagnitude((Vector2)mousePos - (Vector2)transform.position, maxDistance);
        
        RaycastHit2D hit = Physics2D.Raycast(transform.position, displacement.normalized, displacement.magnitude, LayerMask.GetMask("Ground"));
        if (hit.collider != null) {
            displacement = hit.point - (Vector2)transform.position - displacement.normalized * rb.GetComponent<Collider2D>().bounds.extents.magnitude;
            Debug.DrawLine(transform.position, hit.point, Color.green, 20f);
        }
        this.velocity = displacement / dashTime;
        Debug.DrawLine(transform.position, transform.position + (Vector3)displacement, Color.red, 20f);
        StartCoroutine(DashCoroutine());
    }
    
    private IEnumerator DashCoroutine() {
        isDashing = true;
        rb.gravityScale = 0;
        GetComponent<MoveCharacter>().enabled = false;
        GetComponent<GrappleBehavioiur>().enabled = false;
        GetComponent<PlayerGroundAtack>().enabled = false;
        GetComponent<PartyManager>().enabled = false;

        yield return new WaitForSeconds(dashTime);
        
        isDashing = false;
        rb.velocity = Vector2.zero;
        rb.gravityScale = 1;
        GetComponent<MoveCharacter>().enabled = true;
        //GetComponent<GrappleBehavioiur>().enabled = true;
        GetComponent<PlayerGroundAtack>().enabled = true;
        GetComponent<PartyManager>().enabled = true;
    }
}
