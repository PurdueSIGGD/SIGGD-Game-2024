using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAbilityUIManager : MonoBehaviour
{

    [SerializeField] private Image frame;
    [SerializeField] private Slider meterSlider;
    [SerializeField] private Image meterBackground;
    [SerializeField] private Image meterBar;

    [SerializeField] private Image iconBackground;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI number;

    [SerializeField] private Image subIconFrame;
    [SerializeField] private Image subIconBackground;
    [SerializeField] private Image subIcon;

    [SerializeField] private Image subNumberFrame;
    [SerializeField] private Image subNumberBackground;
    [SerializeField] private Image subNumber;

    private Color baseActiveIconBackgroundColor;
    private Color baseInactiveIconBackgroundColor;


    // Start is called before the first frame update
    void Start()
    {
        baseActiveIconBackgroundColor = meterBar.color;
        baseInactiveIconBackgroundColor = iconBackground.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    // UI SETTER ENDPOINTS

    /// <summary>
    /// Set the values shown by the in-world meter UI.
    /// </summary>
    /// <param name="currentValue">The value represented by the meter's fill bar.</param>
    /// <param name="maxValue">The value represented by the meter's overall bar.</param>
    public void updateMeterValue(float currentValue, float maxValue)
    {
        meterSlider.maxValue = maxValue;
        meterSlider.value = currentValue;
    }

    /// <summary>
    /// Set the value shown by the central number field.
    /// </summary>
    /// <param name="value">The value represented by the number field.</param>
    public void updateNumberValue(int value)
    {
        number.text = value.ToString();
    }

    /// <summary>
    /// Set the value shown by the central number field.
    /// </summary>
    /// <param name="value">The value represented by the number field. Float value is ceiled to an int.</param>
    public void updateNumberValue(float value)
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

    public void updateIcon(Sprite iconSprite)
    {
        icon.sprite = iconSprite;
    }

    public void setAbilityEnabled(bool enabled)
    {
        iconBackground.color = enabled ? baseActiveIconBackgroundColor : baseInactiveIconBackgroundColor;
    }

    public void updateAbilityCooldownTime(float currentCooldownTime, float totalCooldownTime)
    {
        if (currentCooldownTime <= 0f)
        {
            setAbilityEnabled(true);
            setNumberActive(false);
            updateMeterValue(1f, 1f);
            return;
        }
        setAbilityEnabled(false);
        setNumberActive(true);
        updateNumberValue(currentCooldownTime);
        updateMeterValue((totalCooldownTime - currentCooldownTime), totalCooldownTime);
    }

    /// <summary>
    /// Set the color of the ability widget frame.
    /// </summary>
    /// <param name="color">The color for the frame. The alpha value is ignored.</param>
    public void updateFrameColor(Color color)
    {
        setImageColor(frame, color, true);
    }



    // UTILITIES

    private void setImageColor(Image image, Color color, bool preserveAlpha)
    {
        Color newColor = color;
        if (preserveAlpha) newColor.a = image.color.a;
        image.color = newColor;
    }
}
