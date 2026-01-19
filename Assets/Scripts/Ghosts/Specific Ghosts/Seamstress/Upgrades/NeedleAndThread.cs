using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedleAndThread : Skill
{
    [SerializeField]
    List<float> values = new List<float>
    {
        0f, 0.5f, 1.0f, 1.5f, 2.0f
    };
    private int pointIndex = 0;

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
        if (pointIndex <= 0) return;
        if (context.actionID == ActionID.SEAMSTRESS_BASIC && context.extraContext != null && context.extraContext.Equals("Gained Spool"))
        {
            Vector2 playerPos = PlayerID.instance.transform.position;
            NeedleThreadParticles threadParticles = Instantiate(NeedleAndThreadParticleSystem, playerPos, transform.rotation).GetComponent<NeedleThreadParticles>();
            threadParticles.Init(Physics2D.OverlapCircleAll(playerPos, effectiveRadius, LayerMask.GetMask("Enemy")), Mathf.FloorToInt(values[pointIndex]), values[pointIndex]);
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
}
