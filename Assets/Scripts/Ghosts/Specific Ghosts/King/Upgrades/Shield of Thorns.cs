#define DEBUG_LOG
using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

public class ShieldOfThorns : Skill
{

    private int pointIndex = 0;
    private KingManager manager;
    private StatManager stats;

    private void OnEnable()
    {
        manager = gameObject.GetComponent<KingManager>();
        stats = manager.GetStats();

        GameplayEventHolder.OnDamageFilter.Insert(0,ReflectDamage); // important
    }

    private void OnDisable()
    {
        GameplayEventHolder.OnDamageFilter.Remove(ReflectDamage);

    }
    
    private void ReflectDamage(ref DamageContext damage)
    {
        // If shield is at full health
        if (manager.selected && pointIndex > 0 &&
            manager.currentShieldHealth >= stats.ComputeValue("Shield Max Health") &&
            damage.attacker.CompareTag("Enemy") &&
            (damage == asdf || damage.victim.CompareTag("Player")))
        {
#if DEBUG_LOG
            Debug.Log("LALALALALA");
#endif
            // Calculate damage that will be reflected

            float damageReflected = 0.08f * pointIndex;

            

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
