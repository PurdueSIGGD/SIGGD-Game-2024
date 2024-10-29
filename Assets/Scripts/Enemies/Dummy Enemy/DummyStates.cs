using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class DummyStates : EnemyStateManager
{
    [SerializeField] protected Transform meleeHit;
    [SerializeField] protected Transform rangeHit;
    [SerializeField] protected Transform rangeOrig;

    public GameObject projectile;

    protected override ActionPool GenerateActionPool()
    {
        Action dummySlash = new(meleeHit, 4.0f, 3f, "HeroKnight_Attack1");
        Action dummyShoot = new(rangeHit, 4.0f, 7f, "Dummy_Shoot");
        Action move = new(null, 0.0f, 0.0f, "HeroKnight_Run");
        Action idle = new(null, 0.0f, 0.0f, "HeroKnight_Idle");

        return new ActionPool(new List<Action> { dummySlash, dummyShoot }, move, idle);
    }

    protected void OnShootEvent()
    {
        Instantiate(projectile, rangeOrig.position, transform.rotation);
    }
    protected void OnSlashEvent()
    {
        Collider2D hit = Physics2D.OverlapBox(meleeHit.position, meleeHit.lossyScale, 0f, LayerMask.GetMask("Player"));
        if (hit) {
            hit.GetComponent<PlayerHealth>().takeDamage(100);
        }
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireCube(meleeHit.position, meleeHit.lossyScale);
        Gizmos.DrawWireCube(rangeHit.position, rangeHit.lossyScale);
        Gizmos.DrawWireSphere(rangeOrig.position, 0.1f);
    }
}
