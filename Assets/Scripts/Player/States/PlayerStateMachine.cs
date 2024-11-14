using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// A simple state machine for the player that toggles booleans hooked up to an animator
/// </summary>
public class PlayerStateMachine : MonoBehaviour
{

    [Header("Control Booleans")]
    [SerializeField] bool grounded; // Is player on the ground?
    [SerializeField] Transform feetTransform; // Transform of the object representing player contact with floor
    [SerializeField] LayerMask groundLayer; //The layer representing ground
    [SerializeField] float groundDetectRadius; //The radius of the circle checking for ground overlap
    [SerializeField] bool moving; // Is the player
    InputAction moveInput; // The move action from the playerInput component

    [Header("References")]
    Animator animator; // the animator of the player object
    String currentState;


    void Start()
    {
        moveInput = GetComponent<PlayerInput>().actions.FindAction("Movement");
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        UpdateGroundedBool();
        UpdateMovingBool();
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
        grounded = Physics2D.OverlapCircle(feetTransform.position, groundDetectRadius, groundLayer);
    }
}
