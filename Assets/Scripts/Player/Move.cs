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
    private bool dashing = false;
    private bool charging = false;

    private float accel;
    private float deaccel;
    private float maxSpeed;


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

        accel = stats.ComputeValue(runningAccelIdx);
        maxSpeed = stats.ComputeValue(maxRunningSpeedIdx);
        deaccel = stats.ComputeValue(runningDeaccelIdx);
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
        //float accel, maxSpeed, deaccel = 0;
        /*if (gliding)
        {
            accel = stats.ComputeValue(glideAccelIdx);
            maxSpeed = stats.ComputeValue(maxGlideSpeedIdx);
            deaccel = stats.ComputeValue(glideDeaccelIdx);
        } else
        {
            accel = stats.ComputeValue(runningAccelIdx);
            maxSpeed = stats.ComputeValue(maxRunningSpeedIdx);
            deaccel = stats.ComputeValue(runningDeaccelIdx);
        }*/

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

        gameObject.transform.localScale = new Vector3(Mathf.Sign(rb.velocity.x) * 1, 1, 1);
    }

    public void StartGlide()
    {
        gliding = true;
        accel = stats.ComputeValue(glideAccelIdx);
        maxSpeed = stats.ComputeValue(maxGlideSpeedIdx);
        deaccel = stats.ComputeValue(glideDeaccelIdx);
    }

    public void StopGlide()
    {
        gliding = false;
        accel = stats.ComputeValue(runningAccelIdx);
        maxSpeed = stats.ComputeValue(maxRunningSpeedIdx);
        deaccel = stats.ComputeValue(runningDeaccelIdx);
    }
    public void StartDash()
    {
        dashing = true;
        accel = stats.ComputeValue(runningAccelIdx);
        maxSpeed = stats.ComputeValue(maxRunningSpeedIdx);
        deaccel = stats.ComputeValue(runningDeaccelIdx);
    }

    public void StopDash()
    {
        dashing = false;
    }

    public void StartHeavyChargeUp()
    {
        charging = true;
        accel = stats.ComputeValue(runningAccelIdx)/2;
        maxSpeed = stats.ComputeValue(maxRunningSpeedIdx)/2;
        deaccel = stats.ComputeValue(runningDeaccelIdx);
    }

    public void StopHeavyChargeUp()
    {
        if (charging)
        {
            charging = false;
            accel = stats.ComputeValue(runningAccelIdx);
            maxSpeed = stats.ComputeValue(maxRunningSpeedIdx);
            deaccel = stats.ComputeValue(runningDeaccelIdx);
            Debug.Log("Stop Heavy ChargeUp");
        }
    }

    public void StartHeavyPrimed()
    {
        Debug.Log("Primed");
        charging = false;
        accel = stats.ComputeValue(runningAccelIdx) / 2;
        maxSpeed = stats.ComputeValue(maxRunningSpeedIdx) / 2;
        deaccel = stats.ComputeValue(runningDeaccelIdx);
    }

    public void StopHeavyPrimed()
    {
        accel = stats.ComputeValue(runningAccelIdx);
        maxSpeed = stats.ComputeValue(maxRunningSpeedIdx);
        deaccel = stats.ComputeValue(runningDeaccelIdx);
    }
}