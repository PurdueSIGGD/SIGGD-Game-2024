using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

public class DynamicTrio : Skill
{
    IdolManager manager;
    List<int> values = new List<int>
    {
        0, 15, 30, 45, 60
    };
    float transferPercentage = 0;
    private static int pointIndex;
    void Start()
    {
        manager = gameObject.GetComponent<IdolManager>();
        manager.evaSelectedEvent.AddListener(EvaSelected);
    }
    void Update()
    {
        if (manager.special && manager.special.spawnSecondClone != pointIndex > 0)
        {
            manager.special.spawnSecondClone = pointIndex > 0;
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
        transferPercentage = (float)values[pointIndex] / 100;
        if (pointIndex > 0)
        {
            if (GameplayEventHolder.OnDamageFilter.Contains(TransferDamage) == false)
            {
                GameplayEventHolder.OnDamageFilter.Add(TransferDamage);
            }
        }
        else
        {
            GameplayEventHolder.OnDamageFilter.Remove(TransferDamage);
        }
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
        float originalDamage = context.damage;

        // split transfer damage across each decoy in the list

        context.damage = originalDamage * transferPercentage / manager.clones.Count;
        foreach (GameObject decoy in manager.clones)
        {
            context.victim = decoy;
            print(context.victim.name);
            decoy.GetComponent<Health>().NoContextDamage(context, context.attacker);
        }

        // set remaining damage for the player

        context.victim = PlayerID.instance.gameObject;
        context.damage = originalDamage * (1 - transferPercentage);
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
