using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ActionID
{
    GHOST_SWAP,
    PLAYER_LIGHT_ATTACK,
    PLAYER_HEAVY_ATTACK,
    PLAYER_BASIC,
    PLAYER_SPECIAL,
    POLICE_CHIEF_BASIC,
    POLICE_CHIEF_SPECIAL,
    IDOL_BASIC,
    IDOL_SPECIAL,
    SAMURAI_BASIC,
    SAMURAI_SPECIAL,
    SEAMSTRESS_BASIC,
    SEAMSTRESS_SPECIAL,
    PLAGUE_DOCTOR_BASIC,
    PLAGUE_DOCTOR_SPECIAL,
    KING_BASIC,
    KING_SPECIAL,
    MISCELLANEOUS,
}

public enum ActionType
{
    LIGHT_ATTACK,
    HEAVY_ATTACK,
    BASIC_ABILITY,
    SPECIAL_ABILITY,
    SKILL,
    SACRIFICE,
    ITEM,
    ENEMY_ATTACK,
    MISCELLANEOUS,
}

public enum DamageStrength
{
    MEAGER,
    LIGHT,
    MODERATE,
    HEAVY,
    DEVASTATING,
}

public enum DamageType
{
    MELEE,
    PROJECTILE,
    AREA,
    STATUS,
    ENVIRONMENTAL,
}

/// <summary>
/// Struct that contains context for a damage instance.
/// </summary>
[Serializable] public struct DamageContext
{
    //IMPORTANT NOTE: Do NOT add new fields to this struct above any existing fields.
    //                Doing so will cause serialized damage information to be corrupted for EVERY attack in this project.
    //                You may still add fields under the existing ones.

    /// <summary>
    /// The GameObject that is inflicting damage.
    /// </summary>
    [NonSerialized] public GameObject attacker;
    /// <summary>
    /// The GameObject that is receiving damage.
    /// </summary>
    [NonSerialized] public GameObject victim;
    /// <summary>
    /// The amount of damage dealt in this damage instance.
    /// </summary>
    public float damage;
    /// <summary>
    /// The damage value of the attack that is causing this damage instance. This might not equal the actual damage dealt.
    /// </summary>
    [NonSerialized] public float trueDamage;
    /// <summary>
    /// An arbitrary value for an attack's power. Useful for determining the strength of effects such as screen shake.
    /// </summary>
    public DamageStrength damageStrength;
    /// <summary>
    /// A list of this damage instance's damage types. Can be relevant for triggering certain gameplay effects.
    /// </summary>
    public List<DamageType> damageTypes;
    /// <summary>
    /// An identifier for the action that is causing this damage instance.
    /// </summary>
    public ActionID actionID;
    /// <summary>
    /// A list of the action types of the action that is causing this damage instance. Can be relevant for triggering certain gameplay effects.
    /// </summary>
    public List<ActionType> actionTypes;
    /// <summary>
    /// A reference to the script that invoked this damage event. Useful for debugging.
    /// </summary>
    [NonSerialized] public object invokingScript;
    /// <summary>
    /// An arbitrary string containing any extra information the user wishes to relay through this damage instance.
    /// </summary>
    public string extraContext;
}

/// <summary>
/// Struct that contains context for a healing instance.
/// </summary>
[Serializable] public struct HealingContext
{
    //IMPORTANT NOTE: Do NOT add new fields to this struct above any existing fields.
    //                Doing so will cause serialized healing information to be corrupted for EVERY healing action in this project.
    //                You may still add fields under the existing ones.

    /// <summary>
    /// The GameObject that is providing healing.
    /// </summary>
    [NonSerialized] public GameObject healer;
    /// <summary>
    /// The GameObject that is receiving healing.
    /// </summary>
    [NonSerialized] public GameObject healee;
    /// <summary>
    /// The amount of healing provided in this healing instance.
    /// </summary>
    public float healing;
    /// <summary>
    /// The healing value of the action that is causing this healing instance. This might not equal the actual healing dealt.
    /// </summary>
    [NonSerialized] public float trueHealing;
    /// <summary>
    /// An identifier for the action that is causing this healing instance.
    /// </summary>
    public ActionID actionID;
    /// <summary>
    /// A list of the action types of the action that is causing this healing instance. Can be relevant for triggering certain gameplay effects.
    /// </summary>
    public List<ActionType> actionTypes;
    /// <summary>
    /// A reference to the script that invoked this healing event. Useful for debugging.
    /// </summary>
    [NonSerialized] public object invokingScript;
    /// <summary>
    /// An arbitrary string containing any extra information the user wishes to relay through this healing instance.
    /// </summary>
    public string extraContext;
}

/// <summary>
/// Struct that contains context for an action instance.
/// </summary>
[Serializable] public struct ActionContext
{
    //IMPORTANT NOTE: Do NOT add new fields to this struct above any existing fields.
    //                Doing so will cause serialized action information to be corrupted for EVERY action using this struct.
    //                You may still add fields under the existing ones.

    /// <summary>
    /// An identifier for this action.
    /// </summary>
    public ActionID actionID;
    /// <summary>
    /// A list of the action types of this action. Can be relevant for triggering certain gameplay effects.
    /// </summary>
    public List<ActionType> actionTypes;
    /// <summary>
    /// A reference to the script that invoked this action instance. Useful for debugging.
    /// </summary>
    [NonSerialized] public object invokingScript;
    /// <summary>
    /// An arbitrary string containing any extra information the user wishes to relay through this action instance.
    /// </summary>
    public string extraContext;
}
