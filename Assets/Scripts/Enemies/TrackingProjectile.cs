using UnityEditor.Rendering;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

/// <summary>
/// Derivative of a an Enemy Projectile. Will track the target until a certain distance
/// away from it. Tracking Distance dictates how far away will the projectile loose tracking.
/// Tracking Projectile's range is measured in time, meaning a projectile existing for a
/// time longer than range will be deleted.
/// </summary>
public class TrackingProjectile : EnemyProjectile
{
    [SerializeField] float trackingStrength; // A larger value will allow the projectile to turn faster.
    [SerializeField] float trackingDistance; // A larger value will lead the projectile to loose tracking much earlier
    private float hangTime = 0;
    private bool tracking = true;


    private void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, target.position) <= trackingDistance)
        {
            tracking = false;
        }
        Move();
        CheckOutOfBounds(Time.deltaTime);
    }

    // Moves the projectile
    private new void Move()
    {
        if (tracking)
        {
            Quaternion rotation = Quaternion.LookRotation(target.position - transform.position);
            rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotation, trackingStrength));
        }
        rb.velocity = transform.forward * speed;
    }

    // Deletes the projectile if it has existed for a time longer than
    // the range field.
    private void CheckOutOfBounds(float dt)
    {
        hangTime += dt;
        if (hangTime > range)
        {
            Destroy(gameObject);
        }
    }
}
