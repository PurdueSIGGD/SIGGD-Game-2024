using System.Collections;
using UnityEngine;

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
    private GameObject sourceMage;  // reference to the Mage who casted this attack
    bool followPlayer;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        ringSpriteRenderer = ringGameObject.GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if (ringGameObject != null)
        {
            ringGameObject.transform.Rotate(0, 0, ringSpinSpeed);
        }
        // follow player
        if (followPlayer && PlayerID.instance.gameObject)
        {
            transform.position = PlayerID.instance.gameObject.transform.position;
        }
    }

    // Sets all instance variables and updates position (similar to a constructor)
    // Invoke this method right after instantiating this GameObject
    public void Initialize(Vector2 attackPosition, float attackRadius, DamageContext damageContext, GameObject sourceMage)
    {
        this.attackPosition = attackPosition;
        this.attackRadius = attackRadius;
        this.damageContext = damageContext;
        this.sourceMage = sourceMage;
        followPlayer = true;

        transform.position = attackPosition;  // update position

        UpdateSpriteSize();  // update the sprite size
    }

    public void StopFollow()
    {
        followPlayer = false;
        ringSpriteRenderer.color = Color.red;
    }

    public void LightningPhase()
    {
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
        }
    }
    public void Fizzle()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// Trigger an automatic sequence if mage dies while spell is active
    /// </summary>
    public void MageDeathHandler()
    {
        StartCoroutine(MageDeathHandlerCoroutine());
    }
    IEnumerator MageDeathHandlerCoroutine()
    {
        StopFollow();
        float flickerTime = 1;
        int numFlickers = 20;
        for (int i = 0; i < numFlickers; i++)
        {
            ringSpriteRenderer.color = i % 2 == 0 ? Color.red : Color.white;
            yield return new WaitForSeconds(flickerTime / numFlickers);
        }
        LightningPhase();
        yield return new WaitForSeconds(0.5f);
        Fizzle();
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
