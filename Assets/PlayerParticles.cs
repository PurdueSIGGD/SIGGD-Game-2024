using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticles : MonoBehaviour
{
    private CameraShake shake;

    [SerializeField] ParticleSystem heavyCharging;
    [SerializeField] ParticleSystem heavyPrimed;
    [SerializeField] ParticleSystem heavyAttack;

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
        StartCoroutine(HeavyAttackCoroutine());
    }

    private IEnumerator HeavyAttackCoroutine()
    {
        yield return new WaitForSeconds(0.3f);
        heavyAttack.Play();
        shake.Shake(0.2f, 10f, 0, 10, new Vector2(Random.Range(-0.5f, 0.5f), 1f));
    }
}
