using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Added debuff that shares any taken damage to other debuffed enemies
/// </summary>
public class FateboundDebuff : MonoBehaviour
{
    YumeSpecial yume;

    void Awake()
    {
        yume = PlayerID.instance.GetComponent<YumeSpecial>();
        GameplayEventHolder.OnDamageDealt += ShareDamage;
    }


    private void ShareDamage(DamageContext context)
    {
        yume.DamageLinkedEnemies(gameObject.GetInstanceID(), context);
    }
}
