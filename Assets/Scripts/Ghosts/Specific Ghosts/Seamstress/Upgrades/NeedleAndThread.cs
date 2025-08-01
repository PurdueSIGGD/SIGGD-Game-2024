using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedleAndThread : Skill
{
    [SerializeField] float effectiveRadius;
    [SerializeField] GameObject projectile;
    [SerializeField] float projectileSpeed;
    [SerializeField] float debuffDuration;
    private SeamstressManager manager;

    void Start()
    {
        manager = GetComponent<SeamstressManager>();
        GameplayEventHolder.OnAbilityUsed += ShootNeedlesOnGainSpools;
    }

    private void OnDisable()
    {
        GameplayEventHolder.OnAbilityUsed -= ShootNeedlesOnGainSpools;
    }

    private void ShootNeedlesOnGainSpools(ActionContext context)
    {
        if (context.actionID == ActionID.SEAMSTRESS_BASIC && context.extraContext.Equals("Gained Spool"))
        {
            foreach (Collider2D collider in Physics2D.OverlapCircleAll(transform.position, effectiveRadius, LayerMask.GetMask("Enemy")))
            {
                NeedleAndThreadProjectile projectileRef = Instantiate(projectile, PlayerID.instance.transform.position, PlayerID.instance.transform.rotation).GetComponent<NeedleAndThreadProjectile>();
                projectileRef.Init(collider.gameObject, projectileSpeed, 5.0f, GetPoints() * 20 + 5, debuffDuration);
            }
        }
    }

    public override void AddPointTrigger() { }
    public override void ClearPointsTrigger() { }
    public override void RemovePointTrigger() { }
}
