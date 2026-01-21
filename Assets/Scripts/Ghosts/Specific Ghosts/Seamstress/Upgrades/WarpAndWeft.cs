
using System.Collections.Generic;
using UnityEngine;

public class WarpAndWeft : Skill
{
    [SerializeField] DamageContext warpDmgContext;

    [SerializeField]
    private List<float> values = new List<float>
    {
        0, 10, 20, 30, 40
    };
    private int pointIndex = 0;

    [SerializeField] private GameObject pulseVFX;

    void OnEnable()
    {
        //GameplayEventHolder.OnEntityStunned += DamageStunnedEnemies;
        GameplayEventHolder.OnDamageFilter.Add(DamageBoost);
    }

    void OnDisable()
    {
        //GameplayEventHolder.OnEntityStunned -= DamageStunnedEnemies;
        GameplayEventHolder.OnDamageFilter.Remove(DamageBoost);
    }

    public void DamageStunnedEnemies(GameObject stunnedEntity)
    {
        /*
        if (GetPoints() > 0 && stunnedEntity.GetComponent<FateboundDebuff>() != null)
        {
            warpDmgContext.damage = values[GetPoints()];
            stunnedEntity.GetComponent<Health>().Damage(warpDmgContext, PlayerID.instance.gameObject);
            GameObject pulse = Instantiate(pulseVFX, transform.position, Quaternion.identity);
            pulse.GetComponent<RingExplosionHandler>().playRingExplosion(3f, GetComponent<GhostIdentity>().GetCharacterInfo().primaryColor);
        }
        */
    }

    public void DamageFateboundEnemies(GameObject fateboundEnemy)
    {
        /*
        if (GetPoints() > 0 && fateboundEnemy.GetComponent<EnemyStateManager>().StunState.isStunned)
        {
            warpDmgContext.damage = values[GetPoints()];
            fateboundEnemy.GetComponent<Health>().Damage(warpDmgContext, PlayerID.instance.gameObject);
            GameObject pulse = Instantiate(pulseVFX, transform.position, Quaternion.identity);
            pulse.GetComponent<RingExplosionHandler>().playRingExplosion(3f, GetComponent<GhostIdentity>().GetCharacterInfo().primaryColor);
        }
        */
    }

    public void DamageBoost(ref DamageContext context)
    {
        if (pointIndex <= 0) return;
        if (!context.victim.CompareTag("Enemy")) return;
        if ((context.victim.GetComponent<EnemyStateManager>() != null && context.victim.GetComponent<EnemyStateManager>().StunState.isStunned) ||
            context.victim.GetComponent<FateboundDebuff>() != null)
        {
            context.damage *= values[pointIndex];
            context.ghostID = GhostID.YUME;
        }
    }

    public override void AddPointTrigger() { pointIndex = GetPoints(); }
    public override void RemovePointTrigger() { pointIndex = GetPoints(); }
    public override void ClearPointsTrigger() { pointIndex = GetPoints(); }
}
