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
    [SerializeField] bool moving; // Is the player moving?
    float minimumMoveSpeed = 0.1f; //The minimum horizontal velocity required to be considered moving
    InputAction moveInput; // The move action from the playerInput component
    [SerializeField] bool falling; // Is player velocity less than zero?
    float minimumFallSpeed = -0.1f; // The minimum negative vertical velocity required to be considered falling
    [SerializeField] bool special; // Is player currently using their special ability?
    ISpecialMove currentSpecial; // A reference to the interface attatched to the special move script
    InputAction specialInput; // The special key action from the playerInput component
    [SerializeField] bool gliding; // Is player currently gliding in the air?
    InputAction jumpInput; // The jump action from the playerInput component
    IGlideMove glideScript; // Reference to the interface attatched to glide script
    [SerializeField] bool jumping;

    bool i_up, i_down, i_horizontal, i_special, p_grounded, p_falling;


    [Header("References")]
    Animator animator; // the animator of the player object
    Rigidbody2D rb; // the rigidbody of the player object
    [SerializeField] String currentAnimation;


    void Start()
    {
        moveInput = GetComponent<PlayerInput>().actions.FindAction("Movement");
        specialInput = GetComponent<PlayerInput>().actions.FindAction("Special");
        jumpInput = GetComponent<PlayerInput>().actions.FindAction("Jump");
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

        UpdateHorizontal();
        UpdateGrounded();
        UpdateFalling();
        UpdateUp();

        //UpdateSpecialBool();
        //UpdateGlidingBool();
        ReadCurrentAnimatorState();
    }
    /// <summary>
    /// Toggles gliding boolean of this script and animator depending on the state of the glidescript that implements IGlideMove
    /// </summary>
    void UpdateGlidingBool()
    {
        glideScript = GetComponent<IGlideMove>();
        if (glideScript == null)
        {
            gliding = false;
            return;
        }
        gliding = glideScript.GetBool();
        if (gliding != animator.GetBool("gliding"))
        {
            animator.SetBool("gliding", gliding);
        }
    }
    /// <summary>
    /// Toggles special boolean of this script and animator if the special action script - implementing ISpecialMove and referenced by currentSpecial -  
    /// determines itself to be active.
    /// </summary>
    void UpdateSpecialBool()
    {
        currentSpecial = GetComponent<ISpecialMove>();
        if (currentSpecial == null)
        {
            special = false;
            return;
        }
        special = currentSpecial.GetBool();
        if (special != animator.GetBool("special"))
        {
            animator.SetBool("special", special);
        }
    }
    /// <summary>
    /// Toggles falling boolean of this script and animator if -y velocity is great enough to be considered falling and player's special ability is not active
    /// and the player is not gliding.
    /// </summary>
    void UpdateFalling()
    {
        p_falling = rb.velocity.y < minimumFallSpeed;
        animator.SetBool("p_falling", p_falling);
    }
    /// <summary>
    /// Toggles moving boolean of this script and animator if moveInput is not zero and the horizontal velocity exceeds minimum move speed threshold
    /// </summary>
    void UpdateHorizontal()
    {
        i_horizontal = moveInput.ReadValue<float>() != 0;
        animator.SetBool("i_horizontal", i_horizontal);
    }

    /// <summary>
    /// Toggles grounded boolean of this script and animator if player detects ground at feet
    /// </summary>
    void UpdateGrounded()
    {
        p_grounded = Physics2D.OverlapCircle(transform.position + new Vector3(0, yFeetDisplacement, 0), groundDetectRadius, groundLayer);
        animator.SetBool("p_grounded", p_grounded);
    }

    void UpdateUp()
    {
        i_up = jumpInput.ReadValue<float>() != 0;
        animator.SetBool("i_up", i_up);
    }

    /// <summary>
    /// Reads the current animator state and sets the currentAnimation string to the currently playing animation
    /// </summary>
    void ReadCurrentAnimatorState()
    {
        AnimatorClipInfo[] animatorClip = animator.GetCurrentAnimatorClipInfo(0);
        currentAnimation = animatorClip[0].clip.name;
    }
}
