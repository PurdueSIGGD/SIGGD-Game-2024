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
    MISCELLANEOUS,
}

public enum DamageStrength
{
    LIGHT,
    MODERATE,
    HEAVY,
}

public enum DamageType
{
    MELEE,
    PROJECTILE,
    AREA,
    STATUS,
    ENVIRONMENTAL,
}

[Serializable] public struct DamageContext
{
    [NonSerialized] public GameObject attacker;
    [NonSerialized] public GameObject victim;
    public float damage;
    [NonSerialized] public float trueDamage;
    public DamageStrength damageStrength;
    public List<DamageType> damageTypes;
    public ActionID actionID;
    public List<ActionType> actionTypes;
    [NonSerialized] public object invokingScript;
    public string extraContext;
}

[Serializable] public struct HealingContext
{
    [NonSerialized] public GameObject healer;
    [NonSerialized] public GameObject healee;
    public float healing;
    [NonSerialized] public float trueHealing;
    public ActionID actionID;
    public List<ActionType> actionTypes;
    [NonSerialized] public object invokingScript;
    public string extraContext;
}

[Serializable] public struct ActionContext
{
    public ActionID actionID;
    public List<ActionType> actionTypes;
    [NonSerialized] public object invokingScript;
    public string extraContext;
}
