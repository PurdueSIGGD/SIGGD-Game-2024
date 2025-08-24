using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealingReceivedBackground : MonoBehaviour
{
    [SerializeField] private float minHealing;
    [SerializeField] private float minOpacity;
    [SerializeField] private float maxHealing;
    [SerializeField] private float maxOpacity;
    [SerializeField] private float mortalWoundOpacity;
    [SerializeField] private float opacityDecayTime;

    private Image background;
    private float initialOpacity = 0f;
    private float opacity = 0f;
    private float timer = 0f;
    private bool healedMortalWound = false;


    private void OnEnable()
    {
        GameplayEventHolder.OnHealingDealt += OnHealingReceived;
    }

    private void OnDestroy()
    {
        GameplayEventHolder.OnHealingDealt -= OnHealingReceived;
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

        if (healedMortalWound && opacity == 0f)
        {
            healedMortalWound = false;
        }
    }



    private void OnHealingReceived(HealingContext context)
    {
        if (!context.healee.Equals(PlayerID.instance.gameObject)) return;
        if (context.healing < minHealing || context.healing <= 0f) return;

        //healedMortalWound = (healedMortalWound) ? true : context.isCriticalHit;

        float healingScale = Mathf.Clamp01(Mathf.InverseLerp(minHealing, maxHealing, context.healing));
        float healingOpacity = Mathf.Lerp(minOpacity, maxOpacity, healingScale);
        //if (context.isCriticalHit) damageOpacity = mortalWoundOpacity;

        PlayDamagePulse(healingOpacity);
    }



    public void PlayDamagePulse(float pulseOpacity)
    {
        pulseOpacity = Mathf.Clamp((pulseOpacity + opacity), minOpacity, ((healedMortalWound) ? mortalWoundOpacity : maxOpacity));
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
