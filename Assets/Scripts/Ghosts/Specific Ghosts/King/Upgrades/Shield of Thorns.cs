//#define DEBUG_LOG
using System.Collections.Generic;

/// <summary>
/// While Radiant Shield is at full health, a portion of damage that 
/// King Aegis or his shield takes is reflected to his attacker instead.
/// Damage Reflected: 8% | 16% | 24% | 32%
/// </summary>
public class ShieldOfThorns : Skill
{

    private KingManager manager;
    private KingBasic basic;

    private void OnEnable()
    {
        manager = gameObject.GetComponent<KingManager>();
        GameplayEventHolder.OnDamageFilter.Insert(0, ReflectDamage);
    }

    private void OnDisable()
    {
        GameplayEventHolder.OnDamageFilter.Remove(ReflectDamage);
    }

    /// <summary>
    /// If player/shield was damaged and Shield of Thorns is in effect, call the
    /// reflect damage method
    /// </summary>
    /// <param name="context"></param>
    public void ReflectDamage(ref DamageContext context)
    {

        // Conditions

        if (GetPoints() <= 0 || !manager.selected)
        {
            return;
        }

#if DEBUG_LOG
        Debug.Log("Shield of Thorns: " + context.victim.name + " hurt " + context.damage + " by " + context.attacker.name + " " + context.attacker.tag);
        Debug.Log("Shield of Thorns: Shield health " + manager.currentShieldHealth + "/" + manager.GetStats().ComputeValue("Shield Max Health") + ", points: " + GetPoints() + 
                  ", Shield up: " + manager.basic.isShielding);
#endif

        if (manager.currentShieldHealth < manager.GetStats().ComputeValue("Shield Max Health"))
        {
            return;
        }

        // Attacker should not be player

        if (context.attacker.CompareTag("Player") || !context.victim.CompareTag("Player"))
        {
            return;
        }

        // Calculate damage that will be reflected

        float damageReflected = 0.08f * GetPoints() * context.damage;

        // Reduce damage

        DamageContext newContext = new DamageContext();
        newContext.damage = damageReflected;
        newContext.damageStrength = DamageStrength.MEAGER;
        newContext.damageTypes = new List<DamageType>() { DamageType.STATUS };
        newContext.actionID = ActionID.MISCELLANEOUS;
        newContext.actionTypes = new List<ActionType>() { ActionType.SKILL } ;

#if DEBUG_LOG
        Debug.Log("Health before: " + context.attacker.GetComponent<Health>().currentHealth);
        Debug.Log("Shield of Thorns: Reflected " + 8 * GetPoints() + "% damage to " + context.attacker);
#endif

        context.attacker.GetComponent<Health>().Damage(newContext, context.victim);

#if DEBUG_LOG
        // not sure why this is not printing FIXME
        Debug.Log("Enemy Health " + context.attacker.GetComponent<Health>().currentHealth);
#endif
    }

    public override void AddPointTrigger()
    {
        //pointIndex = GetPoints();
    }

    public override void ClearPointsTrigger()
    {
        //pointIndex = GetPoints();
    }

    public override void RemovePointTrigger()
    {
        //pointIndex = GetPoints(); 
    }

}
