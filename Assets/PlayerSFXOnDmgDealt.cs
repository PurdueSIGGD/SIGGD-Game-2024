using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSFXOnDmgDealt : MonoBehaviour
{
    private void OnEnable()
    {
        GameplayEventHolder.OnDamageDealt += PlaySFXOnDMG;
        GameplayEventHolder.OnDeath += PlaySFXOnDead;
    }

    private void OnDisable()
    {
        GameplayEventHolder.OnDamageDealt -= PlaySFXOnDMG;
        GameplayEventHolder.OnDeath -= PlaySFXOnDead;
    }

    private void PlaySFXOnDMG(DamageContext context)
    {
        if (context.victim && context.victim.CompareTag("Enemy"))
        {

            //AudioManager.Instance.SFXBranch.GetSFXTrack("MeatyEnemyDamageSFX").SetPitch(0f, 1f);
            //Health victimHealth = context.victim.GetComponent<Health>();
            //if (victimHealth != null) AudioManager.Instance.SFXBranch.GetSFXTrack("MeatyEnemyDamageSFX").SetPitch(victimHealth.GetStats().ComputeValue("Max Health") - victimHealth.currentHealth, victimHealth.GetStats().ComputeValue("Max Health"));
            AudioManager.Instance.SFXBranch.PlaySFXTrack("MeatyEnemyDamageSFX");

            /*
            if (context.damageStrength == DamageStrength.MEAGER ||
                context.damageStrength == DamageStrength.MINOR ||
                context.damageStrength == DamageStrength.LIGHT)
            {
                AudioManager.Instance.SFXBranch.PlaySFXTrack("EnemyHitSFX");
            }
            if (context.damageStrength == DamageStrength.MODERATE ||
                context.damageStrength == DamageStrength.HEAVY ||
                context.damageStrength == DamageStrength.DEVASTATING)
            {
                AudioManager.Instance.SFXBranch.PlaySFXTrack("MeatyEnemyDamageSFX");
            }
            */

            if (context.isCriticalHit)
            {
                AudioManager.Instance.SFXBranch.PlaySFXTrack("Akihito-Parry Success");
            }
        }
    }

    private void PlaySFXOnDead(DamageContext context)
    {
        if (context.victim && context.victim.CompareTag("Enemy"))
        {
            AudioManager.Instance.SFXBranch.PlaySFXTrack("EnemyDeathSFX");
        }
    }
}
