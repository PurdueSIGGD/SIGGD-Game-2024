using UnityEngine;

/// <summary>
/// Propels an Enemy projectile towards the player.
/// To be attached to projectile prefabs.
/// </summary>
public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] protected float speed; // Speed of the projectile
    [SerializeField] public DamageContext projectileDamage; // Damage of the projectile

    [SerializeField] protected float range = Screen.width; // Range of the projectile, defaults to the bounds of the camera.

    public string target = "Player";

    //protected Transform target; // Target location at the time of releasing the projectile
    protected Vector3 dir;
    protected Rigidbody2D rb;
    protected Vector3 bounds;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //target = GameObject.FindGameObjectWithTag("Player").transform;

        //dir = (target.position - transform.position).normalized;
        //bounds = dir * range + transform.position;
    }

    void FixedUpdate()
    {
        Move();
        CheckOutOfBounds();
    }

    /// <summary>
    /// Initialize the projectile with a target location and a damage value
    /// </summary>
    /// <param name="target"> transform of the target object </param>
    /// <param name="damage"> damage of the projectile </param>
    public void Init(Vector3 target, float damage)
    {
        projectileDamage.damage = damage;
        dir = (target - transform.position).normalized;
        bounds = dir * range + transform.position;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(target))
        {
            collision.gameObject.GetComponent<Health>().Damage(projectileDamage, gameObject);
        }

        if (collision.gameObject.CompareTag(target) || collision.gameObject.CompareTag("Enviroment"))
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
            Debug.Log("outta bouds");
        }
    }

    public void SwitchDirections()
    {
        dir = new Vector3(-dir.x, -dir.y, 0);
        bounds = dir * range + transform.position;
    }
}
