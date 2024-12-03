using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.SceneTemplate;
using UnityEngine;
using UnityEngine.InputSystem;

public class Move : MonoBehaviour
{
    private InputAction playerActionMovement;
    private InputAction playerActionDown;
    private Rigidbody2D rb;
    public Boolean doubleJump = true;

    private Stats stats;
    private int maxSpeedIdx;
    private int accelerationIdx;
    private int jumpForceIdx;

    // Start is called before the first frame update
    void Start()
    {
        playerActionMovement = GetComponent<PlayerInput>().actions.FindAction("Movement");
        playerActionDown = GetComponent<PlayerInput>().actions.FindAction("Down");
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<Stats>();
        maxSpeedIdx = stats.GetStatIndex("Max Speed");
        accelerationIdx = stats.GetStatIndex("Acceleration");

        jumpForceIdx = stats.GetStatIndex("Jump Force");
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
        float input = playerActionMovement.ReadValue<float>();
        Vector2 newVel = new Vector2(0, 0);

        // accelerates player in direction of input
        newVel.x = rb.velocity.x + input * stats.ComputeValue(accelerationIdx);

        // caps top speed
        if (newVel.magnitude > stats.ComputeValue(maxSpeedIdx))
        {
            newVel = newVel.normalized * stats.ComputeValue(maxSpeedIdx);
        }

        // deaccelerate if no input
        if (input == 0)
        {
            newVel.x *= 0.9f;
        }

        // keep updating y velocity
        newVel.y = rb.velocity.y;

        // update rigidbody velocity to new velocity
        rb.velocity = newVel;
    }

    /// <summary>
    /// Method is called once the player presses the button binded to "Jump" in Player Actions also controlls double jump
    /// </summary>
    private void OnJump()
    {
        if (OnGround())
        {
            doubleJump = true;
            rb.gravityScale = 4;
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(new Vector2(0, 1) * stats.ComputeValue(jumpForceIdx), ForceMode2D.Impulse);
        }
        //if double jump has not been used yet and in the air, turn off double jump
        else if (doubleJump)
        {
            doubleJump = false;
            rb.gravityScale = 4;
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(new Vector2(0, 1) * stats.ComputeValue(jumpForceIdx), ForceMode2D.Impulse);
        }
    }

    /// <summary>
    /// Method that uses a circle cast to determine whether or not the player in on the ground
    /// </summary>
    /// <returns>Boolean of whether or not the player is on the ground</returns>
    public Boolean OnGround()
    {
        LayerMask mask = LayerMask.GetMask("ground");
        return Physics2D.CircleCast(transform.position + Vector3.down, 0.5f, Vector2.down.normalized, mask);
    }
}