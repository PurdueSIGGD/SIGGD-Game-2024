using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PoliceChiefLethalForce : Skill
{
    [SerializeField] private int[] pointCounts = { 4, 3, 2, 1 };
    private int numHits = -1;
    private int consecutiveHits = 0;
    private float timer = -1.0f;

    private void Start()
    {
        /*if(GetPoints() > 0)
        {
            numHits = pointCounts[GetPoints() - 1];
        }
        else
        {
            numHits = -1;
        }*/

        
    }

    private void OnEnable()
    {
        GameplayEventHolder.OnDamageFilter.Add(OnDamage);
    }

    private void OnDisable()
    {
        GameplayEventHolder.OnDamageFilter.Remove(OnDamage);
    }

    private void Update()
    {
        if (timer > 0) { 
            timer -= Time.deltaTime;
            if (timer <= 0) {
                timer = -1f;
                consecutiveHits = 0;
            }
        }
    }

    public override void AddPointTrigger()
    {
        numHits = pointCounts[GetPoints() - 1];
    }
    public override void RemovePointTrigger()
    {

    }
    public override void ClearPointsTrigger()
    {
        numHits = -1;
    }

    public void OnDamage(ref DamageContext context)
    {
        if(context.attacker == PlayerID.instance.gameObject && context.actionID == ActionID.POLICE_CHIEF_BASIC)
        {
            if(numHits != -1 && consecutiveHits >= numHits)
            {
                context.damage *= 2;
                context.damageStrength = context.damageStrength + 1;
                consecutiveHits = 0;
            }
            else
            {
                consecutiveHits += 1;
                timer = -1.0f;
            }
        }
    }

    public void OnAbilityUse(ActionContext action)
    {
        if(action.actionID == ActionID.POLICE_CHIEF_BASIC)
        {
            timer = 0.1f;
        }
    }

    public int GetConsecutiveHits()
    {
        return consecutiveHits;
    }

    public int GetTotalHits()
    {
        return numHits;
    }
}
