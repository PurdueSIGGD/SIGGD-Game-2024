using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Enemy AI for Knight (Offence Variant)
/// </summary>
public class KnightOffence : EnemyStateManager
{
    [Header("Sword Attack")]
    [SerializeField] protected Transform swordTrigger;
    [SerializeField] protected DamageContext swordDamage;

    protected void OnSwordEvent()
    {
        
    }
}
