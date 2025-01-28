using UnityEngine;

public class Glide : MonoBehaviour
{
    //initiate variables
    private Rigidbody2D rb;
    private StatManager stats;

    private bool isFalling;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<StatManager>();

    }

    private void FixedUpdate()
    {
        if (isFalling)
        {
            rb.velocity = new Vector2(rb.velocity.x, -1 * stats.ComputeValue("Glide Fall Speed"));
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