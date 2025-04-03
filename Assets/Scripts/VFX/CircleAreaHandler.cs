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

    // Start is called before the first frame update
    void Awake()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Plays the circle area's activation animation. This GameObject will remain inactive until this function is called.
    /// </summary>
    /// <param name="circleRadius">The circle area's radius.</param>
    /// <param name="color">The circle area's color.</param>
    public void playCircleStart(float circleRadius, Color color)
    {
        gameObject.SetActive(true);
        StartCoroutine(animateCircleStart(circleRadius, color));
    }

    private IEnumerator animateCircleStart(float circleRadius, Color color)
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = new Color(color.r, color.g, color.b, spriteRenderer.color.a);
        float initialScale = 0.05f;
        float finalScale = circleRadius * 2f;
        transform.localScale = new Vector3(initialScale, initialScale, 1f);
        int step = 20;
        for (int i = 0; i < step; i++)
        {
            float currentScale = Mathf.Lerp(initialScale, finalScale, ((float) i / (float) step));
            transform.localScale = new Vector3(currentScale, currentScale, 1f);
            yield return new WaitForSeconds(startDurationTime / (float) step);
        }
    }

    /// <summary>
    /// Plays the cirle area's deactivation animation. This GameObject is destroyed shortly after this function is called.
    /// </summary>
    public void playCircleEnd()
    {
        StartCoroutine(animateCircleEnd());
        StartCoroutine(fadeOutCircle());
    }

    private IEnumerator animateCircleEnd()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        float initialScale = transform.localScale.x;
        float finalScale = initialScale * endFinalScaleMultiplier;
        transform.localScale = new Vector3(initialScale, initialScale, 1f);
        int step = 20;
        for (int i = 0; i < step; i++)
        {
            float currentScale = Mathf.Lerp(initialScale, finalScale, ((float) i / (float) step));
            transform.localScale = new Vector3(currentScale, currentScale, 1f);
            yield return new WaitForSeconds(startDurationTime / (float) step);
        }
        Destroy(gameObject);
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
