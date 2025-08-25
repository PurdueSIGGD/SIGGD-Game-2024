using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageTakenBackground : MonoBehaviour
{
    [SerializeField] private float minDamage;
    [SerializeField] private float minOpacity;
    [SerializeField] private float maxDamage;
    [SerializeField] private float maxOpacity;
    [SerializeField] private float mortalWoundOpacity;
    [SerializeField] private float opacityDecayTime;

    private Image background;
    private float initialOpacity = 0f;
    private float opacity = 0f;
    private float timer = 0f;
    private bool receivedMortalWound = false;


    private void OnEnable()
    {
        GameplayEventHolder.OnDamageDealt += OnDamageTaken;
    }

    private void OnDestroy()
    {
        GameplayEventHolder.OnDamageDealt -= OnDamageTaken;
    }

    // Start is called before the first frame update
    void Start()
    {
        background = GetComponent<Image>();
        SetBackgroundOpacity(0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0f)
        {
            opacity = initialOpacity * (timer / opacityDecayTime);
            if (opacity < 0f) opacity = 0f;
            SetBackgroundOpacity(opacity);

            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                timer = 0f;
                SetBackgroundOpacity(0f);
            }
        }

        if (receivedMortalWound && opacity == 0f)
        {
            receivedMortalWound = false;
        }
    }



    private void OnDamageTaken(DamageContext context)
    {
        if (!context.victim.Equals(PlayerID.instance.gameObject)) return;
        if (context.damage < minDamage || context.damage <= 0f) return;

        receivedMortalWound = (receivedMortalWound) ? true : context.isCriticalHit;

        float damageScale = Mathf.Clamp01(Mathf.InverseLerp(minDamage, maxDamage, context.damage));
        float damageOpacity = Mathf.Lerp(minOpacity, maxOpacity, damageScale);
        if (context.isCriticalHit) damageOpacity = mortalWoundOpacity;

        PlayDamagePulse(damageOpacity);
    }



    public void PlayDamagePulse(float pulseOpacity)
    {
        pulseOpacity = Mathf.Clamp((pulseOpacity + opacity), minOpacity, ((receivedMortalWound) ? mortalWoundOpacity : maxOpacity));
        SetBackgroundOpacity(pulseOpacity);
        initialOpacity = pulseOpacity;
        timer = opacityDecayTime;
    }



    private void SetBackgroundOpacity(float opacity)
    {
        Color newColor = background.color;
        newColor.a = opacity;
        background.color = newColor;
    }
}
