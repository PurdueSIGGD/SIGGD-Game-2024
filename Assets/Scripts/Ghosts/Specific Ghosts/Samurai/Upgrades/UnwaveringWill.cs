using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class UnwaveringWill : Skill
{
    // [SerializeField] float triggerThreshold;

    [SerializeField] HealingContext healContext;
    [Header("Life steal rate")]
    [SerializeField] float healRate;

    private static int pointIndex;
    private SamuraiManager manager;
    private PlayerHealth health;

    void Start()
    {
        manager = GetComponent<SamuraiManager>();
        health = PlayerHealth.instance;
    }


    private void OnEnable()
    {
        GameplayEventHolder.OnDamageDealt += LifeSteal;
    }

    private void OnDisable()
    {
        GameplayEventHolder.OnDamageDealt -= LifeSteal;
    }

    private void LifeSteal(DamageContext context)
    {
        if (pointIndex > 0 && manager.selected && 
            health.MortallyWounded && context.attacker.CompareTag("Player"))
        {
            healContext.healing = context.damage * pointIndex * healRate;
            health.Heal(healContext, gameObject);
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
