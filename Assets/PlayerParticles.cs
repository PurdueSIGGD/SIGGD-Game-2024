using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticles : MonoBehaviour
{
    [SerializeField] ParticleSystem heavyCharging;
    [SerializeField] ParticleSystem heavyPrimed;
    [SerializeField] ParticleSystem heavyAttack;

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
        heavyAttack.Play();
    }
}
