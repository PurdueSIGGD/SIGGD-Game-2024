using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;

public class MageLightningAttack : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    protected Vector2 attackPosition;   // where the attack will be created
    protected float attackRadius;   // the size of the lightning
    protected DamageContext damageContext;  // Note: if you want to modify lightning damage context, go to Mage enemy prefab
    protected float chargeDuration = 1;   // how many seconds before damage is applied
    protected GameObject sourceMage;  // reference to the Mage who casted this attack


    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Sets all instance variables and updates position (similar to a constructor)
    // Invoke this method right after instantiating this GameObject
    public void Initialize(Vector2 attackPosition, float attackRadius, DamageContext damageContext, float chargeDuration, GameObject sourceMage)
    {
        this.attackPosition = attackPosition;
        this.attackRadius = attackRadius;
        this.damageContext = damageContext;
        this.chargeDuration = chargeDuration;
        this.sourceMage = sourceMage;
        
        transform.position = attackPosition;  // update position

        UpdateSpriteSize();  // update the sprite size
    }

    // Starts the charging animation and the attack as a coroutine
    // Make sure to call this only after Initialize() has been called
    public void StartCharging()
    {
        animator.Play("Charging");

        StartCoroutine(Attack(chargeDuration));
    }

    
    // Waits for a certain number of seconds and actually applies the attack
    // If you want to activate this attack, invoke StartCharging() instead of this method
    private IEnumerator Attack(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        // Check for player to do damage
        Collider2D hit = Physics2D.OverlapCircle(attackPosition, attackRadius, LayerMask.GetMask("Player"));
        if (hit)
        {
            //hit.GetComponent<PlayerHealth>().TakeDamage(damage);
            
            //Debug.Log("HIT");
            hit.GetComponent<Health>().Damage(damageContext, sourceMage);
        }

        Destroy(gameObject);
    }

    // Draws the size of the area attack
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }

    // Changes SpriteRenderer component's variable "size" so that the area circle reflect the real size of the attack
    // This method only works correctly if the sprite's bounds are an exact fit for the circle (i.e. don't change the sprite for now)
    private void UpdateSpriteSize()
    {
        Vector2 spriteWorldSize = spriteRenderer.sprite.bounds.size;
        float scaleFactor = (2 * attackRadius) / spriteWorldSize.x;

        spriteRenderer.size = spriteWorldSize * scaleFactor;
    }
}
