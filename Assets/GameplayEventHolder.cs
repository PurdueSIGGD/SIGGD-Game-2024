using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameplayEventHolder : MonoBehaviour
{

    // ON DAMAGE RECEIVED
    /// <param name="context">Struct containing context for this event.</param>
    public delegate void DamageReceivedEvent(DamageContext context);
    /// <summary>
    /// Invoked when the player receives damage.
    /// </summary>
    public static DamageReceivedEvent OnDamageReceived;


}


/*
[Serializable] public struct DamageEventContext
{
    [NonSerialized] public GameObject attacker;
    [NonSerialized] public GameObject victim;
    public float damage;
    public DamageStrength damageStrength;
    public List<DamageType> damageTypes;
    public ActionID actionID;
    public List<ActionType> actionTypes;
    [NonSerialized] public object invokingScript;
    public string extraContext;
}
*/

/*
    // ON DAMAGE RECEIVED
    /// <param name="damage">The amount of damage received by the player.</param>
    /// <param name="context">Optional string meant to communicate unique context of this damage instance.</param>
    public delegate void DamageReceivedEvent(float damage, GameObject attacker, GameObject victim, string context = "");
    /// <summary>
    /// Invoked when the player receives damage.
    /// </summary>
    public static DamageReceivedEvent OnDamageReceived;
    */



/*
// ON DAMAGE RECEIVED
/// <param name="context">Struct containing context for this event.</param>
public delegate void DamageReceivedEvent(DamageContext context);
/// <summary>
/// Invoked when the player receives damage.
/// </summary>
public static DamageReceivedEvent OnDamageReceived;



// ON LETHAL DAMAGE RECEIVED
/// <param name="damage">The amount of damage received by the player.</param>
/// <param name="context">Optional string meant to communicate unique context of this damage instance.</param>
public delegate void LethalDamageReceivedEvent(float damage, GameObject attacker, GameObject victim, string context = "");
/// <summary>
///  Invoked when the player receives damage that reduces their health to zero or less.
/// </summary>
public static LethalDamageReceivedEvent OnLethalDamageReceived;



// ON DAMAGE DEALT
/// <param name="damage">The amount of damage dealt by the player.</param>
/// <param name="context">Optional string meant to communicate unique context of this damage instance.</param>
public delegate void DamageDealtEvent(float damage, string context = "");
/// <summary>
/// Invoked when the player deals damage.
/// </summary>
public static DamageDealtEvent OnDamageDealt;



// ON LETHAL DAMAGE DEALT
/// <param name="damage">The amount of damage dealt by the player.</param>
/// <param name="context">Optional string meant to communicate unique context of this damage instance.</param>
public delegate void LethalDamageDealtEvent(float damage, string context = "");
/// <summary>
/// Invoked when the player deals damage that reduces their target's health to zero or less.
/// </summary>
public static LethalDamageDealtEvent OnLethalDamageDealt;
*/