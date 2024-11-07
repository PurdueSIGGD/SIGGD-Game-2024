using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Enemy AI for the Dummy Enemy
/// </summary>

// This class can be consultated as a template for a basic Enemy
public class DummyStates : EnemyStateManager
{
    [SerializeField] protected Transform meleeHit; // Area in which enemy will attempt to Dummy Slash
    [SerializeField] protected Transform rangeHit; // Area in which enemy will attempt to Dummy Shoot
    [SerializeField] protected Transform rangeOrig; // Location where Dummy Shoot's Projectile will spawn

    [SerializeField] protected int meleeDamage = 0; // Melee Damage of Enemy --> For 'Slash Action'
    [SerializeField] protected int rangeDamage = 0; // Range Damage of Enemy --> For 'Shoot Action'

    public GameObject projectile;

    // Dummy Enemy's all possible actions
    protected override ActionPool GenerateActionPool()
    {
        // Declare the Enemy's attacks
        Action dummySlash = new(meleeHit, 4.0f, 3f, "HeroKnight_Attack1");
        Action dummyShoot = new(rangeHit, 4.0f, 7f, "Dummy_Shoot");
        
        // Most Enemy should also add their Run and Idle animations to the ActionPool
        Action move = new(null, 0.0f, 0.0f, "HeroKnight_Run");
        Action idle = new(null, 0.0f, 0.0f, "HeroKnight_Idle");

        return new ActionPool(new List<Action> { dummySlash, dummyShoot }, move, idle);
    }

    // These are events to be bound to the animations

    // Instantiates a projectile during Dummy Shoot
    protected void OnShootEvent()
    {
        GameObject proj_i = Instantiate(projectile, rangeOrig.position, transform.rotation);
        proj_i.GetComponent<EnemyProjectile>().damage = rangeDamage;
    }
    // Checks for collision with Player during Dummy Shoot.
    protected void OnSlashEvent()
    {
        Collider2D hit = Physics2D.OverlapBox(meleeHit.position, meleeHit.lossyScale, 0f, LayerMask.GetMask("Player"));
        if (hit) {
            hit.GetComponent<PlayerHealth>().TakeDamage(meleeDamage);
        }
    }

    // Draws the Enemy attack range in the editor
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireCube(meleeHit.position, meleeHit.lossyScale);
        Gizmos.DrawWireCube(rangeHit.position, rangeHit.lossyScale);
        Gizmos.DrawWireSphere(rangeOrig.position, 0.1f);
    }
}
