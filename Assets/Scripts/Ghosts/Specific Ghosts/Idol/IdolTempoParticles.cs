using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdolTempoParticles : MonoBehaviour
{
    [SerializeField] private ParticleSystem upParticleSystem;
    [SerializeField] private ParticleSystem downParticleSystem;

    [SerializeField] private float minEmission;
    [SerializeField] private float maxEmission;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetIntensity(float currentValue, float maxValue)
    {
        float emission = Mathf.Lerp(minEmission, maxEmission, (currentValue / maxValue));
        ParticleSystem.EmissionModule upEmission = upParticleSystem.emission;
        ParticleSystem.EmissionModule downEmission = downParticleSystem.emission;
        upEmission.rateOverTime = emission;
        downEmission.rateOverTime = emission;
    }
}
