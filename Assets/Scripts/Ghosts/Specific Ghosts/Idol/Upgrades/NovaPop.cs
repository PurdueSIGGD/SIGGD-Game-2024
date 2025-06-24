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
    private static int pointIndex;

    void Start()
    {
        playerRef = PlayerID.instance.gameObject;

        stat = GetComponent<StatManager>();
        manager = GetComponent<IdolManager>();
        manager.evaSelectedEvent.AddListener(EvaSelected);
        manager.evaDeselectedEvent.AddListener(EvaDeselected);
    }

    private void OnEnable()
    {
        GameplayEventHolder.OnDeath += ExplodeOnDeath;
    }

    private void OnDisable()
    {
        GameplayEventHolder.OnDeath -= ExplodeOnDeath;
    }

        public void ExplodeOnDeath(DamageContext context)
    {
        if (context.victim.CompareTag("Idol_Clone") && pointIndex > 0)
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
                    hit.GetComponent<EnemyStateManager>().Stun(stunContext, 0.6f * pointIndex);
                }
            }
        }
    }


    public override void AddPointTrigger()
    {
        pointIndex = GetPoints();
    }

    public override void ClearPointsTrigger()
    {
        pointIndex = GetPoints();
    }

    public override void RemovePointTrigger()
    {
        pointIndex = GetPoints();
    }

    private void EvaSelected()
    {
        manager.passive.avaliableCloneLostVA.Add("Eva-Idol Nova Pop");
    }

    private void EvaDeselected()
    {
        manager.passive.avaliableCloneLostVA.Remove("Eva-Idol Nova Pop");
    }
}
