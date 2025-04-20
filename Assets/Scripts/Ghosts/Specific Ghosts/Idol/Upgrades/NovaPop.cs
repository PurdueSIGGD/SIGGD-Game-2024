using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class NovaPop : Skill
{
    [SerializeField] private DamageContext explosionContext;
    [SerializeField] private DamageContext stunContext;
    [SerializeField] private float radius;
    private GameObject playerRef;
    private StatManager stat;
    private IdolManager manager;

    void Start()
    {
        playerRef = PlayerID.instance.gameObject;
        GameplayEventHolder.OnDeath += ExplodeOnDeath;

        stat = GetComponent<StatManager>();
        manager = GetComponent<IdolManager>();
    }

    public void ExplodeOnDeath(DamageContext context)
    {
        if (context.victim.CompareTag("Idol_Clone"))
        {
            GameObject explosion = Instantiate(manager.explosionVFX, context.victim.transform.position, Quaternion.identity);
            explosion.GetComponent<RingExplosionHandler>().playRingExplosion(radius, manager.GetComponent<GhostIdentity>().GetCharacterInfo().primaryColor);

            Collider2D[] hits = Physics2D.OverlapCircleAll(context.victim.transform.position, radius);
            foreach (Collider2D hit in hits)
            {
                if (hit.transform.gameObject.CompareTag("Enemy"))
                {
                    explosionContext.damage = stat.ComputeValue("Nova Pop Damage");
                    hit.transform.gameObject.GetComponent<Health>().Damage(explosionContext, playerRef);
                    hit.GetComponent<EnemyStateManager>().Stun(stunContext, 0.6f * skillPts);
                }
            }
        }
    }


    public override void AddPointTrigger()
    {
    }

    public override void ClearPointsTrigger()
    {
    }

    public override void RemovePointTrigger()
    {
    }
}
