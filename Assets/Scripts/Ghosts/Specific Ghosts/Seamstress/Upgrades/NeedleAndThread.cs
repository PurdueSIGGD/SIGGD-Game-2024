using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedleAndThread : Skill
{
    [SerializeField] float effectiveRadius;
    [SerializeField] NeedleAndThreadProjectile projectile;
    private SeamstressManager manager;

    void Start()
    {
        manager = GetComponent<SeamstressManager>();
        GameplayEventHolder.OnAbilityUsed += ShootNeedlesOnGainSpools;
    }

    private void ShootNeedlesOnGainSpools(ActionContext context)
    {
        if (context.actionID == ActionID.SEAMSTRESS_BASIC && context.extraContext.Equals("Gained Spool"))
        {
            Physics2D.OverlapCircleAll(transform.position, effectiveRadius);
            NeedleAndThreadProjectile projectileRef = Instantiate(projectile, PlayerID.instance.transform.position, PlayerID.instance.transform.rotation);
            
        }
    }

    public override void AddPointTrigger() { }
    public override void ClearPointsTrigger() { }
    public override void RemovePointTrigger() { }
}
