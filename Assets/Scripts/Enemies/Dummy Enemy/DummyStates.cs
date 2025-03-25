using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Enemy AI for the Dummy Enemy
/// </summary>

// This class can be consultated as a template for a basic Enemy
public class DummyStates : EnemyStateManager, IDamageable
{
    [SerializeField] protected Transform meleeTrigger; // Area in which enemy will attempt to Dummy Slash
    [SerializeField] protected Transform rangeTrigger; // Area in which enemy will attempt to Dummy Shoot
    [SerializeField] protected Transform rangeOrig; // Location where Dummy Shoot's Projectile will spawn

    private int meleeDamage = 0; // Melee Damage of Enemy --> For 'Slash Action'
    //private int rangeDamage = 0; // Range Damage of Enemy --> For 'Shoot Action'
    private float damageReduction;
    private float health;
    public GameObject projectile; 

    void Start()
    {
        // StatManager grab
        stats = GetComponent<StatManager>();
        health = (int) stats.ComputeValue("Health"); 
        damageReduction = stats.ComputeValue("Armor");  
        meleeDamage = (int) stats.ComputeValue("Melee Damage");
        //rangeDamage = (int) stats.ComputeValue("Range Damage");

        if (damageReduction < 0 || damageReduction > 1)
        {
            Debug.LogWarning($"Enemy {name} has an invalid damage reduction {damageReduction}, must be between 0 and 1");
            damageReduction = Mathf.Clamp01(damageReduction);
        }
    }

    // These are events to be bound to the animations

    // Instantiates a projectile during Dummy Shoot
    protected void OnShootEvent()
    {
        GameObject proj_i = Instantiate(projectile, rangeOrig.position, transform.rotation);
        //proj_i.GetComponent<EnemyProjectile>().damage = rangeDamage;
    }
    // Checks for collision with Player during Dummy Shoot.
    protected void OnSlashEvent()
    {
        Collider2D hit = Physics2D.OverlapBox(meleeTrigger.position, meleeTrigger.lossyScale, 0f, LayerMask.GetMask("Player"));
        if (hit) {
            TakeDamage(meleeDamage);
        }
    }

    public void TakeDamage(float damage) {
        
        if(damage <= 0) {
            print("Negative damage is not supported, unless it's really cool then maybe");
        }
    
        damage = Mathf.RoundToInt(damage * (1 - damageReduction));
        health -= (int) damage;
        print("Enemy took damage: " + health);
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    // Draws the Enemy attack range in the editor
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireCube(meleeTrigger.position, meleeTrigger.lossyScale);
        Gizmos.DrawWireCube(rangeTrigger.position, rangeTrigger.lossyScale);
        Gizmos.DrawWireSphere(rangeOrig.position, 0.1f);
    }

    public float Damage(DamageContext context, GameObject attacker)
    {
        return 0f;
    }

    public float Heal(HealingContext context, GameObject healer)
    {
        return 0f;
    }
}
