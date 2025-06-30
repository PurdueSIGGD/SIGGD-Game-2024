using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class PoliceChiefPowerSpike : Skill
{
    private float timer = -1.0f;
    private bool critHit = false;
    private bool ableToCrit = false;
    [SerializeField] private float[] values = {0f, 1.25f, 1.45f, 1.65f, 1.85f};
    private static int pointIndex;
    [SerializeField] private float immediateTimeframe = 0.25f;
    private bool reset = true;
    //private PlayerStateMachine playerStateMachine;

    public override void AddPointTrigger()
    {
        pointIndex = GetPoints();
    }
    public override void RemovePointTrigger()
    {

    }
    public override void ClearPointsTrigger()
    {
        pointIndex = GetPoints();
    }

    void Start()
    {
        if (pointIndex <= 0) pointIndex = 0;
        //playerStateMachine = PlayerID.instance.GetComponent<PlayerStateMachine>();
    }

    private void OnEnable()
    {
        //GameplayEventHolder.OnAbilityUsed += OnAbilityUse;
        GameplayEventHolder.OnDamageFilter.Add(OnDamage);
    }

    void OnDisable()
    {
        //GameplayEventHolder.OnAbilityUsed -= OnAbilityUse;
        GameplayEventHolder.OnDamageFilter.Remove(OnDamage);
    }

    void Update()
    {
        if (timer > 0.0f)
        {
            timer -= Time.deltaTime;
            ableToCrit = true;
        }
        else if (ableToCrit)
        {
            AudioManager.Instance.SFXBranch.GetSFXTrack("North-Sidearm Attack").SetPitch(0.2f, 1f);
            ableToCrit = false;
        }

        /*
        string currentAnim = playerStateMachine.currentAnimation;
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
        */
    }

    public void StartCritTimer()
    {
        if (pointIndex <= 0) return;
        ableToCrit = true;
        timer = immediateTimeframe;
        GetComponent<PoliceChiefUIDriver>().basicAbilityUIManager.pingAbility();
        AudioManager.Instance.SFXBranch.GetSFXTrack("North-Sidearm Attack").SetPitch(1f, 1f);
    }

    public void StopCritTimer()
    {
        ableToCrit = false;
        timer = 0f;
    }

    /*
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
    */

    void OnDamage(ref DamageContext context)
    {
        /*
        if (currentBuff != -1.0f && critHit && context.actionID == ActionID.POLICE_CHIEF_BASIC)
        {
            critHit = false;
            context.damage *= currentBuff;
        }
        */

        if (pointIndex > 0 && ableToCrit && context.actionID == ActionID.POLICE_CHIEF_BASIC && !context.actionTypes.Contains(ActionType.SKILL))
        {
            context.damage *= values[pointIndex];
            StopCritTimer();
        }
    }

    public bool GetAbleToCrit()
    {
        return ableToCrit;
    }
}
