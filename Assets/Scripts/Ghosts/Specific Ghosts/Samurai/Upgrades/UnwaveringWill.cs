using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class UnwaveringWill : Skill
{
    // [SerializeField] float triggerThreshold;

    [SerializeField] HealingContext healContext;
    //[Header("Life steal rate")]
    //[SerializeField] float healRate;

    [SerializeField]
    List<float> values = new List<float>
    {
        0f, 0.05f, 0.1f, 0.15f, 0.2f
    };
    private int pointIndex;

    private SamuraiManager manager;
    private PlayerHealth health;

    void Start()
    {
        manager = GetComponent<SamuraiManager>();
        health = PlayerHealth.instance;
    }

    private void Update()
    {
        if (manager.selected && pointIndex > 0)
        {
            if (health.MortallyWounded) PlayerParticles.instance.PlayGhostGoodBuff(GetComponent<GhostIdentity>().GetCharacterInfo().highlightColor, 0.5f, 1f);
            else PlayerParticles.instance.StopGhostGoodBuff();
        }
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
            healContext.healing = context.damage * values[pointIndex];
            health.Heal(healContext, PlayerID.instance.gameObject);
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
