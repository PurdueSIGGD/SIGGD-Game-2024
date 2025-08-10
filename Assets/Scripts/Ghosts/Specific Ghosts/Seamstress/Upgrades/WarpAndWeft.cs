
using UnityEngine;

public class WarpAndWeft : Skill
{
    [SerializeField] DamageContext warpDmgContext;

    void Start()
    {
        GameplayEventHolder.OnEntityStunned += DamageStunnedEnemies;
    }

    void OnDisable()
    {
        GameplayEventHolder.OnEntityStunned -= DamageStunnedEnemies;
    }

    private void DamageStunnedEnemies(GameObject stunnedEntity)
    {
        if (GetPoints() > 0 && stunnedEntity.GetComponent<FateboundDebuff>() != null)
        {
            warpDmgContext.damage = GetPoints() * 10;
            stunnedEntity.GetComponent<Health>().NoContextDamage(warpDmgContext, PlayerID.instance.gameObject);
        }
    }

    public override void AddPointTrigger() { }
    public override void RemovePointTrigger() { }
    public override void ClearPointsTrigger() { }
}
