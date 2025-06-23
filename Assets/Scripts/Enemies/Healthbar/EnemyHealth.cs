using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{

    [SerializeField] private Image frameBackground1;
    [SerializeField] private Image frameBackground2;
    [SerializeField] private Slider healthMeterSlider;
    [SerializeField] private Image healthMeterBackground;
    [SerializeField] private Image healthMeterBar;

    [SerializeField] private Slider stunMeterSlider;
    [SerializeField] private Image stunMeterBackground;
    [SerializeField] private Image stunMeterBar;

    [SerializeField] private Slider delayedMeterSlider;
    [SerializeField] private Image delayedMeterBar;

    [SerializeField] private float inactiveYPositionOffset;
    [SerializeField] private float fadeInDurationTime;
    [SerializeField] private float fadeOutDurationTime;

    private bool isActive;
    private bool isActivating;
    private bool isDeactivating;

    public float activeYPosition;
    public float inactiveYPosition;

    private float alphaMultiplier;
    private float activeFrameBackgroundAlpha;
    private float activeMeterBackgroundAlpha;
    private float activeMeterBarAlpha;
    private float activeSubMeterBackgroundAlpha;
    private float activeSubMeterBarAlpha;

    private Color baseBackgroundColor;
    private Color baseMeterColor;
    private Color baseSubMeterColor;

    // Health associated
    private Health health;
    private StunMeter stun;
    private StatManager stat;
    private EnemyStateManager enemyStateManager;
    private Quaternion UIRotation;
    private Vector3 UIScale;
    private float UIScaleFactor;

    void Start()
    {
        isActive = false;
        isActivating = false;
        isDeactivating = false;

        activeYPosition = transform.localPosition.y;
        inactiveYPosition = activeYPosition + inactiveYPositionOffset;
        transform.localPosition = new Vector3(transform.localPosition.x, inactiveYPosition, transform.localPosition.z);

        alphaMultiplier = 0f;
        activeFrameBackgroundAlpha = frameBackground1.color.a;
        activeMeterBackgroundAlpha = healthMeterBackground.color.a;
        activeMeterBarAlpha = healthMeterBar.color.a;
        activeSubMeterBackgroundAlpha = stunMeterBackground.color.a;
        activeSubMeterBarAlpha = stunMeterBar.color.a;

        baseBackgroundColor = frameBackground1.color;
        baseMeterColor = healthMeterBar.color;
        baseSubMeterColor = stunMeterBar.color;

        health = GetComponentInParent<Health>();
        stun = GetComponentInParent<StunMeter>();
        stat = GetComponentInParent<StatManager>();
        enemyStateManager = GetComponentInParent<EnemyStateManager>();
        UIRotation = new Quaternion(0f, transform.parent.rotation.y, 0f, 0f);
        UIScale = new Vector3(transform.parent.localScale.x, transform.localScale.y, transform.localScale.z);
        UIScaleFactor = transform.localScale.x;

        gameObject.SetActive(false);
        activateWidget();
    }

    void Update()
    {
        UIRotation.y = transform.parent.rotation.y;
        transform.localRotation = UIRotation;
        UIScale.x = (transform.parent.localScale.x / Mathf.Abs(transform.parent.localScale.x)) * UIScaleFactor;
        transform.localScale = UIScale;

        float maxHealth = stat.ComputeValue("Max Health");

        if (!TimeFreezeManager.GetIsActive())
        {
            delayedMeterSlider.maxValue = maxHealth;
            delayedMeterSlider.value = health.currentHealth;
        }

        healthMeterSlider.maxValue = maxHealth;
        healthMeterSlider.value = health.currentHealth;
        if (stun == null || enemyStateManager == null) return;
        stunMeterSlider.maxValue = stat.ComputeValue("Stun Threshold");
        stunMeterSlider.value = stat.ComputeValue("Stun Threshold") - stun.currentStun;
        stunMeterSlider.value = (enemyStateManager.StunState.isStunned) ? stunMeterSlider.maxValue : stunMeterSlider.value;
    }


    // UI SETTER ENDPOINTS

    /// <summary>
    /// Show the in-world meter UI.
    /// </summary>
    /// <param name="delayTime">Time before the meter UI is shown.</param>
    public void activateWidget(float delayTime)
    {
        if (isActive) return;
        isActive = true;
        gameObject.SetActive(true);
        StartCoroutine(animateWidgetStateChange(true, delayTime));
    }

    /// <summary>
    /// Show the in-world meter UI.
    /// </summary>
    public void activateWidget()
    {
        activateWidget(0f);
    }

    /// <summary>
    /// Hide the in-world meter UI.
    /// </summary>
    /// <param name="delayTime">Time before the meter UI is hidden.</param>
    public void deactivateWidget(float delayTime)
    {
        if (!isActive || !gameObject.activeInHierarchy) return;
        isActive = false;
        StartCoroutine(animateWidgetStateChange(false, delayTime));
    }

    /// <summary>
    /// Hide the in-world meter UI.
    /// </summary>
    public void deactivateWidget()
    {
        deactivateWidget(0f);
    }

    /// <summary>
    /// Set the values shown by the in-world meter UI.
    /// </summary>
    /// <param name="currentValue">The value represented by the meter's fill bar.</param>
    /// <param name="maxValue">The value represented by the meter's overall bar.</param>
    public void setMeterValue(float currentValue, float maxValue)
    {
        healthMeterSlider.maxValue = maxValue;
        healthMeterSlider.value = currentValue;
    }

    /// <summary>
    /// Set the color of the in-world meter UI's fill bar.
    /// </summary>
    /// <param name="color">The color for the fill bar. The alpha value is ignored.</param>
    public void setMeterColor(Color color)
    {
        setImageColor(healthMeterBar, color, true);
    }

    /// <summary>
    /// Reset the color of the in-world meter UI's fill bar to its default color.
    /// </summary>
    public void resetMeterColor()
    {
        setImageColor(healthMeterBar, baseMeterColor, true);
    }

    /// <summary>
    /// Set the values shown by the in-world sub meter UI.
    /// </summary>
    /// <param name="currentValue">The value represented by the sub meter's fill bar.</param>
    /// <param name="maxValue">The value represented by the sub meter's overall bar.</param>
    public void setSubMeterValue(float currentValue, float maxValue)
    {
        stunMeterSlider.maxValue = maxValue;
        stunMeterSlider.value = currentValue;
    }

    /// <summary>
    /// Set the color of the in-world sub meter UI's fill bar.
    /// </summary>
    /// <param name="color">The color for the fill bar. The alpha value is ignored.</param>
    public void setSubMeterColor(Color color)
    {
        setImageColor(stunMeterBar, color, true);
    }

    /// <summary>
    /// Reset the color of the in-world sub meter UI's fill bar to its default color.
    /// </summary>
    public void resetSubMeterColor()
    {
        setImageColor(stunMeterBar, baseSubMeterColor, true);
    }

    /// <summary>
    /// Set the color of the in-world meter UI's background frame.
    /// </summary>
    /// <param name="color">The color for the background frame. The alpha value is ignored.</param>
    public void setBackgroundColor(Color color)
    {
        setImageColor(frameBackground1, color, true);
    }

    /// <summary>
    /// Reset the color of the in-world meter UI's background frame to its default color.
    /// </summary>
    public void resetBackgroundColor()
    {
        setImageColor(frameBackground1, baseBackgroundColor, true);
    }



    // UTILITIES

    private void setImageColor(Image image, Color color, bool preserveAlpha)
    {
        Color newColor = color;
        if (preserveAlpha) newColor.a = image.color.a;
        image.color = newColor;
    }

    private IEnumerator animateWidgetStateChange(bool activate, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        if ((activate && !isActive) || (!activate && isActive)) yield break;
        isDeactivating = !activate;
        isActivating = activate;
        float initialYPosition = transform.localPosition.y;
        float initialAlpha = alphaMultiplier;
        int baseStep = 20;
        int step = (activate) ?
            (int)(baseStep * ((initialYPosition - activeYPosition) / (inactiveYPosition - activeYPosition))) :
            (int)(baseStep * (1 - ((initialYPosition - activeYPosition) / (inactiveYPosition - activeYPosition))));
        for (int i = 0; i < step; i++)
        {
            if ((activate && !isActivating) || (!activate && !isDeactivating)) yield break;
            float newYPosition = Mathf.Lerp(initialYPosition, ((activate) ? activeYPosition : inactiveYPosition), ((float)i / (float)step));
            transform.localPosition = new Vector3(transform.localPosition.x, newYPosition, transform.localPosition.z);
            alphaMultiplier = Mathf.Lerp(initialAlpha, ((activate) ? 1f : 0f), ((float)i / (float)step));
            frameBackground1.color = new Color(frameBackground1.color.r, frameBackground1.color.g, frameBackground1.color.b, activeFrameBackgroundAlpha * alphaMultiplier);
            healthMeterBackground.color = new Color(healthMeterBackground.color.r, healthMeterBackground.color.g, healthMeterBackground.color.b, activeMeterBackgroundAlpha * alphaMultiplier);
            healthMeterBar.color = new Color(healthMeterBar.color.r, healthMeterBar.color.g, healthMeterBar.color.b, activeMeterBarAlpha * alphaMultiplier);
            stunMeterBackground.color = new Color(stunMeterBackground.color.r, stunMeterBackground.color.g, stunMeterBackground.color.b, activeSubMeterBackgroundAlpha * alphaMultiplier);
            stunMeterBar.color = new Color(stunMeterBar.color.r, stunMeterBar.color.g, stunMeterBar.color.b, activeSubMeterBackgroundAlpha * alphaMultiplier);
            yield return new WaitForSeconds(((activate) ? fadeInDurationTime : fadeOutDurationTime) / (float)baseStep);
        }
        transform.localPosition = new Vector3(transform.localPosition.x, ((activate) ? activeYPosition : inactiveYPosition), transform.localPosition.z);
        alphaMultiplier = (activate) ? 1f : 0f;
        frameBackground1.color = new Color(frameBackground1.color.r, frameBackground1.color.g, frameBackground1.color.b, activeFrameBackgroundAlpha * alphaMultiplier);
        healthMeterBackground.color = new Color(healthMeterBackground.color.r, healthMeterBackground.color.g, healthMeterBackground.color.b, activeMeterBackgroundAlpha * alphaMultiplier);
        healthMeterBar.color = new Color(healthMeterBar.color.r, healthMeterBar.color.g, healthMeterBar.color.b, activeMeterBarAlpha * alphaMultiplier);
        stunMeterBackground.color = new Color(stunMeterBackground.color.r, stunMeterBackground.color.g, stunMeterBackground.color.b, activeSubMeterBackgroundAlpha * alphaMultiplier);
        stunMeterBar.color = new Color(stunMeterBar.color.r, stunMeterBar.color.g, stunMeterBar.color.b, activeSubMeterBackgroundAlpha * alphaMultiplier);
        if (activate)
        {
            isActivating = false;
        }
        else
        {
            isDeactivating = false;
            gameObject.SetActive(false);
        }
    }

}
