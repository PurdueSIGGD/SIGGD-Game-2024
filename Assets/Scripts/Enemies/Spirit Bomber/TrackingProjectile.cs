using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor.Rendering;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

/// <summary>
/// A type of a an Enemy Projectile. Will track the player until a certain distance
/// away from it.
/// </summary>
public class TrackingProjectile : EnemyProjectile
{
    [SerializeField] float trackingStrength; // A larger value will allow the projectile to turn faster.
    [SerializeField] float trackingDistance; // A larger value will lead the projectile to loose tracking earlier
    private float hangTime = 0;
    private bool tracking = true;
    protected Transform player;
    private StatManager stats;

    //Consistently tracks player
    protected virtual void Awake()
    {
        stats = this.GetComponent<StatManager>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        GameObject enemy = GameObject.FindWithTag("Enemy");
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), enemy.GetComponent<Collider2D>());
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
    public void Move()
    {
            if (tracking)
            {
            Vector3 directionToTarget = (player.position - transform.position).normalized;
            Quaternion rotation = Quaternion.LookRotation(directionToTarget);
            rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotation, trackingStrength));
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
