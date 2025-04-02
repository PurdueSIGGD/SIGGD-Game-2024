using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class PlayerInWorldMeterUIManager : MonoBehaviour
{
    public static PlayerInWorldMeterUIManager instance;

    [SerializeField] private Image frameBackground;
    [SerializeField] private Slider meterSlider;
    [SerializeField] private Image meterBackground;
    [SerializeField] private Image meterBar;

    [SerializeField] private float inactiveYPositionOffset;
    [SerializeField] private float fadeInDurationTime;
    [SerializeField] private float fadeOutDurationTime;

    private bool isActive;
    private bool isActivating;
    private bool isDeactivating;

    //private float yOffset;
    public float activeYPosition;
    public float inactiveYPosition;

    private float alphaMultiplier;
    private float activeFrameBackgroundAlpha;
    private float activeMeterBackgroundAlpha;
    private float activeMeterBarAlpha;

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

        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }


    
    // UI SETTER ENDPOINTS
    public void activateWidget(float delayTime)
    {
        if (isActive) return;
        isActive = true;
        gameObject.SetActive(true);
        StartCoroutine(animateWidgetStateChange(true, delayTime));
    }

    public void deactivateWidget(float delayTime)
    {
        if (!isActive) return;
        isActive = false;
        StartCoroutine(animateWidgetStateChange(false, delayTime));
    }

    public void updateMeterValue(float currentValue, float maxValue)
    {
        meterSlider.maxValue = maxValue;
        meterSlider.value = currentValue;
    }

    public void updateMeterColor(Color color, bool preserveAlpha)
    {
        setImageColor(meterBar, color, preserveAlpha);
    }

    public void updateBackgroundColor(Color color, bool preserveAlpha)
    {
        setImageColor(frameBackground, color, preserveAlpha);
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
            yield return new WaitForSeconds(((activate) ? fadeInDurationTime : fadeOutDurationTime) / (float) baseStep);
        }
        transform.localPosition = new Vector3(transform.localPosition.x, ((activate) ? activeYPosition : inactiveYPosition), transform.localPosition.z);
        alphaMultiplier = (activate) ? 1f : 0f;
        frameBackground.color = new Color(frameBackground.color.r, frameBackground.color.g, frameBackground.color.b, activeFrameBackgroundAlpha * alphaMultiplier);
        meterBackground.color = new Color(meterBackground.color.r, meterBackground.color.g, meterBackground.color.b, activeMeterBackgroundAlpha * alphaMultiplier);
        meterBar.color = new Color(meterBar.color.r, meterBar.color.g, meterBar.color.b, activeMeterBarAlpha * alphaMultiplier);
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
