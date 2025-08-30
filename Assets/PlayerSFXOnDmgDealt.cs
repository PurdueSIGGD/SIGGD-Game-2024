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
            if (context.damageStrength <= DamageStrength.MODERATE)
            {
                AudioManager.Instance.SFXBranch.PlaySFXTrack("EnemyHitSFX");
            }
            if (context.damageStrength >= DamageStrength.HEAVY)
            {
                AudioManager.Instance.SFXBranch.PlaySFXTrack("MeatyEnemyDamageSFX");
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
