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
            manager.DamageLinkedEnemies(gameObject.GetInstanceID(), context, true);
        }
    }

    private void PreserveConnection(DamageContext context)
    {
        if (context.victim == gameObject)
        {

            // Handle Scrap Saver Skill
            manager.gameObject.GetComponent<ScrapSaver>().HandleEnemyDefeated();

            manager.RemoveFromLink(gameObject.GetInstanceID());

            RemoveShareDamage();


            // Handle Unraveled Fate Skill (must be after RemoveFromLink - otherwise stack overflow)
            manager.gameObject.GetComponent<UnraveledFate>().DamageFateboundEnemies(gameObject.GetInstanceID());


        }
    }
}
