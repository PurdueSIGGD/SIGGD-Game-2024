using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.SceneTemplate;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.InputSystem;

public class Move : MonoBehaviour
{
    private InputAction moveInput;
    private InputAction playerActionDown;
    private Rigidbody2D rb;
    public Boolean doubleJump = true;

    private Stats stats;
    private int maxRunningSpeedIdx;
    private int runningAccelIdx;
    private int runningDeaccelIdx;


    private int maxGlideSpeedIdx;
    private int glideAccelIdx;
    private int glideDeaccelIdx;

    private bool gliding = false;


    // Start is called before the first frame update
    void Start()
    {
        moveInput = GetComponent<PlayerInput>().actions.FindAction("Move");
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<Stats>();

        maxRunningSpeedIdx = stats.GetStatIndex("Max Running Speed");
        runningAccelIdx = stats.GetStatIndex("Running Acceleration");
        runningDeaccelIdx = stats.GetStatIndex("Running Deacceleration");

        maxGlideSpeedIdx = stats.GetStatIndex("Max Glide Speed");
        glideAccelIdx = stats.GetStatIndex("Glide Acceleration");
        glideDeaccelIdx = stats.GetStatIndex("Glide Deacceleration");
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    /// <summary>
    /// Controlls the basic movement of the player (moving left and right and ground slam)
    /// </summary>
    private void Movement()
    {
        float accel, maxSpeed, deaccel = 0;
        if (gliding)
        {
            accel = stats.ComputeValue(glideAccelIdx);
            maxSpeed = stats.ComputeValue(maxGlideSpeedIdx);
            deaccel = stats.ComputeValue(glideDeaccelIdx);
        } else
        {
            accel = stats.ComputeValue(runningAccelIdx);
            maxSpeed = stats.ComputeValue(maxRunningSpeedIdx);
            deaccel = stats.ComputeValue(runningDeaccelIdx);
        }

        float input = moveInput.ReadValue<float>();
        Vector2 newVel = new Vector2(0, 0);

        // accelerates player in direction of input
        newVel.x = rb.velocity.x + input * accel;

        // caps top speed
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
}