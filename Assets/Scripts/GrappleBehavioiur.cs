using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script attached to the player that controls the grappling mechanics of the game
/// </summary>

public class GrappleBehavioiur : MonoBehaviour
{
    [SerializeField] float range = 10.0f; //Range of the grapple projectile
    [SerializeField] float speed = 10.0f; //How fast you go when you grapple to an object

    private Camera mainCamera;
    private Rigidbody2D rb;
    private Boolean grappleStart = false;
    private Vector2 positionToGo;

    private void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (grappleStart)
        {
            Vector2 dirToGo = new Vector2(transform.position.x, transform.position.y) - positionToGo;
            rb.velocity = dirToGo * -speed;
            //rb.AddForce(dirToGo.normalized * new Vector2(-5000, -50), ForceMode2D.Force);
            if (dirToGo.sqrMagnitude < 1)
            {
                grappleStart = false;
            }
        }
    }
    /// <summary>
    /// function that gets called when you press the "Grapple" keybind in the Player Actions
    /// </summary>
    void OnGrapple()
    {
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 subV = gameObject.transform.position - mousePos;
        RaycastHit2D hit = Physics2D.Raycast(gameObject.transform.position, -subV.normalized, range);
        if(hit.collider != null)
        {
            Debug.Log((new Vector2(transform.position.x, transform.position.y) - hit.point).normalized * -500);
            grappleStart = true;
            positionToGo = hit.point;
        }
    }
}
