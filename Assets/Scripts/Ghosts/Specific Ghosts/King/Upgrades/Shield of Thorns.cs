//#define DEBUG_LOG
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// While Radiant Shield is at full health, a portion of damage that 
/// King Aegis or his shield takes is reflected to his attacker instead.
/// Damage Reflected: 8% | 16% | 24% | 32%
/// </summary>
public class ShieldOfThorns : Skill
{

    [SerializeField]
    List<float> values = new List<float>
    {
        0, 8, 16, 24, 32
    };
    public int pointIndex;

    private KingManager manager;
    private KingBasic basic;

    [SerializeField] public float shieldHealthPercentRequired = 10f;
    [SerializeField] private float damageRadius = 4f;
    [SerializeField] private DamageContext thornDamage;
    [SerializeField] private Sprite defenseIcon;
    [SerializeField] private GameObject thornsPulseVFX;

    [HideInInspector] public bool isShieldHealthSufficient = false;

    private void OnEnable()
    {
        manager = gameObject.GetComponent<KingManager>();
        GameplayEventHolder.OnDamageFilter.Insert(0, ReflectDamage);
        GameplayEventHolder.OnDeath += OnKillVoiceLine;
    }

    private void OnDisable()
    {
        GameplayEventHolder.OnDamageFilter.Remove(ReflectDamage);
        GameplayEventHolder.OnDeath -= OnKillVoiceLine;
    }

    private void Update()
    {
        /*
        if (manager.selected && manager.currentShieldHealth >= manager.GetStats().ComputeValue("Shield Max Health"))
        {
            PlayerParticles.instance.PlayGhostBadBuff(GetComponent<GhostIdentity>().GetCharacterInfo().whiteColor, 1f, 1f);
        }
        else
        {
            PlayerParticles.instance.StopGhostBadBuff();
        }
        */

        if (manager.selected && pointIndex > 0)
        {
            isShieldHealthSufficient = (manager.currentShieldHealth >= (manager.GetStats().ComputeValue("Shield Max Health") * shieldHealthPercentRequired));
            if (isShieldHealthSufficient && manager.basic != null && manager.basic.isShielding) PlayerParticles.instance.PlayGhostBadBuff(GetComponent<GhostIdentity>().GetCharacterInfo().whiteColor, 1f, 1f);
            else PlayerParticles.instance.StopGhostBadBuff();
        }
    }

    /// <summary>
    /// If player/shield was damaged and Shield of Thorns is in effect, call the
    /// reflect damage method
    /// </summary>
    /// <param name="context"></param>
    public void ReflectDamage(ref DamageContext context)
    {

        // Conditions

        if (pointIndex <= 0 || !manager.selected)
        {
            return;
        }

#if DEBUG_LOG
        Debug.Log("Shield of Thorns: " + context.victim.name + " hurt " + context.damage + " by " + context.attacker.name + " " + context.attacker.tag);
        Debug.Log("Shield of Thorns: Shield health " + manager.currentShieldHealth + "/" + manager.GetStats().ComputeValue("Shield Max Health") + ", points: " + GetPoints() + 
                  ", Shield up: " + manager.basic.isShielding);
#endif

        if (!isShieldHealthSufficient || (manager.basic != null && !manager.basic.isShielding))
        {
            return;
        }

        // Attacker should not be player

        if (context.attacker.CompareTag("Player") || !context.victim.CompareTag("Player"))
        {
            return;
        }

        if (context.damage <= 0f)
        {
            return;
        }

        // VFX & SFX
        GameObject thornsPulse = Instantiate(thornsPulseVFX, PlayerID.instance.transform.position, Quaternion.identity);
        thornsPulse.GetComponent<RingExplosionHandler>().playRingExplosion(damageRadius, GetComponent<GhostIdentity>().GetCharacterInfo().whiteColor);
        AudioManager.Instance.SFXBranch.PlaySFXTrack("Aegis-Thorns Damage");

        // Calculate damage that will be reflected

        //float damageReflected = 5f * context.damage * (values[pointIndex] / 100f);
        float damageReflected = context.damage * (values[pointIndex] / 100f);

        // Reduce damage
        /*
        context.damage *= ((100f - values[pointIndex]) / 100f);
        context.icon = defenseIcon;
        context.extraContext = "Thorns";
        */

        //if (!context.damageTypes.Contains(DamageType.MELEE)) return;

        /*
        DamageContext newContext = new DamageContext();
        newContext.damage = damageReflected;
        newContext.damageStrength = DamageStrength.MEAGER;
        newContext.damageTypes = new List<DamageType>() { DamageType.STATUS };
        newContext.actionID = ActionID.MISCELLANEOUS;
        newContext.actionTypes = new List<ActionType>() { ActionType.SKILL } ;
        */
        thornDamage.damage = damageReflected;

#if DEBUG_LOG
        Debug.Log("Health before: " + context.attacker.GetComponent<Health>().currentHealth);
        Debug.Log("Shield of Thorns: Reflected " + 8 * GetPoints() + "% damage to " + context.attacker);
#endif

        //context.attacker.GetComponent<Health>().Damage(thornDamage, context.victim);

        // Affect enemies
        Collider2D[] enemies = Physics2D.OverlapCircleAll(PlayerID.instance.transform.position, damageRadius, LayerMask.GetMask("Enemy"));
        foreach (Collider2D enemy in enemies)
        {
            enemy.gameObject.GetComponent<Health>().Damage(thornDamage, context.victim);
        }

#if DEBUG_LOG
        // not sure why this is not printing FIXME
        Debug.Log("Enemy Health " + context.attacker.GetComponent<Health>().currentHealth);
#endif
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


    private void OnKillVoiceLine(DamageContext context)
    {
        if (context.victim != null && context.victim.CompareTag("Enemy") && context.extraContext != null && context.extraContext.Equals("Thorns"))
        {
            AudioManager.Instance.VABranch.PlayVATrack("Aegis-King Thorns");
        }
    }

}
