using UnityEngine;

/// <summary>
/// Dictates Enemy projectile behavior based on given speed.
/// To be attached to projectile prefabs. Projectile must be instantiated within a RigidBody/Collider.
/// </summary>
public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] float speed; // Speed of the projectile
    [SerializeField] float damage; // Damage of the projectile
    [SerializeField] Collider collide; // Collider that is attached to projectile

    [SerializeField] float range = Screen.width; // Range of the projectile, defaults to the bounds of the camera.
    [SerializeField] bool canFriendlyDamage = false; // Toggle to allow for friendly fire
    [SerializeField] bool canPassWalls = false; // Toggle to allow for projectile to pass through environment

    private Transform target; // Target location at the time of releasing the projectile
    private Vector3 dir;
    private Vector2 bounds;

    private void Start()
    {
        collide.isTrigger = true;
        dir = (target.position - transform.position).normalized;

        bounds = dir * range + transform.position;
    }

    private void Update()
    {
        transform.position += dir * speed * Time.deltaTime;
        checkOutOfBounds();
    }

    /// <summary>
    /// Initialize the Projectile. Use this after instantiating an EnemyProjectile.
    /// </summary>
    /// <param name="target"> Transform containing the position of the target location. </param>
    public void init(Transform target)
    {
        this.target = target;
    }
    /// <summary>
    /// Initialize the Projectile with custom attributes. Use this after instantiating an EnemyProjectile.
    /// </summary>
    /// <param name="target"> Transform containing the position of the target location. </param>
    /// <param name="speed"> Speed of the projectile. </param>
    /// <param name="damage"> Damage of the projectile. </param>
    /// <param name="range"> Range of the projectile before it expires. </param>
    public void init(Transform target, float speed, float damage, float range)
    {
        this.target = target;
        this.speed = speed;
        this.damage = damage;
        this.range = range;
    }

    // Allow for projectile to collide with bodies after it has exited
    // the shooter body.
    private void OnTriggerExit(Collider other)
    {
        collide.isTrigger = false;
    }
    // TODO Handle standard collisions.
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
    // Destroys the projectile if it goes out of range.
    private void checkOutOfBounds()
    {
        if ((transform.position.x - bounds.x <= 0) == (dir.x <= 0) ||
            (transform.position.y - bounds.y <= 0) == (dir.y <= 0))
        {
            Destroy(gameObject);
        }
    }
}
