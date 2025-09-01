using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedleAndThread : Skill
{
    [SerializeField] GameObject NeedleAndThreadParticleSystem;
    [SerializeField] float effectiveRadius;
    [SerializeField] float debuffDuration;

    void Start()
    {
        GameplayEventHolder.OnAbilityUsed += ShootNeedlesOnGainSpools;
    }

    private void OnDisable()
    {
        GameplayEventHolder.OnAbilityUsed -= ShootNeedlesOnGainSpools;
    }

    private void ShootNeedlesOnGainSpools(ActionContext context)
    {
        if (context.actionID == ActionID.SEAMSTRESS_BASIC && context.extraContext != null && context.extraContext.Equals("Gained Spool"))
        {
            Vector2 playerPos = PlayerID.instance.transform.position;
            NeedleThreadParticles threadParticles = Instantiate(NeedleAndThreadParticleSystem, playerPos, transform.rotation).GetComponent<NeedleThreadParticles>();
            threadParticles.Init(Physics2D.OverlapCircleAll(playerPos, effectiveRadius, LayerMask.GetMask("Enemy")), GetPoints() * 20 + 5, debuffDuration);
        }
    }

    public override void AddPointTrigger() { }
    public override void ClearPointsTrigger() { }
    public override void RemovePointTrigger() { }
}
