using UnityEngine;

public class Glide : MonoBehaviour
{
    //initiate variables
    private Rigidbody2D rb;
    private Stats stats;

    private int glideFallSpeedIdx;
    private bool isFalling;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<Stats>();

        glideFallSpeedIdx = stats.GetStatIndex("Glide Fall Speed");
    }

    private void FixedUpdate()
    {
        if (isFalling)
        {
            rb.velocity = new Vector2(rb.velocity.x, -1 * stats.ComputeValue(glideFallSpeedIdx));
        }
    }

    public void StartGlide()
    {
        isFalling = true;
        rb.gravityScale = 0;
    }

    public void StopGlide()
    {
        isFalling = false;
        rb.gravityScale = 4;
    }
}