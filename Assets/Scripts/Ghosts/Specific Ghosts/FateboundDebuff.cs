using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
