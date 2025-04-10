using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class PlayerAbilityUIManager : MonoBehaviour
{
    [SerializeField] private Image frame;
    [SerializeField] private Image flareOverlay;
    [SerializeField] private Slider meterSlider;
    [SerializeField] private Image meterBackground;
    [SerializeField] private Image meterBar;

    [SerializeField] private Image iconBackground;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI number;

    [SerializeField] private GameObject subNumberWidget;
    [SerializeField] private Image subNumberFrame;
    [SerializeField] private Image subNumberBackground;
    [SerializeField] private TextMeshProUGUI subNumber;

    private Color baseActiveIconBackgroundColor;
    private Color baseInactiveIconBackgroundColor;

    private Color basePartialSubNumberBackgroundColor;
    private Color baseFullSubNumberBackgroundColor;

    private float minHighlightedFlareAlpha;
    private float maxHighlightedFlareAlpha;
    private float highlightedFlarePulseDurationTime;
    private bool isHighlighted;
    private bool isHighlightedBuffer;

    private bool isEnabled;



    // Start is called before the first frame update
    void Start()
    {
        baseActiveIconBackgroundColor = meterBar.color;
        baseInactiveIconBackgroundColor = iconBackground.color;
        if (subNumberBackground != null) basePartialSubNumberBackgroundColor = subNumberBackground.color;
        if (subNumber != null) baseFullSubNumberBackgroundColor = subNumber.color;

        minHighlightedFlareAlpha = 0f;
        maxHighlightedFlareAlpha = 1f;
        highlightedFlarePulseDurationTime = 0.8f;
        isHighlighted = false;
        isHighlightedBuffer = false;

        isEnabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    // UI SETTER ENDPOINTS

    /// <summary>
    /// Set the values shown by the ring meter UI.
    /// </summary>
    /// <param name="currentValue">The value represented by the meter's fill bar.</param>
    /// <param name="maxValue">The value represented by the meter's overall bar.</param>
    public void setMeterValue(float currentValue, float maxValue)
    {
        meterSlider.maxValue = maxValue;
        meterSlider.value = currentValue;
    }

    /// <summary>
    /// Set the value shown by the central number field.
    /// </summary>
    /// <param name="value">The value represented by the number field.</param>
    public void setNumberValue(int value)
    {
        number.text = value.ToString();
    }

    /// <summary>
    /// Set the value shown by the central number field.
    /// </summary>
    /// <param name="value">The value represented by the number field. Float value is ceiled to an int.</param>
    public void setNumberValue(float value)
    {
        number.text = Mathf.CeilToInt(value).ToString();
    }

    /// <summary>
    /// Show or hide the central number field.
    /// </summary>
    /// <param name="active">If true, the number field is shown. If false, the number field is hidden.</param>
    public void setNumberActive(bool active)
    {
        number.gameObject.SetActive(active);
    }

    /// <summary>
    /// Set the ability icon.
    /// </summary>
    /// <param name="iconSprite">The ability icon.</param>
    public void setIcon(Sprite iconSprite)
    {
        icon.sprite = iconSprite;
    }

    /// <summary>
    /// Set the ability to appear as enabled or disabled.
    /// </summary>
    /// <param name="enabled">If true, the abilty will appear enabled. If false, the ability will appear disabled.</param>
    public void setAbilityEnabled(bool enabled)
    {
        setAbilityEnabled(enabled, false);
    }

    /// <summary>
    /// Set the ability to appear as enabled or disabled.
    /// </summary>
    /// <param name="enabled">If true, the ability will appear enabled. If false, the ability will appear disabled.</param>
    /// <param name="pingOnReenable">If true, the ability will play a ping animation if this function call causes it to switch from disabled to enabled.</param>
    public void setAbilityEnabled(bool enabled, bool pingOnReenable)
    {
        Color color = enabled ? baseActiveIconBackgroundColor : baseInactiveIconBackgroundColor;
        if (pingOnReenable && enabled && !isEnabled && !isHighlighted) pingAbility();
        setImageColor(iconBackground, color, false);
        isEnabled = enabled;
    }

    /// <summary>
    /// Set the ability's current cooldown time. If off-cooldown, the ability appears enabled and its meter is filled. If on-cooldown, the ability appears disabled, and its meter and central number shows the cooldown time.
    /// </summary>
    /// <param name="currentCooldownTime">The current cooldown time of this ability.</param>
    /// <param name="totalCooldownTime">The total cooldown time of this ability.</param>
    public void setAbilityCooldownTime(float currentCooldownTime, float totalCooldownTime)
    {
        if (currentCooldownTime <= 0f)
        {
            setAbilityEnabled(true, true);
            setNumberActive(false);
            setMeterValue(1f, 1f);
            return;
        }
        setAbilityEnabled(false);
        setNumberActive(true);
        setNumberValue(currentCooldownTime);
        setMeterValue((totalCooldownTime - currentCooldownTime), totalCooldownTime);
    }

    /// <summary>
    /// Set the color of the ability widget frame.
    /// </summary>
    /// <param name="color">The color for the frame. The alpha value is ignored.</param>
    public void setFrameColor(Color color)
    {
        setImageColor(frame, color, true);
        if (subNumberFrame != null) setImageColor(subNumberFrame, color, true);
    }

    /// <summary>
    /// Set the value shown by the offset charge number.
    /// </summary>
    /// <param name="currentValue">The current charge value.</param>
    /// <param name="maxValue">The maximum charge value.</param>
    public void setChargeValue(int currentValue, int maxValue)
    {
        if (subNumber == null) return;
        subNumber.text = currentValue.ToString();
        Color backgroundColor = (currentValue >= maxValue) ? baseFullSubNumberBackgroundColor : basePartialSubNumberBackgroundColor;
        setImageColor(subNumberBackground, backgroundColor, true);
        Color numberColor = (currentValue >= maxValue) ? icon.color : baseFullSubNumberBackgroundColor;
        subNumber.color = numberColor;
    }

    /// <summary>
    /// Set the value shown by the offset charge number.
    /// </summary>
    /// <param name="currentValue">The current charge value.</param>
    /// <param name="maxValue">The maximum charge value.</param>
    public void setChargeValue(float currentValue, float maxValue)
    {
        if (subNumber == null) return;
        subNumber.text = Mathf.CeilToInt(currentValue).ToString();
        Color backgroundColor = (currentValue >= maxValue) ? baseFullSubNumberBackgroundColor : basePartialSubNumberBackgroundColor;
        setImageColor(subNumberBackground, backgroundColor, true);
        Color numberColor = (currentValue >= maxValue) ? icon.color : baseFullSubNumberBackgroundColor;
        subNumber.color = numberColor;
    }

    /// <summary>
    /// Show or hide the offset charge number widget.
    /// </summary>
    /// <param name="active">If true, the widget's gameobject will be activated. If false, the widget's gameobject will be deactivated.</param>
    public void setChargeWidgetActive(bool active)
    {
        subNumberWidget.SetActive(active);
    }

    /// <summary>
    /// Set the ability to appear highlighted or not highlighted. While highlighted, the ability will play a looping ping animation.
    /// </summary>
    /// <param name="highlighted">If true, the ability will appear highlighted. If false, the ability will not appear highlighted.</param>
    public void setAbilityHighlighted(bool highlighted)
    {
        if ((highlighted && isHighlighted) || (!highlighted && !isHighlighted)) return;
        isHighlightedBuffer = highlighted;
        flareOverlay.GetComponent<Image>().enabled = highlighted;
        if (highlighted) StartCoroutine(animateHighlightedFlare());
    }

    /// <summary>
    /// Play a ping animation.
    /// </summary>
    public void pingAbility()
    {
        StartCoroutine(animatePingFlare(false));
    }

    /// <summary>
    /// Show or hide the ability.
    /// </summary>
    /// <param name="active">If true, each of the ability's gameobjects will be activated. If false, each of the ability's gameobjects will be deactivated and the ability will stop being highlighted.</param>
    public void setUIActive(bool active)
    {
        if (!active) isHighlightedBuffer = false;
        frame.gameObject.SetActive(active);
        flareOverlay.gameObject.SetActive(active);
        meterSlider.gameObject.SetActive(active);
        meterBackground.gameObject.SetActive(active);
        meterBar.gameObject.SetActive(active);
        iconBackground.gameObject.SetActive(active);
        icon.gameObject.SetActive(active);
        number.gameObject.SetActive(active);
        subNumberWidget.SetActive(active);
    }



    // UTILITIES

    private void setImageColor(Image image, Color color, bool preserveAlpha)
    {
        Color newColor = color;
        if (preserveAlpha) newColor.a = image.color.a;
        image.color = newColor;
    }

    private IEnumerator animateHighlightedFlare()
    {
        while (isHighlighted || isHighlightedBuffer)
        {
            yield return animatePingFlare(true);
        }
    }

    private IEnumerator animatePingFlare(bool isHighlightPing)
    {
        float initialAlpha = maxHighlightedFlareAlpha;
        float finalAlpha = minHighlightedFlareAlpha;
        int step = 20;
        for (int i = 0; i < step; i++)
        {
            isHighlighted = isHighlightedBuffer;
            if (!isHighlighted && isHighlightPing) yield break;
            float currentAlpha = Mathf.Lerp(initialAlpha, finalAlpha, (float) i / (float) step);
            Color color = flareOverlay.color;
            color.a = currentAlpha;
            flareOverlay.color = color;
            yield return new WaitForSeconds(highlightedFlarePulseDurationTime / (float) step);
            yield return new WaitForEndOfFrame();
        }
        Color finalColor = flareOverlay.color;
        finalColor.a = finalAlpha;
        flareOverlay.color = finalColor;
    }
}
