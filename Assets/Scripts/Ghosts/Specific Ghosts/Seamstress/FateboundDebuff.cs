using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Added debuff that shares any taken damage to other debuffed enemies
/// </summary>
public class FateboundDebuff : MonoBehaviour
{
    public SeamstressManager manager;

    void Awake()
    {
        GameplayEventHolder.OnDamageDealt += ShareDamage;
        GameplayEventHolder.OnDeath += PreserveConnection;
    }

    public void RemoveShareDamage()
    {
        GameplayEventHolder.OnDamageDealt -= ShareDamage;
        GameplayEventHolder.OnDeath -= PreserveConnection;
    }

    private void ShareDamage(DamageContext context)
    {
        if (context.victim == gameObject)
        {
            manager.DamageLinkedEnemies(gameObject.GetInstanceID(), context);
        }
    }

    private void PreserveConnection(ref DamageContext context)
    {
        if (context.victim == gameObject)
        {
            manager.RemoveFromLink(gameObject.GetInstanceID());
        }
    }
}
