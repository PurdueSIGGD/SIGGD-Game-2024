using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


/// <summary>
/// (For testing)Player movement script by yours truly, the World Team!!!!!! :D
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    //private variables
    private float horizontal;
    private float speed = 8f;
    private float jumpingPower = 14f;
    private bool isFacingRight = true;

    //serialized fields to reference the different objects
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //returns value of -1 or 0 or +1 depending on direction of movement
        horizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") && isGrounded())
        {
            //set rb velocity to x and jumping power
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }

        flip();
    }

    private void FixedUpdate()
    {
        //basically set its position to the position * speed 
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    private void flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector2 localScale = transform.localScale;
            localScale.x *= 1f;
            transform.localScale = localScale;
        }
    }
    private bool isGrounded()
    {
        //creates a little area around player if touching then you can jump
        //basically prevents double jump
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }
}
