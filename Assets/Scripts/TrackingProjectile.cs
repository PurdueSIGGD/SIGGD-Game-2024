using UnityEngine;
using static UnityEngine.GraphicsBuffer;

/// <summary>
/// Derivative of a an Enemy Projectile. Will track the target until a certain distance
/// away from it. Tracking Strength dictates how far away will the projectile loose tracking.
/// Tracking Projectile's range is measured in time, meaning a projectile existing for a
/// time longer than range will be deleted.
/// </summary>
public class TrackingProjectile : EnemyProjectile
{
    [SerializeField] float trackingStrength; // A larger value will lead the projectile to loose tracking much earlier
    private float hangTime = 0;
    private bool tracking = true;

    private void Update()
    {
        if (Vector3.Distance(transform.position, target.position) <= trackingStrength)
        {
            tracking = false;
        }
        Move();
    }

    // Moves the projectile
    private new void Move()
    {
        if (tracking)
        {
            dir = (target.position - transform.position).normalized;
            transform.LookAt((Vector2)target.position);
        }
        transform.position += dir * speed * Time.deltaTime;
        CheckOutOfBounds(Time.deltaTime);
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
