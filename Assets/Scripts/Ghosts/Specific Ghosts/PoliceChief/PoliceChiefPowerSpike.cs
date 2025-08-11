using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class PoliceChiefPowerSpike : Skill
{
    private float timer = -1.0f;
    private bool ableToCrit = false;

    [SerializeField] private float[] values = {0f, 1.25f, 1.45f, 1.65f, 1.85f};
    private int pointIndex;

    [SerializeField] private float immediateTimeframe = 0.25f;
    [SerializeField] public float explosionRadius = 3f;
    [SerializeField] public DamageContext explosionDamage;
    //private bool reset = true;

    public override void AddPointTrigger()
    {
        pointIndex = GetPoints();

    }
    public override void RemovePointTrigger()
    {
        pointIndex = GetPoints();
    }
    public override void ClearPointsTrigger()
    {
        pointIndex = GetPoints();
    }

    void Start()
    {
        if (pointIndex <= 0) pointIndex = 0;
    }

    private void OnEnable()
    {
        //GameplayEventHolder.OnDamageFilter.Add(OnDamage);
    }

    void OnDisable()
    {
        //GameplayEventHolder.OnDamageFilter.Remove(OnDamage);
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
    void OnDamage(ref DamageContext context)
    {
        if (pointIndex > 0 && ableToCrit && context.actionID == ActionID.POLICE_CHIEF_BASIC && !context.actionTypes.Contains(ActionType.SKILL))
        {
            context.damage *= values[pointIndex];
            StopCritTimer();
        }
    }
    */

    public bool GetAbleToCrit()
    {
        return ableToCrit;
    }

    public DamageContext GetExplosionDamage()
    {
        explosionDamage.damage = values[pointIndex];
        return explosionDamage;
    }
}
