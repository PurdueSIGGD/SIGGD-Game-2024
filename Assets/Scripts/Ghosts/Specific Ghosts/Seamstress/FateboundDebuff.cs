using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Added debuff that shares any taken damage to other debuffed enemies
/// </summary>
public class FateboundDebuff : MonoBehaviour
{
    void Awake()
    {
        GameplayEventHolder.OnDamageDealt += ShareDamage;
    }


    private void ShareDamage(DamageContext context)
    {
        SeamstressManager.DamageLinkedEnemies(gameObject.GetInstanceID(), context);
    }
}
