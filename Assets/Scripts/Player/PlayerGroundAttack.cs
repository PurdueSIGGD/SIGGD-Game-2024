using UnityEngine;

/// <summary>
/// This is the script that contains the code form the player's melee attack
/// </summary>
public class PlayerGroundAtack : MonoBehaviour
{
    [SerializeField] GameObject indicator; //The hitbox for the sword attack
    [SerializeField] float swordDist; //How far away the sword should rotate from the player
    [SerializeField] float cooldown = 1; // Cooldown of player attack
    [SerializeField] LayerMask attackMask;
    private float damage;
    float cooldown_cur = 0; // Current timer
    private StatManager stats;
    
    private int counter = 0;
    private Camera mainCamera;
    
    private void Start()
    {
        //StatManager grab
        stats = GetComponent<StatManager>();
        damage = stats.ComputeValue("Damage");

        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }
    private void Update()
    {
        cooldown_cur += Time.deltaTime;
        if (indicator.activeSelf)
        {
            if (counter <= 0) {
                indicator.SetActive(false);
                attack();
            }
            counter--;
        }

        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 subV = gameObject.transform.position - mousePos;
        float angle = Mathf.Atan2(subV.y, subV.x);
        Vector2 offset = new Vector2(-Mathf.Cos(angle), -Mathf.Sin(angle));
        indicator.transform.position = gameObject.transform.position + new Vector3(offset.x, offset.y, 0) * swordDist;
        indicator.transform.Rotate(0, 0, angle * 180/Mathf.PI - indicator.transform.eulerAngles.z);
    }
    /// <summary>
    /// Checks sword collision with objects
    /// If Enemy tag then hits with sword damage
    /// </summary>
    private void attack() {
        Collider2D[] collided = Physics2D.OverlapBoxAll(new Vector2(indicator.transform.position.x,indicator.transform.position.y), indicator.transform.localScale, indicator.transform.eulerAngles.z, attackMask);
        foreach(Collider2D collide in collided) {
            // Currently hits all enemies in range
           if (collide.gameObject.GetComponent<IDamageable>() != null) {
                //collide.GetComponent<IDamageable>().TakeDamage(damage);
           }
        }
    }

    /// <summary>
    /// A function that gets called whenever the "Hit" action in Player Actions (currently mouse left)
    /// </summary>
    private void OnHit()
    {
        if (cooldown < cooldown_cur) { // If cooldown is over
            indicator.SetActive(true);
            counter = 10;
            cooldown_cur = 0;
        }
        
    }
}
