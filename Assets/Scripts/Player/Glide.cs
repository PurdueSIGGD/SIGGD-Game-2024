using UnityEngine;

[DisallowMultipleComponent]
public class Glide : MonoBehaviour, IStatList
{
    [SerializeField]
    private StatManager.Stat[] statList;

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
            Vector2 newVel = new Vector2(rb.velocity.x, 0f);
            float glideFallSpeed = -1 * stats.ComputeValue("Glide Fall Speed");

            //Glide deceleration while moving up
            if (rb.velocity.y > glideFallSpeed)
            {
                float deaccel = Mathf.Clamp(rb.velocity.y * (1 - stats.ComputeValue("Glide Fall Upward Deaccel.")), 0.2f, 100f);
                newVel.y = Mathf.Max(rb.velocity.y - deaccel, glideFallSpeed);
            }
            //Glide deceleration while moving down
            else if (rb.velocity.y < glideFallSpeed)
            {
                float deaccel = Mathf.Clamp(rb.velocity.y * (1 - stats.ComputeValue("Glide Fall Downward Deaccel.")), -100f, -0.2f);
                newVel.y = Mathf.Min(rb.velocity.y - deaccel, glideFallSpeed);
            }
            //Maintain standard glide speed
            else
            {
                newVel.y = glideFallSpeed;
            }

            rb.velocity = newVel;
        }
    }

    public void StartGlide()
    {
        isFalling = true;
        rb.gravityScale = 0;
        AudioManager.Instance.SFXBranch.PlaySFXTrack(SFXTrackName.GLIDE);
    }

    public void StopGlide()
    {
        isFalling = false;
        rb.gravityScale = 4;
        AudioManager.Instance.SFXBranch.StopSFXTrack(SFXTrackName.GLIDE);
    }

    public StatManager.Stat[] GetStatList()
    {
        return statList;
    }
}