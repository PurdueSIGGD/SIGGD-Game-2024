using UnityEngine;

/// <summary>
/// A type of a an Enemy Projectile. Will track the player until a certain distance
/// away from it.
/// </summary>
public class TrackingProjectile : EnemyProjectile
{
    SpriteRenderer sprite;
    [SerializeField] float trackingStrength; // A larger value will allow the projectile to turn faster.
    [SerializeField] float trackingDistance; // A larger value will lead the projectile to loose tracking earlier
    private float hangTime = 0;
    private bool tracking = true;
    protected Transform player;

    //Consistently tracks player
    protected override void Start()
    {
        base.Start();
        sprite = GetComponent<SpriteRenderer>();
        player = PlayerID.instance.transform;
        Vector3 directionToTarget = (player.position - transform.position).normalized;
        rb.velocity = directionToTarget * speed;
    }
    void Update()
    {
        if (PlayerID.instance.gameObject.transform.position.x - transform.position.x < 0)
        {
            transform.localScale = new Vector3(0.7f, transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(-0.7f, transform.localScale.y, transform.localScale.z);
        }
    }

    void FixedUpdate()
    {   // Stops tracking within certain distance of player
        if (Vector3.Distance(transform.position, player.position) <= trackingDistance)
        {
            tracking = false;
        }
        Move();
        CheckOutOfBounds(Time.deltaTime);
    }

    // Moves the projectile
    public new void Move()
    {
        if (tracking)
        {
            Vector3 directionToTarget = (player.position - transform.position).normalized;
            //Quaternion rotation = Quaternion.LookRotation(directionToTarget);
            //rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotation, trackingStrength));
            rb.velocity = directionToTarget * speed;
        }
    }

    // Deletes the projectile if it has existed for a certain amount of time
    private void CheckOutOfBounds(float dt)
    {
        hangTime += dt;
        if (hangTime > range)
        {
            Destroy(gameObject);
        }
    }
}
