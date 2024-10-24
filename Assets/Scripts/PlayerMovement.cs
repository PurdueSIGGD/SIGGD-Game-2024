using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private InputAction playerAction;
    private Rigidbody2D rb;
    private Boolean doubleJump = true;
    private Boolean onGround = false;

    private Stats stats;
    private int moveSpeedIdx;
    private int jumpForceIdx;

    // Start is called before the first frame update
    void Start()
    {
        playerAction = GetComponent<PlayerInput>().actions.FindAction("Movement");
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<Stats>();
        moveSpeedIdx = stats.GetStatIndex("Move Speed");
        jumpForceIdx = stats.GetStatIndex("Jump Force");
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    private void Movement()
    {
        float input = playerAction.ReadValue<float>();
        rb.velocity = new Vector2(input * stats.ComputeValue(moveSpeedIdx), rb.velocity.y);
    }

    private void OnJump()
    {
        Debug.Log(onGround + " " + doubleJump);
        if (onGround || doubleJump)
        {
            rb.AddForce(new Vector2(0, 1) * stats.ComputeValue(jumpForceIdx), ForceMode2D.Impulse);
            if (!onGround)
            {
                doubleJump = false;
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        doubleJump = true;
        onGround = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        onGround = false;
    }

}
