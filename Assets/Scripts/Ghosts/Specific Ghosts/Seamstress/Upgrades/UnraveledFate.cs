#define DEBUG_LOG

using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;

/// <summary>
/// Fatebound enemies release a flurry of threads when defeated, damaging all other fatebound enemies.
/// Damage: 15 | 25 | 35 | 45
/// </summary>
public class UnraveledFate : Skill
{

    private SeamstressManager manager;

    private SeamstressManager GetManager()
    {
        if (manager == null)
        {
            manager = gameObject.GetComponent<SeamstressManager>();
        }
        return manager;
    }

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
    }

    /// <summary>
    /// Damages fatebound enemies, called in seamstress manager when fatebound enemy dies
    /// </summary>
    /// <param name="enemyID"> The instance id of the enemy that was defeated </param>
    public void DamageFateboundEnemies(int enemyID)
    {
        if (GetPoints() <= 0) { return; }

        if (!GetManager()) { return; }

        int damageAmount = (GetPoints() - 1) * 10 + 15;

        DamageContext damage = new DamageContext();
        damage.damage = damageAmount;
        damage.damageStrength = DamageStrength.MEAGER;
        damage.damageTypes = new List<DamageType>() { DamageType.PROJECTILE };
        damage.actionID = ActionID.MISCELLANEOUS;
        damage.actionTypes = new List<ActionType>() { ActionType.SKILL };

        manager.DamageLinkedEnemies(enemyID, damage, false);

#if DEBUG_LOG
        Debug.Log("Unraveled Fate: Damaged fatebound enemies by " + damageAmount);
#endif

    }

    public override void AddPointTrigger()
    {
    }

    public override void ClearPointsTrigger()
    {
    }

    public override void RemovePointTrigger()
    {
    }
}
