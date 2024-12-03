using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Glide : MonoBehaviour, IGlideMove
{
    //initiate variables
    //the constant fall speed for glide
    [SerializeField] private float glideFallSpeed = 2f;
    private MoveCharacter moveCharacter;
    private Rigidbody2D rb;
    private InputAction playerActionJump;
    private bool isGliding = false;

    private void Start()
    {
        moveCharacter = GetComponent<MoveCharacter>();
        rb = GetComponent<Rigidbody2D>();
        playerActionJump = GetComponent<PlayerInput>().actions.FindAction("Jump");
    }

    private void Update()
    {
        //if player is not on ground and if all jumps are used up
        if (!moveCharacter.OnGround() && !moveCharacter.doubleJump)
        {
            HandleGlide();
        }
        else
        {
            //if on ground then no glide
            StopGlide();
        }
    }

    private void HandleGlide()
    {
        //if jump is pressed and no velocity
        if (playerActionJump.IsPressed() && rb.velocity.y <= 0)
        {
            if (!isGliding)
            {
                StartGlide();
            }
            rb.velocity = new Vector2(rb.velocity.x, -glideFallSpeed);
        }
        else if (isGliding)
        {
            StopGlide();
        }
    }

    private void StartGlide()
    {
        isGliding = true;
        //disabling gravity
        rb.gravityScale = 0;
    }

    private void StopGlide()
    {
        isGliding = false;
        rb.gravityScale = 4;
    }
    public bool GetBool()
    {
        return isGliding;
    }
}