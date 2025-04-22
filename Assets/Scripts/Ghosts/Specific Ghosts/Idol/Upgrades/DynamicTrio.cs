using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class DynamicTrio : Skill
{
    IdolManager manager;
    List<int> values = new List<int>
    {
        0, 15, 30, 45, 60
    };
    float transferPercentage = 0;
    void Start()
    {
        manager = gameObject.GetComponent<IdolManager>();

        // testing code
        /*
        int points = 4;
        for (int i = 0; i < points; i++)
        {
            AddPoint();
        }
        */
    }
    void Update()
    {
        if (manager.special && manager.special.spawnSecondClone != skillPts > 0)
        {
            manager.special.spawnSecondClone = skillPts > 0;
        }
    }
    public override void AddPointTrigger()
    {
        UpdateSkill();
    }
    public override void RemovePointTrigger()
    {
        UpdateSkill();
    }
    public override void ClearPointsTrigger()
    {
        UpdateSkill();
    }
    public void UpdateSkill()
    {
        transferPercentage = (float)values[skillPts] / 100;
        if (skillPts > 0)
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
}
