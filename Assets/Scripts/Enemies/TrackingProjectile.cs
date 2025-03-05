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


    void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, target.position) <= trackingDistance)
        {
            tracking = false;
        }
        Move();
        CheckOutOfBounds(Time.deltaTime);
    }

    // Moves the projectile
    protected new void Move()
    {
        if (tracking)
        {
            Quaternion rotation = Quaternion.LookRotation(target.position - transform.position);
            rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotation, trackingStrength));
        }
        rb.velocity = transform.forward * speed;
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
