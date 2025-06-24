using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticles : MonoBehaviour
{

    [SerializeField] ParticleSystem heavyCharging;
    [SerializeField] ParticleSystem heavyPrimed;
    [SerializeField] ParticleSystem heavyAttack;
    [SerializeField] ParticleSystem lightAttack;
    [SerializeField] ParticleSystem upLightAttack;

    void Start()
    {

    }



    // Heavy Attack
    public void StartHeavyChargeUp()
    {
        heavyCharging.Play();
    }

    public void StopHeavyChargeUp()
    {
        heavyCharging.Stop();
    }

    public void StartHeavyPrimed()
    {
        heavyPrimed.Play();
    }

    public void StopHeavyPrimed()
    {
        heavyPrimed.Stop();
    }

    public void PlayHeavyAttackVFX()
    {
        StartCoroutine(HeavyAttackCoroutine());
    }

    private IEnumerator HeavyAttackCoroutine()
    {
        yield return new WaitForSeconds(0.12f);
        heavyAttack.Play();
    }



    // Light Attack
    public void PlayLightAttackVFX(bool isUp)
    {
        StartCoroutine(LightAttackCoroutine(isUp));
    }

    private IEnumerator LightAttackCoroutine(bool isUp)
    {
        yield return new WaitForSeconds(0.05f);
        if (isUp)
        {
            upLightAttack.Play();
            yield break;
        }
        lightAttack.Play();
    }



    // Police Chief Heavy Attack
    public void StartSidearmChargeUp()
    {
        heavyCharging.Play();
    }

    public void StopSidearmChargeUp()
    {
        heavyCharging.Stop();
    }

    public void StartSidearmPrimed()
    {
        heavyPrimed.Play();
    }

    public void StopSidearmPrimed()
    {
        heavyPrimed.Stop();
    }



    // Police Chief Special Ability
    public void StartSpecialChargeUp()
    {
        heavyCharging.Play();
    }

    public void StopSpecialChargeUp()
    {
        heavyCharging.Stop();
    }

    public void StartSpecialPrimed()
    {
        heavyPrimed.Play();
    }

    public void StopSpecialPrimed()
    {
        heavyPrimed.Stop();
    }
}
