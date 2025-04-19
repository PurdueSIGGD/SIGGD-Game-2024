using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameplayEventHolder : MonoBehaviour
{

    // ON DAMAGE DEALT
    /// <param name="context">Struct containing context for this event.</param>
    public delegate void DamageDealtEvent(DamageContext context);
    /// <summary>
    /// Invoked when an entity deals damage.
    /// </summary>
    public static DamageDealtEvent OnDamageDealt;

    // ON DAMAGE FILTER
    /// <param name="context">Struct containing context for this event.</param>
    public delegate void DamageFilterEvent(ref DamageContext context);
    /// <summary>
    /// Invoked when an entity deals damage.
    /// </summary>
    public static List<DamageFilterEvent> OnDamageFilter = new List<DamageFilterEvent>();


    // ON HEALING DEALT
    /// <param name="context">Struct containing context for this event.</param>
    public delegate void HealingDealtEvent(HealingContext context);
    /// <summary>
    /// Invoked when an entity causes healing.
    /// </summary>
    public static HealingDealtEvent OnHealingDealt;

    // ON HEALING FILTER
    /// <param name="context">Struct containing context for this event.</param>
    public delegate void HealingFilterEvent(ref HealingContext context);
    /// <summary>
    /// Invoked when an entity deals damage.
    /// </summary>
    public static List<HealingFilterEvent> OnHealingFilter = new List<HealingFilterEvent>();



    // ON DEATH
    /// <param name="context">Struct containing context for this event.</param>
    public delegate void DeathEvent(ref DamageContext context);
    /// <summary>
    /// Invoked when an entity dies.
    /// </summary>
    public static DeathEvent OnDeath;

    // ON DEATH FILTER
    /// <param name="context">Struct containing context for this event.</param>
    public delegate void DeathFilterEvent(ref DamageContext context);
    /// <summary>
    /// Invoked when an entity deals damage.
    /// </summary>
    public static List<DeathFilterEvent> OnDeathFilter = new List<DeathFilterEvent>();

}
