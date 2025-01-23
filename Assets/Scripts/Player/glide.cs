using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Glide : MonoBehaviour
{
    //initiate variables
    private Rigidbody2D rb;
    private Stats stats;

    private int glideFallSpeedIdx;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<Stats>();

        glideFallSpeedIdx = stats.GetStatIndex("Glide Fall Speed");
    }

    public void StartGlide()
    {
        rb.velocity = new Vector2(rb.velocity.x, -1 * stats.ComputeValue(glideFallSpeedIdx));
        rb.gravityScale = 0;
    }

    public void StopGlide()
    {
        rb.gravityScale = 4;
    }
}