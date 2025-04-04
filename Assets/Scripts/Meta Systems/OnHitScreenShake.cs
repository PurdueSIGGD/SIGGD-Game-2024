using UnityEngine;

public class OnHitScreenShake : MonoBehaviour
{

    [SerializeField] private bool useCustomAmplitude = false;
    [SerializeField] private float customAmplitude = 1f;
    [SerializeField] private float playerScreenShakeMultiplier = 1f;
    [SerializeField] private float meagerAmplitude = 0.04f;
    [SerializeField] private float lightAmplitude = 0.1f;
    [SerializeField] private float moderateAmplitude = 0.2f;
    [SerializeField] private float heavyAmplitude = 0.4f;
    [SerializeField] private float devastatingAmplitude = 1.0f;
    [SerializeField] private float shakeFrequency = 10f;
    [SerializeField] private float taperDecreaseRate = 10f;
    [SerializeField] private float taperDelayTime = 0f;

    private CameraShake cameraShaker;

    private void OnEnable()
    {
        GameplayEventHolder.OnDamageDealt += onHitScreenShake;
    }

    private void OnDisable()
    {
        GameplayEventHolder.OnDamageDealt -= onHitScreenShake;
    }

    private void Start()
    {
        cameraShaker = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraShake>();
    }

    private void onHitScreenShake(DamageContext context)
    {
        Vector2 shakeDirection = new Vector2(Random.Range(-0.5f, 0.5f), 1f);
        if (useCustomAmplitude)
        {
            cameraShaker.Shake(customAmplitude, taperDecreaseRate, taperDelayTime, shakeFrequency, shakeDirection);
            return;
        }
        float amplitude = 0f;
        switch (context.damageStrength)
        {
            case DamageStrength.MEAGER:
                amplitude = meagerAmplitude;
                break;
            case DamageStrength.LIGHT:
                amplitude = lightAmplitude;
                break;
            case DamageStrength.MODERATE:
                amplitude = moderateAmplitude;
                break;
            case DamageStrength.HEAVY:
                amplitude = heavyAmplitude;
                break;
            case DamageStrength.DEVASTATING:
                amplitude = devastatingAmplitude;
                break;
            default:
                amplitude = meagerAmplitude;
                break;
        }
        amplitude *= (context.victim.tag == "Player") ? playerScreenShakeMultiplier : 1f;
        cameraShaker.Shake(amplitude, taperDecreaseRate, taperDelayTime, shakeFrequency, shakeDirection);
    }

}
