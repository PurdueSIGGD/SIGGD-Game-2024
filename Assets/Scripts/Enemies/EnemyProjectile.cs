using UnityEngine;

/// <summary>
/// Propels an Enemy projectile towards the player.
/// To be attached to projectile prefabs.
/// </summary>
public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] protected float speed; // Speed of the projectile
    [SerializeField] public int damage = 0; // Damage of the projectile

    [SerializeField] protected float range = Screen.width; // Range of the projectile, defaults to the bounds of the camera.


    protected Transform target; // Target location at the time of releasing the projectile
    protected Vector3 dir;
    protected Rigidbody2D rb;
    protected Vector3 bounds;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").transform;

        dir = (target.position - transform.position).normalized;
        bounds = dir * range + transform.position;
    }

    void FixedUpdate()
    {
        Move();
        CheckOutOfBounds();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);
        }
        Destroy(gameObject);
    }

    // Moves the projectile according to speed.
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
