using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInWorldMeterUIManager : MonoBehaviour
{
    public static PlayerInWorldMeterUIManager instance;

    [SerializeField] private Image frameBackground;
    [SerializeField] private Slider meterSlider;
    [SerializeField] private Image meterBackground;
    [SerializeField] private Image meterBar;
    [SerializeField] private Slider subMeterSlider;
    [SerializeField] private Image subMeterBackground;
    [SerializeField] private Image subMeterBar;

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

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        isActive = false;
        isActivating = false;
        isDeactivating = false;

        activeYPosition = transform.localPosition.y;
        inactiveYPosition = activeYPosition + inactiveYPositionOffset;
        transform.localPosition = new Vector3(transform.localPosition.x, inactiveYPosition, transform.localPosition.z);

        alphaMultiplier = 0f;
        activeFrameBackgroundAlpha = frameBackground.color.a;
        activeMeterBackgroundAlpha = meterBackground.color.a;
        activeMeterBarAlpha = meterBar.color.a;
        activeSubMeterBackgroundAlpha = subMeterBackground.color.a;
        activeSubMeterBarAlpha = subMeterBar.color.a;

        baseBackgroundColor = frameBackground.color;
        baseMeterColor = meterBar.color;
        baseSubMeterColor = subMeterBar.color;

        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

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
        meterSlider.maxValue = maxValue;
        meterSlider.value = currentValue;
    }

    /// <summary>
    /// Set the color of the in-world meter UI's fill bar.
    /// </summary>
    /// <param name="color">The color for the fill bar. The alpha value is ignored.</param>
    public void setMeterColor(Color color)
    {
        setImageColor(meterBar, color, true);
    }

    /// <summary>
    /// Reset the color of the in-world meter UI's fill bar to its default color.
    /// </summary>
    public void resetMeterColor()
    {
        setImageColor(meterBar, baseMeterColor, true);
    }

    /// <summary>
    /// Set the values shown by the in-world sub meter UI.
    /// </summary>
    /// <param name="currentValue">The value represented by the sub meter's fill bar.</param>
    /// <param name="maxValue">The value represented by the sub meter's overall bar.</param>
    public void setSubMeterValue(float currentValue, float maxValue)
    {
        subMeterSlider.maxValue = maxValue;
        subMeterSlider.value = currentValue;
    }

    /// <summary>
    /// Set the color of the in-world sub meter UI's fill bar.
    /// </summary>
    /// <param name="color">The color for the fill bar. The alpha value is ignored.</param>
    public void setSubMeterColor(Color color)
    {
        setImageColor(subMeterBar, color, true);
    }

    /// <summary>
    /// Reset the color of the in-world sub meter UI's fill bar to its default color.
    /// </summary>
    public void resetSubMeterColor()
    {
        setImageColor(subMeterBar, baseSubMeterColor, true);
    }

    /// <summary>
    /// Set the color of the in-world meter UI's background frame.
    /// </summary>
    /// <param name="color">The color for the background frame. The alpha value is ignored.</param>
    public void setBackgroundColor(Color color)
    {
        setImageColor(frameBackground, color, true);
    }

    /// <summary>
    /// Reset the color of the in-world meter UI's background frame to its default color.
    /// </summary>
    public void resetBackgroundColor()
    {
        setImageColor(frameBackground, baseBackgroundColor, true);
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
            (int) (baseStep * ((initialYPosition - activeYPosition) / (inactiveYPosition - activeYPosition))) :
            (int) (baseStep * (1 - ((initialYPosition - activeYPosition) / (inactiveYPosition - activeYPosition))));
        for (int i = 0; i < step; i++)
        {
            if ((activate && !isActivating) || (!activate && !isDeactivating)) yield break;
            float newYPosition = Mathf.Lerp(initialYPosition, ((activate) ? activeYPosition : inactiveYPosition), ((float) i / (float) step));
            transform.localPosition = new Vector3(transform.localPosition.x, newYPosition, transform.localPosition.z);
            alphaMultiplier = Mathf.Lerp(initialAlpha, ((activate) ? 1f : 0f), ((float) i / (float) step));
            frameBackground.color = new Color(frameBackground.color.r, frameBackground.color.g, frameBackground.color.b, activeFrameBackgroundAlpha * alphaMultiplier);
            meterBackground.color = new Color(meterBackground.color.r, meterBackground.color.g, meterBackground.color.b, activeMeterBackgroundAlpha * alphaMultiplier);
            meterBar.color = new Color(meterBar.color.r, meterBar.color.g, meterBar.color.b, activeMeterBarAlpha * alphaMultiplier);
            subMeterBackground.color = new Color(subMeterBackground.color.r, subMeterBackground.color.g, subMeterBackground.color.b, activeSubMeterBackgroundAlpha * alphaMultiplier);
            subMeterBar.color = new Color(subMeterBar.color.r, subMeterBar.color.g, subMeterBar.color.b, activeSubMeterBackgroundAlpha * alphaMultiplier);
            yield return new WaitForSeconds(((activate) ? fadeInDurationTime : fadeOutDurationTime) / (float) baseStep);
        }
        transform.localPosition = new Vector3(transform.localPosition.x, ((activate) ? activeYPosition : inactiveYPosition), transform.localPosition.z);
        alphaMultiplier = (activate) ? 1f : 0f;
        frameBackground.color = new Color(frameBackground.color.r, frameBackground.color.g, frameBackground.color.b, activeFrameBackgroundAlpha * alphaMultiplier);
        meterBackground.color = new Color(meterBackground.color.r, meterBackground.color.g, meterBackground.color.b, activeMeterBackgroundAlpha * alphaMultiplier);
        meterBar.color = new Color(meterBar.color.r, meterBar.color.g, meterBar.color.b, activeMeterBarAlpha * alphaMultiplier);
        subMeterBackground.color = new Color(subMeterBackground.color.r, subMeterBackground.color.g, subMeterBackground.color.b, activeSubMeterBackgroundAlpha * alphaMultiplier);
        subMeterBar.color = new Color(subMeterBar.color.r, subMeterBar.color.g, subMeterBar.color.b, activeSubMeterBackgroundAlpha * alphaMultiplier);
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
