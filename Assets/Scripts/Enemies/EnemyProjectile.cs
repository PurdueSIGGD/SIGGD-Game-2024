using UnityEngine;

/// <summary>
/// Dictates Enemy projectile behavior based on given speed.
/// To be attached to projectile prefabs, projectile must be instantiated within a RigidBody/Collider.
/// </summary>
public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] protected float speed; // Speed of the projectile
    [SerializeField] protected float damage; // Damage of the projectile

    [SerializeField] protected float range = Screen.width; // Range of the projectile, defaults to the bounds of the camera.


    protected Transform target; // Target location at the time of releasing the projectile
    protected Vector3 dir;
    protected Collider2D c;
    protected Rigidbody2D rb;
    protected Vector3 bounds;

    private void Start()
    {
        c = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        c.isTrigger = true;
        target = GameObject.FindGameObjectWithTag("Player").transform;
        dir = (target.position - transform.position).normalized;
        //transform.LookAt(target.position);
        bounds = dir * range + transform.position;
    }

    private void FixedUpdate()
    {
        Move();
        CheckOutOfBounds();
    }

    /// <summary>
    /// Initialize the Projectile. Use this after instantiating an EnemyProjectile.
    /// </summary>
    /// <param ghostName="target"> Transform containing the position of the target location. </param>
    public void Init(Transform target)
    {
        print(target.parent.name);
        this.target = target;
    }

    // Allow for projectile to collide with bodies after it has exited
    // the shooter body.
    private void OnTriggerExit2D(Collider2D other)
    {
        c.isTrigger = false;
    }
    // TODO Handle standard collisions.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }

    protected void Move()
    {
        rb.velocity = dir * speed;
    }

    // Destroys the projectile if it goes out of range.
    protected void CheckOutOfBounds()
    {
        if ((transform.position.x - bounds.x <= 0) == (dir.x <= 0) ||
            (transform.position.y - bounds.y <= 0) == (dir.y <= 0))
        {
            Destroy(gameObject);
        }
    }
}
