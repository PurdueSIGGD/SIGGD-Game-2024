using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleAreaHandler : MonoBehaviour
{
    [SerializeField] private float startDurationTime;
    [SerializeField] private float endDurationTime;
    [SerializeField] private float endFinalScaleMultiplier;
    [SerializeField] private float fadeOutDelayTime;
    [SerializeField] private float fadeOutDurationTime;

    private float startInitialScale;
    private float startFinalScale;
    private float startTimer = -1f;

    private float endInitialScale;
    private float endFinalScale;
    private float endTimer = -1f;

    // Start is called before the first frame update
    void Awake()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (startTimer > 0f)
        {
            startTimer -= Time.deltaTime;
            float currentScale = Mathf.Lerp(startInitialScale, startFinalScale, (startDurationTime - startTimer) / startDurationTime);
            transform.localScale = new Vector3(currentScale, currentScale, 1f);
            if (startTimer <= 0f)
            {
                startTimer = -1f;
            }
        }

        if (endTimer > 0f)
        {
            endTimer -= Time.deltaTime;
            float currentScale = Mathf.Lerp(endInitialScale, endFinalScale, (endDurationTime - endTimer) / endDurationTime);
            transform.localScale = new Vector3(currentScale, currentScale, 1f);
            if (endTimer <= 0f)
            {
                Destroy(gameObject);
            }
        }
    }

    /// <summary>
    /// Plays the circle area's activation animation. This GameObject will remain inactive until this function is called.
    /// </summary>
    /// <param name="circleRadius">The circle area's radius.</param>
    /// <param name="color">The circle area's color.</param>
    public void playCircleStart(float circleRadius, Color color)
    {
        gameObject.SetActive(true);
        animateCircleStart(circleRadius, color);
    }

    /// <summary>
    /// Plays the circle area's activation animation. This GameObject will remain inactive until this function is called.
    /// </summary>
    /// <param name="circleRadius">The circle area's radius.</param>
    /// <param name="color">The circle area's color.</param>
    /// <param name="alpha">The circle area's opacity.</param>
    public void playCircleStart(float circleRadius, Color color, float alpha)
    {
        gameObject.SetActive(true);
        animateCircleStart(circleRadius, color, alpha);
    }

    private void animateCircleStart(float circleRadius, Color color)
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = new Color(color.r, color.g, color.b, spriteRenderer.color.a);
        startInitialScale = 0.05f;
        startFinalScale = circleRadius * 2f;
        transform.localScale = new Vector3(startInitialScale, startInitialScale, 1f);
        /*
        int step = 20;
        for (int i = 0; i < step; i++)
        {
            float currentScale = Mathf.Lerp(initialScale, finalScale, ((float) i / (float) step));
            transform.localScale = new Vector3(currentScale, currentScale, 1f);
            yield return new WaitForSeconds(startDurationTime / (float) step);
        }
        */
        startTimer = startDurationTime;
    }

    private void animateCircleStart(float circleRadius, Color color, float alpha)
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = new Color(color.r, color.g, color.b, alpha);
        startInitialScale = 0.05f;
        startFinalScale = circleRadius * 2f;
        transform.localScale = new Vector3(startInitialScale, startInitialScale, 1f);
        /*
        int step = 20;
        for (int i = 0; i < step; i++)
        {
            float currentScale = Mathf.Lerp(initialScale, finalScale, ((float)i / (float)step));
            transform.localScale = new Vector3(currentScale, currentScale, 1f);
            yield return new WaitForSeconds(startDurationTime / (float)step);
        }
        */
        startTimer = startDurationTime;
    }

    /// <summary>
    /// Plays the cirle area's deactivation animation. This GameObject is destroyed shortly after this function is called.
    /// </summary>
    public void playCircleEnd()
    {
        animateCircleEnd();
        StartCoroutine(fadeOutCircle());
    }

    private void animateCircleEnd()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        endInitialScale = transform.localScale.x;
        endFinalScale = endInitialScale * endFinalScaleMultiplier;
        transform.localScale = new Vector3(endInitialScale, endInitialScale, 1f);
        /*
        int step = 20;
        for (int i = 0; i < step; i++)
        {
            float currentScale = Mathf.Lerp(initialScale, finalScale, ((float) i / (float) step));
            transform.localScale = new Vector3(currentScale, currentScale, 1f);
            yield return new WaitForSeconds(startDurationTime / (float) step);
        }
        Destroy(gameObject);
        */
        endTimer = endDurationTime;
    }

    private IEnumerator fadeOutCircle()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        yield return new WaitForSeconds(fadeOutDelayTime);
        float initialAlpha = spriteRenderer.color.a;
        int step = 20;
        for (int i = 0; i < step; i++)
        {
            float currentAlpha = Mathf.Lerp(initialAlpha, 0f, (float) i / (float) step);
            Color color = spriteRenderer.color;
            color.a = currentAlpha;
            spriteRenderer.color = color;
            yield return new WaitForSeconds(fadeOutDurationTime / (float) step);
        }
    }
}
