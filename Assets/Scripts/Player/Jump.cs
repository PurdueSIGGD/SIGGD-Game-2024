using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Jump : MonoBehaviour
{

    private Rigidbody2D rb;
    private Stats stats;

    private int jumpForceIdx;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<Stats>();

        jumpForceIdx = stats.GetStatIndex("Jump Force");
    }

    public void StartJump()
    {
        rb.gravityScale = 4;
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(new Vector2(0, 1) * stats.ComputeValue(jumpForceIdx), ForceMode2D.Impulse);
    }
}
