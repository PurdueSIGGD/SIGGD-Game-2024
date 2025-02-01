using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.SceneTemplate;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.InputSystem;

[DisallowMultipleComponent]
public class Move : MonoBehaviour, IStatList
{
    [SerializeField]
    private StatManager.Stat[] statList;

    private InputAction moveInput;
    private Rigidbody2D rb;

    private StatManager stats;

    private bool gliding = false;
    private bool dashing = false;


    // Start is called before the first frame update
    void Start()
    {
        moveInput = GetComponent<PlayerInput>().actions.FindAction("Move");
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<StatManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!dashing)
        {
            Movement();
        }
    }

    /// <summary>
    /// Controlls the basic movement of the player (moving left and right and ground slam)
    /// </summary>
    private void Movement()
    {
        float accel, maxSpeed, deaccel = 0;
        if (gliding)
        {
            accel = stats.ComputeValue("Glide Accel.");
            maxSpeed = stats.ComputeValue("Max Glide Speed");
            deaccel = stats.ComputeValue("Glide Deaccel.");
        } else
        {
            accel = stats.ComputeValue("Running Accel.");
            maxSpeed = stats.ComputeValue("Max Running Speed");
            deaccel = stats.ComputeValue("Running Deaccel.");
        }
//        Debug.Log(String.Format("Max speed: {0}, Accel: {1}, Deaccel {2}", maxSpeed, accel, deaccel));

        float input = moveInput.ReadValue<float>();
        Vector2 newVel = new Vector2(0, 0);

        // accelerates player in direction of input
        newVel.x = rb.velocity.x + input * accel;

        // caps top horizontal speed
        if (newVel.magnitude > maxSpeed)
        {
            newVel = newVel.normalized * maxSpeed;
        }

        // deaccelerate if no input
        if (input == 0)
        {
            newVel.x *= deaccel;
        }

        // keep updating y velocity
        newVel.y = rb.velocity.y;

        // update rigidbody velocity to new velocity
        rb.velocity = newVel;
    }

    public void StartGlide()
    {
        gliding = true;
    }

    public void StopGlide()
    {
        gliding = false;
    }
    public void StartDash()
    {
        dashing = true;
    }

    public void StopDash()
    {
        dashing = false;
    }

    public StatManager.Stat[] GetStatList()
    {
        return statList;
    }
}