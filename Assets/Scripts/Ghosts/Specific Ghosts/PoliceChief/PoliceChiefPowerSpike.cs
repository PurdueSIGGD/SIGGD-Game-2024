using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class PoliceChiefPowerSpike : Skill
{
    private float timer = -1.0f;
    private bool critHit = false;
    public bool ableToCrit = false;
    [SerializeField] private float[] pointCounts = {1.25f, 1.45f, 1.65f, 1.85f};
    private float currentBuff = -1.0f;
    [SerializeField] private float immediateTimeframe = 0.25f;
    private bool reset = true;
    public override void AddPointTrigger()
    {
        currentBuff = pointCounts[GetPoints() - 1];
    }
    public override void RemovePointTrigger()
    {

    }
    public override void ClearPointsTrigger()
    {
        currentBuff = -1.0f;
    }

    void Start()
    {
        AddPoint();
        AddPoint();
        GameplayEventHolder.OnAbilityUsed += OnAbilityUse;
        GameplayEventHolder.OnDamageFilter.Add(OnDamage);
    }

    void Update()
    {
        if(timer > 0.0f)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            ableToCrit = false;
        }

        string currentAnim = PlayerID.instance.gameObject.GetComponent<PlayerStateMachine>().currentAnimation;
        if (reset && currentBuff > 0 && currentAnim.Equals("sidearm_primed") && timer < 0f)
        {
            timer = immediateTimeframe;
            ableToCrit = true;
            reset = false;
        }

        if (currentBuff > 0 && currentAnim.Equals("player_idle"))
        {
            reset = true;
        }
    }

    void OnAbilityUse(ActionContext context)
    {
        if (timer > 0.0f)
        {
            critHit = true;
        }
        else
        {
            critHit = false;
        }
    }

    void OnDamage(ref DamageContext context)
    {
        if (currentBuff != -1.0f && critHit && context.actionID == ActionID.POLICE_CHIEF_BASIC)
        {
            critHit = false;
            context.damage *= currentBuff;
        }
    }
}
