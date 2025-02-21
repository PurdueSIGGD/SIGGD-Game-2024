using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.SceneTemplate;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.InputSystem;

public class Move : MonoBehaviour, IStatList
{
    [SerializeField]
    private StatManager.Stat[] statList;

    private InputAction moveInput;
    private InputAction playerActionDown;
    private Rigidbody2D rb;
    public Boolean doubleJump = true;

    private StatManager stats;
    private Animator animator;

    private bool dashing = false;
    //private bool dashEnding = false;
    private bool charging = false;

    private float accel;
    private float deaccel;
    public float maxSpeed;

    public float overflowSpeed;
    private float overflowDeaccel;


    // Start is called before the first frame update
    void Start()
    {
        moveInput = GetComponent<PlayerInput>().actions.FindAction("Move");
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<StatManager>();
        animator = GetComponent<Animator>();

        accel = stats.ComputeValue("Running Accel.");
        maxSpeed = stats.ComputeValue("Max Running Speed");
        deaccel = stats.ComputeValue("Running Deaccel.");

        overflowSpeed = maxSpeed;
        overflowDeaccel = 0.96f;
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
        float input = moveInput.ReadValue<float>();
        Vector2 newVel = new Vector2(0, 0);

        // accelerates player in direction of input
        newVel.x = rb.velocity.x + input * accel;

        // caps top horizontal speed, accounting for overflow top speed due to knockback
        if (overflowSpeed > maxSpeed)
        {
            newVel.x = Mathf.Clamp(newVel.x, -1 * overflowSpeed, overflowSpeed);
            overflowSpeed = Mathf.Clamp(Mathf.Abs(rb.velocity.x), maxSpeed, overflowSpeed * overflowDeaccel);
        }
        else
        {
            newVel.x = Mathf.Clamp(newVel.x, -1 * maxSpeed, maxSpeed);
            overflowSpeed = maxSpeed;
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

    public void StartJump()
    {
        accel = stats.ComputeValue("Airborne Accel.");
        maxSpeed = stats.ComputeValue("Max Running Speed");
        deaccel = stats.ComputeValue("Airborne Deaccel.");
    }

    public void StopJump()
    {
        accel = stats.ComputeValue("Running Accel.");
        maxSpeed = stats.ComputeValue("Max Running Speed");
        deaccel = stats.ComputeValue("Running Deaccel.");
    }

    public void StartFall()
    {
        accel = stats.ComputeValue("Airborne Accel.");
        maxSpeed = stats.ComputeValue("Max Running Speed");
        deaccel = stats.ComputeValue("Airborne Deaccel.");
    }

    public void StopFall()
    {
        accel = stats.ComputeValue("Running Accel.");
        maxSpeed = stats.ComputeValue("Max Running Speed");
        deaccel = stats.ComputeValue("Running Deaccel.");
    }

    public void StartFastFall()
    {
        accel = stats.ComputeValue("Airborne Accel.");
        maxSpeed = stats.ComputeValue("Max Running Speed");
        deaccel = stats.ComputeValue("Airborne Deaccel.");
    }

    public void StopFastFall()
    {
        accel = stats.ComputeValue("Running Accel.");
        maxSpeed = stats.ComputeValue("Max Running Speed");
        deaccel = stats.ComputeValue("Running Deaccel.");
    }

    public void StartGlide()
    {
        accel = stats.ComputeValue("Glide Accel.");
        maxSpeed = stats.ComputeValue("Max Glide Speed");
        deaccel = stats.ComputeValue("Glide Deaccel.");
    }

    public void StopGlide()
    {
        accel = stats.ComputeValue("Running Accel.");
        maxSpeed = stats.ComputeValue("Max Running Speed");
        deaccel = stats.ComputeValue("Running Deaccel.");
    }
    public void StartDash()
    {
        dashing = true;
        accel = stats.ComputeValue("Running Accel.");
        maxSpeed = stats.ComputeValue("Max Running Speed");
        deaccel = stats.ComputeValue("Running Deaccel.");
    }

    public void StopDash()
    {
        dashing = false;
        if (animator.GetBool("p_grounded")) return;
        ApplyKnockback(rb.velocity.normalized, rb.velocity.magnitude);
    }

    public void StartHeavyChargeUp()
    {
        charging = true;
        accel = stats.ComputeValue("Accel. while Heavy");
        maxSpeed = stats.ComputeValue("Max Speed while Heavy");
        deaccel = stats.ComputeValue("Deaccel. while Heavy");
    }

    public void StopHeavyChargeUp()
    {
        if (charging)
        {
            charging = false;
            accel = stats.ComputeValue("Running Accel.");
            maxSpeed = stats.ComputeValue("Max Running Speed");
            deaccel = stats.ComputeValue("Running Deaccel.");
            Debug.Log("Stop Heavy ChargeUp");
        }
    }

    public void StartHeavyPrimed()
    {
        Debug.Log("Primed");
        charging = false;
        accel = stats.ComputeValue("Accel. while Heavy");
        maxSpeed = stats.ComputeValue("Max Speed while Heavy");
        deaccel = stats.ComputeValue("Deaccel. while Heavy");
    }

    public void StopHeavyPrimed()
    {
        accel = stats.ComputeValue("Running Accel.");
        maxSpeed = stats.ComputeValue("Max Running Speed");
        deaccel = stats.ComputeValue("Running Deaccel.");
    }

    public StatManager.Stat[] GetStatList()
    {
        return statList;
    }

    public void ApplyKnockback(Vector2 direction, float knockbackStrength)
    {
        overflowSpeed = knockbackStrength;
        rb.AddForce(direction.normalized * knockbackStrength);
    }
}