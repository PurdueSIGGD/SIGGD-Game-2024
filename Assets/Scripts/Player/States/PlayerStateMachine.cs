using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

/// <summary>
/// A simple state machine for the player that toggles booleans hooked up to an animator
/// </summary>
[DisallowMultipleComponent]
public class PlayerStateMachine : MonoBehaviour
{
    [SerializeField] LayerMask groundLayer; //The layer representing ground
    [SerializeField] public string currentAnimation;

    float groundDetectRadius = 0.5f; //The radius of the circle checking for ground overlap
    float minimumFallSpeed = -0.1f; // The minimum negative vertical velocity required to be considered falling
    float yFeetDisplacement = -1.5f; // Distance between the transform position of player and the player's "feet"

    InputAction moveInput; // The move action from the playerInput component
    InputAction jumpInput; // The jump action from the playerInput component
    InputAction fallInput; // The fall actions frmo playerINput component
    InputAction specialInput; // The special key action from the playerInput component
    InputAction attackInput;

    Animator animator; // the animator of the player object
    Rigidbody2D rb; // the rigidbody of the player object
    Camera mainCamera; //the main Camera of the current Scene


    void Start()
    {
        PlayerInput input = GetComponent<PlayerInput>();
        moveInput = input.actions.FindAction("Move");
        jumpInput = input.actions.FindAction("Jump");
        fallInput = input.actions.FindAction("Fall");
        attackInput = input.actions.FindAction("Attack");
        specialInput = input.actions.FindAction("Special");

        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    void Update()
    {
        UpdateHorizontal();
        UpdateGrounded();
        UpdateFalling();
        UpdateUp();
        UpdateDown();
        UpdateAttack();
        UpdateSpecial();
        UpdateMouseDir();
        ReadCurrentAnimatorState();
    }

    /// <summary>
    /// Toggles falling boolean of this script and animator if -y velocity is great enough to be considered falling and player's special ability is not active
    /// and the player is not gliding.
    /// </summary>
    void UpdateFalling()
    {
        bool p_falling = rb.velocity.y < minimumFallSpeed;
        animator.SetBool("p_falling", p_falling);
    }

    /// <summary>
    /// Toggles moving boolean of this script and animator if moveInput is not zero and the horizontal velocity exceeds minimum move speed threshold
    /// </summary>
    void UpdateHorizontal()
    {
        bool i_horizontal = moveInput.ReadValue<float>() != 0;
        animator.SetBool("i_horizontal", i_horizontal);
    }

    /// <summary>
    /// Toggles grounded boolean of this script and animator if player detects ground at feet
    /// </summary>
    void UpdateGrounded()
    {
        bool p_grounded = Physics2D.OverlapCircle(transform.position + new Vector3(0, yFeetDisplacement, 0), groundDetectRadius, groundLayer);
        animator.SetBool("p_grounded", p_grounded);
    }

    void UpdateUp()
    {
        bool i_up = jumpInput.ReadValue<float>() != 0;
        animator.SetBool("i_up", i_up);
    }

    void UpdateDown()
    {
        bool i_down = fallInput.ReadValue<float>() != 0;
        animator.SetBool("i_down", i_down);
    }

    void UpdateAttack()
    {
        bool i_attack = attackInput.ReadValue<float>() != 0;
        animator.SetBool("i_attack", i_attack);
    }

    void UpdateSpecial()
    {
        bool i_special = specialInput.ReadValue<float>() != 0;
        animator.SetBool("i_special", i_special);
    }

    void UpdateMouseDir()
    {
        int mouseDir = 0;
        Vector2 mouseDiff = transform.position - mainCamera.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
        float angle = Mathf.Atan2(mouseDiff.y, mouseDiff.x) + Mathf.PI;
        if (angle > Mathf.PI / 4 && angle < 3 * Mathf.PI / 4)
        {
            mouseDir = 1;
        }
        else if (angle > 3 * Mathf.PI / 4 && angle < 5 * Mathf.PI / 4)
        {
            mouseDir = 2;
        }
        else if (angle > 5 * Mathf.PI / 4 && angle < 7 * Mathf.PI / 4)
        {
            mouseDir = 3;
        }
        animator.SetInteger("m_dir", mouseDir);
    }

    /// <summary>
    /// Reads the current animator state and sets the currentAnimation string to the currently playing animation
    /// </summary>
    void ReadCurrentAnimatorState()
    {
        AnimatorClipInfo[] animatorClip = animator.GetCurrentAnimatorClipInfo(0);
        currentAnimation = animatorClip[0].clip.name;
    }

    public void EnableTrigger(string triggerName)
    {
        animator.SetTrigger(triggerName);
    }

    public void OnCooldown(string cooldownName)
    {
        animator.SetBool(cooldownName, true);
    }

    public void OffCooldown(string cooldownName)
    {
        animator.SetBool(cooldownName, false);
    }

    /// <summary>
    /// Stuns the player by locking all player inputs
    /// </summary>
    /// <param name="duration"></param>
    public void SetStun(float duration)
    {
        StartCoroutine(StunCoroutine(duration));
    }

    private IEnumerator StunCoroutine(float duration)
    {
        animator.speed = 0;
        moveInput.Disable();
        jumpInput.Disable();
        specialInput.Disable();
        attackInput.Disable();
        yield return new WaitForSeconds(duration);
        animator.speed = 1;
        moveInput.Enable();
        jumpInput.Enable();
        specialInput.Enable();
        attackInput.Enable();
    }
}
