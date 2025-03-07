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



    // ON HEALING DEALT
    /// <param name="context">Struct containing context for this event.</param>
    public delegate void HealingDealtEvent(HealingContext context);
    /// <summary>
    /// Invoked when an entity causes healing.
    /// </summary>
    public static HealingDealtEvent OnHealingDealt;



    // ON DEATH
    /// <param name="context">Struct containing context for this event.</param>
    public delegate void DeathEvent(DamageContext context);
    /// <summary>
    /// Invoked when an entity dies.
    /// </summary>
    public static DeathEvent OnDeath;

}
