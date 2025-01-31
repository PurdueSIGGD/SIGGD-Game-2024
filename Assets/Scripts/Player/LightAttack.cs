using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// This is the script that contains the code form the player's melee attack
/// </summary>
public class LightAttack : MonoBehaviour
{
    [SerializeField] float range; // radius of the attack cone
    [SerializeField] float angle; // angle of the attack cone
    [SerializeField] DamageContext lightDamage;
    [SerializeField] float cooldown = 1; // Cooldown of player attack
    [SerializeField] LayerMask attackMask;
    
    private float damage;
    private float cooldown_cur = 0; // Current timer
    private Stats stats;
    private int counter = 0;
    private Camera mainCamera;
    
    private void Start()
    {
        //Stats grab
        stats = GetComponent<Stats>();
        damage = stats.ComputeValue("Damage");

        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    /// <summary>
    /// A function that gets called whenever the "Hit" action in Player Actions (currently mouse left)
    /// </summary>
    //private void OnHit()
    //{
    //    if (cooldown < cooldown_cur)
    //    { // If cooldown is over
    //        Attack();
    //    }
    //}

    /// <summary>
    /// Checks sword collision with objects
    /// If Enemy tag then hits with sword damage
    /// </summary>
    public void StartLightAttack()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, range, attackMask);

        Vector2 orig = transform.position;
        Vector2 center = mainCamera.ScreenToWorldPoint(Input.mousePosition).normalized * range; // furtherest point of player attack cone

        DrawCone(center);

        foreach (Collider2D hit in hits)
        {
            IDamageable damageableComponent = hit.gameObject.GetComponent<IDamageable>();
            // if obj cannot be damaged, forget it..
            if (damageableComponent == null)
            {
                continue;
            }
            Vector2 hitPos = hit.transform.position;
            Vector2 a = hitPos - orig;
            Vector2 b = center - orig;

            float hitAngle = Vector2.Dot(a, b) / (a.magnitude * b.magnitude);

            if (hitAngle <= angle / 2)
            {
                damageableComponent.Damage(lightDamage, gameObject);
            }
        }
    }

    private void DrawCone(Vector2 center)
    {
        Vector2 orig = transform.position;
        float halfAngle = (angle / 2) * Mathf.Deg2Rad;

        Vector2 a = new Vector2(center.x * Mathf.Cos(halfAngle) - center.y * Mathf.Sin(halfAngle), 
                                center.x * Mathf.Sin(halfAngle) + center.y * Mathf.Cos(halfAngle));

        Vector2 b = new Vector2(center.x * Mathf.Cos(halfAngle) + center.y * Mathf.Sin(halfAngle),
                                -center.x * Mathf.Sin(halfAngle) + center.y * Mathf.Cos(halfAngle));

        Debug.DrawLine(orig, center, Color.white, 1.0f);
        Debug.DrawLine(orig, a, Color.white, 1.0f);
        Debug.DrawLine(orig, b, Color.white, 1.0f);
    }
}
