using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Added debuff that shares any taken damage to other debuffed enemies
/// </summary>
public class FateboundDebuff : MonoBehaviour
{
    public SeamstressManager manager;
    public GameObject fateboundVFX;

    void OnEnable()
    {
        GameplayEventHolder.OnDamageDealt += ShareDamage;
        GameplayEventHolder.OnDeath += PreserveConnection;
    }

    private void OnDisable()
    {
        GameplayEventHolder.OnDamageDealt -= ShareDamage;
        GameplayEventHolder.OnDeath -= PreserveConnection;
    }

    public void RemoveShareDamage()
    {
        Destroy(fateboundVFX);
        GameplayEventHolder.OnDamageDealt -= ShareDamage;
        GameplayEventHolder.OnDeath -= PreserveConnection;
    }

    private void ShareDamage(DamageContext context)
    {
        if (context.victim == gameObject && context.actionID != ActionID.SEAMSTRESS_SPECIAL /*!context.damageTypes.Contains(DamageType.STATUS)*/)
        {
            manager.DamageLinkedEnemies(gameObject.GetInstanceID(), context, true);
        }
    }

    private void PreserveConnection(DamageContext context)
    {
        if (context.victim == gameObject)
        {
            AudioManager.Instance.VABranch.PlayVATrack("Yume-Seamstress Fatebound Kill");
            AudioManager.Instance.SFXBranch.PlaySFXTrack("Yume-Fatebound Damage");

            // Handle Scrap Saver Skill
            manager.gameObject.GetComponent<ScrapSaver>().HandleEnemyDefeated();

            RemoveShareDamage();

            manager.gameObject.GetComponent<UnraveledFate>().DamageFateboundEnemies(gameObject.GetInstanceID(), gameObject.transform.position);

            manager.RemoveFromLink(gameObject.GetInstanceID());

        }
    }
}
