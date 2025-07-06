using System.Collections.Generic;
using UnityEngine;

public class DynamicTrio : Skill
{
    IdolManager manager;
    [SerializeField] List<float> values = new List<float>
    {
        0, 15, 30, 45, 60
    };
    float transferPercentage = 0;
    private int pointIndex;

    void Start()
    {
        manager = gameObject.GetComponent<IdolManager>();
        manager.evaSelectedEvent.AddListener(EvaSelected);
    }

    private void OnEnable()
    {
        GameplayEventHolder.OnDamageFilter.Add(TransferDamage);
    }

    private void OnDisable()
    {
        GameplayEventHolder.OnDamageFilter.Remove(TransferDamage);
    }

    void Update()
    {
        if (manager.special && manager.special.spawnSecondClone != (pointIndex > 0))
        {
            manager.special.spawnSecondClone = (pointIndex > 0);
        }
    }

    public override void AddPointTrigger()
    {
        pointIndex = GetPoints();
        UpdateSkill();
    }

    public override void RemovePointTrigger()
    {
        pointIndex = GetPoints();
        UpdateSkill();
    }

    public override void ClearPointsTrigger()
    {
        pointIndex = GetPoints();
        UpdateSkill();
    }

    public void UpdateSkill()
    {
        transferPercentage = values[pointIndex] / 100f;
    }

    public void TransferDamage(ref DamageContext context)
    {
        if (context.victim != PlayerID.instance.gameObject)
        {
            return;
        }

        if (manager.clones.Count == 0)
        {
            return;
        }

        transferPercentage = values[pointIndex] / 100f;
        float originalDamage = context.damage;

        // split transfer damage across each decoy in the list
        context.damage = originalDamage * transferPercentage;
        foreach (GameObject decoy in manager.clones)
        {
            context.victim = decoy;
            decoy.GetComponent<Health>().NoContextDamage(context, context.attacker);
        }

        // set remaining damage for the player
        context.victim = PlayerID.instance.gameObject;
        context.damage = originalDamage * (1f - transferPercentage);
    }

    private void EvaSelected()
    {
        if (pointIndex == 0)
        {
            return;
        }
        manager.special.avaliableHoloJumpVA.Add("Eva-Idol Dynamic Trio");
    }
}
