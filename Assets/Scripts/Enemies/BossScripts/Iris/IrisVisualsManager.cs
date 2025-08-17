using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IrisVisualsManager : MonoBehaviour
{
    [SerializeField] List<IrisVisualComponent> visualComponents = new List<IrisVisualComponent>();
    [SerializeField] List<float> shakeIntensities = new List<float>();
    [SerializeField] float deathShakeIntensity;
    [SerializeField] GameObject shieldVisual;
    [SerializeField] GameObject shieldUIVisual;
    int localDamageState = IrisVisualStates.NORMAL;
    float shakeIntensity = 0;
    float shakeTimer;
    [SerializeField] float shakeTime;
    Vector2 center;
    void Start()
    {
        center = transform.position;
        SetVisualState(localDamageState);
    }
    void Update()
    {
        shakeTimer += Time.deltaTime;
        if (shakeTimer > (shakeTime / (1 + shakeIntensity)))
        {
            if (shakeIntensity == 0)
                return;
            Vector2 offset = Random.insideUnitCircle * shakeIntensity;
            transform.position = center + offset;
            shakeTimer = 0;
        }
    }
    public void SetVisualState(int damageState)
    {
        // don't bother updating if the damage state didn't change
        if (damageState == localDamageState) return;

        shakeIntensity = shakeIntensities[damageState];
        foreach (IrisVisualComponent component in visualComponents)
        {
            component.SetSpriteState(damageState);
        }
    }
    public void ToggleShieldVisual(bool val)
    {
        shieldVisual.SetActive(val);
        shieldUIVisual.SetActive(val);
    }
    public void ActivateDeathVisual()
    {
        shakeIntensity = deathShakeIntensity;
    }
}
