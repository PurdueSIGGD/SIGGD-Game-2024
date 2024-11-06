using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// This is the script that contains the code form the player's melee attack
/// </summary>
public class PlayerGroundAtack : MonoBehaviour
{
    [SerializeField] GameObject indicator; //The hitbox for the sword attack
    [SerializeField] float swordDist; //How far away the sword should rotate from the player
    [SerializeField] int damage = 0; //Damage of Player
    [SerializeField] float cooldown = 1; // Cooldown of player attack
    float cooldown_cur = 0;
    
    private int counter = 0;
    private Camera mainCamera;
    
    private void Start()
    {
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
        Collider2D[] collided = Physics2D.OverlapBoxAll(new Vector2(indicator.transform.position.x,indicator.transform.position.y), indicator.transform.localScale, indicator.transform.eulerAngles.z);
        foreach(Collider2D collide in collided) {
           if (collide.transform.CompareTag("Enemy")) {
                collide.GetComponent<EnemyHealth>().TakeDamage(damage);
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
