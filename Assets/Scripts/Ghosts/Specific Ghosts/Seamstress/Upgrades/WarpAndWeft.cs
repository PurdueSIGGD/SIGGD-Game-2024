
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

    [SerializeField] private GameObject pulseVFX;

    void OnEnable()
    {
        GameplayEventHolder.OnEntityStunned += DamageStunnedEnemies;
    }

    void OnDisable()
    {
        GameplayEventHolder.OnEntityStunned -= DamageStunnedEnemies;
    }

    public void DamageStunnedEnemies(GameObject stunnedEntity)
    {
        if (GetPoints() > 0 && stunnedEntity.GetComponent<FateboundDebuff>() != null)
        {
            warpDmgContext.damage = values[GetPoints()];
            stunnedEntity.GetComponent<Health>().Damage(warpDmgContext, PlayerID.instance.gameObject);
            GameObject pulse = Instantiate(pulseVFX, transform.position, Quaternion.identity);
            pulse.GetComponent<RingExplosionHandler>().playRingExplosion(3f, GetComponent<GhostIdentity>().GetCharacterInfo().primaryColor);
        }
    }

    public void DamageFateboundEnemies(GameObject fateboundEnemy)
    {
        if (GetPoints() > 0 && fateboundEnemy.GetComponent<EnemyStateManager>().StunState.isStunned)
        {
            warpDmgContext.damage = values[GetPoints()];
            fateboundEnemy.GetComponent<Health>().Damage(warpDmgContext, PlayerID.instance.gameObject);
            GameObject pulse = Instantiate(pulseVFX, transform.position, Quaternion.identity);
            pulse.GetComponent<RingExplosionHandler>().playRingExplosion(3f, GetComponent<GhostIdentity>().GetCharacterInfo().primaryColor);
        }
    }

    public override void AddPointTrigger() { }
    public override void RemovePointTrigger() { }
    public override void ClearPointsTrigger() { }
}
