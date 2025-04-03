using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;

public class MageLightningAttack : MonoBehaviour
{
    [SerializeField] private float ringSpinSpeed = 2f;
    [SerializeField] private float particlesDuration = 2f;
    [SerializeField] private ParticleSystem particleSys;
    
    private Animator animator;
    [SerializeField] private GameObject ringGameObject;
    private SpriteRenderer ringSpriteRenderer;

    private Vector2 attackPosition;   // where the attack will be created
    private float attackRadius;   // the size of the lightning
    private DamageContext damageContext;  // Note: if you want to modify lightning damage context, go to Mage enemy prefab
    private float chargeDuration = 1;   // how many seconds before damage is applied
    private GameObject sourceMage;  // reference to the Mage who casted this attack


    private void Awake()
    {
        animator = GetComponent<Animator>();
        ringSpriteRenderer = ringGameObject.GetComponent<SpriteRenderer>();
        //spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if (ringGameObject != null)
        {
            ringGameObject.transform.Rotate(0, 0, ringSpinSpeed);
        }
    }

    // Sets all instance variables and updates position (similar to a constructor)
    // Invoke this method right after instantiating this GameObject
    public void Initialize(Vector2 attackPosition, float attackRadius, DamageContext damageContext, float chargeDuration, GameObject sourceMage)
    {
        //Debug.Log("Mage attack init");
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

        //Debug.Log("starting charging");

        StartCoroutine(Attack(chargeDuration));
    }

    
    // Waits for a certain number of seconds and actually applies the attack
    // If you want to activate this attack, invoke StartCharging() instead of this method
    private IEnumerator Attack(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        //Debug.Log("actual attack");

        // Making the lightning ring white as a VFX
        ringSpriteRenderer.color = Color.white;

        if (particleSys != null)
        {
            particleSys.Play();
        }

        // Check for player to do damage
        Collider2D hit = Physics2D.OverlapCircle(attackPosition, attackRadius, LayerMask.GetMask("Player"));
        if (hit)
        {
            hit.GetComponent<Health>().Damage(damageContext, sourceMage);
            
            //Debug.Log("HIT");
            //hit.GetComponent<Health>().Damage(damageContext, sourceMage);
        }

        StartCoroutine(SelfDestruct(particlesDuration));
    }

    private IEnumerator SelfDestruct(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        //Debug.Log("actual destruct");

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
        Debug.Log("Update sprite size");
        Vector2 spriteWorldSize = ringSpriteRenderer.sprite.bounds.size;
        float scaleFactor = (2 * attackRadius) / spriteWorldSize.x;

        ringSpriteRenderer.size = spriteWorldSize * scaleFactor;
    }
}
