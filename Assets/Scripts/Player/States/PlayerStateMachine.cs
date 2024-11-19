using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// A simple state machine for the player that toggles booleans hooked up to an animator
/// </summary>
public class PlayerStateMachine : MonoBehaviour
{

    [Header("Control Booleans")]
    [SerializeField] bool grounded; // Is player on the ground?
    float yFeetDisplacement = -0.5f; // Distance between the transform position of player and the player's "feet"
    [SerializeField] LayerMask groundLayer; //The layer representing ground
    float groundDetectRadius = 0.5f; //The radius of the circle checking for ground overlap
    [SerializeField] bool moving; // Is the player
    InputAction moveInput; // The move action from the playerInput component
    [SerializeField] bool falling; // Is player velocity less than zero?
    float minimumFallSpeed = -0.1f; // The minimum negative vertical velocity required to be considered falling

    [Header("References")]
    Animator animator; // the animator of the player object
    Rigidbody2D rb; // the rigidbody of the player object
    [SerializeField] String currentAnimation;


    void Start()
    {
        moveInput = GetComponent<PlayerInput>().actions.FindAction("Movement");
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        UpdateGroundedBool();
        UpdateMovingBool();
        UpdateFallingBool();
        ReadCurrentAnimatorState();
    }
    /// <summary>
    /// Toggles falling boolean of this script and animator if -y velocity is great enough to be considered falling 
    /// </summary>
    void UpdateFallingBool()
    {
        falling = rb.velocity.y < minimumFallSpeed;
        if (falling != animator.GetBool("falling"))
        {
            animator.SetBool("falling", falling);
        }
    }
    /// <summary>
    /// Toggles moving boolean of this script and animator if moveInput is not zero
    /// </summary>
    void UpdateMovingBool()
    {
        moving = moveInput.ReadValue<float>() != 0;
        if (moving != animator.GetBool("moving"))
        {
            animator.SetBool("moving", moving);
        }
    }
    /// <summary>
    /// Toggles grounded boolean of this script and animator if player detects ground at feet
    /// </summary>
    void UpdateGroundedBool()
    {
        grounded = Physics2D.OverlapCircle(transform.position + new Vector3(0, yFeetDisplacement, 0), groundDetectRadius, groundLayer);
        if (grounded != animator.GetBool("grounded"))
        {
            animator.SetBool("grounded", grounded);
        }
    }
    void ReadCurrentAnimatorState()
    {
        AnimatorClipInfo[] animatorClip = animator.GetCurrentAnimatorClipInfo(0);
        currentAnimation = animatorClip[0].clip.name;
    }
}
