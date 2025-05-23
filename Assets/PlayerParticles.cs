using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticles : MonoBehaviour
{
    private CameraShake shake;

    [SerializeField] ParticleSystem heavyCharging;
    [SerializeField] ParticleSystem heavyPrimed;
    [SerializeField] ParticleSystem heavyAttack;
    [SerializeField] ParticleSystem lightAttack;
    [SerializeField] ParticleSystem upLightAttack;

    void Start()
    {
        shake = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShake>();
    }

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
}
