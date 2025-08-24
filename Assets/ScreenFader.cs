using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenFader : MonoBehaviour
{
    [SerializeField] public float fadeInDelay;
    [SerializeField] public float fadeInDuration;
    [SerializeField] public float fadeOutDelay;
    [SerializeField] public float fadeOutDuration;
    [SerializeField] private Color fullyFadedColor;

    [HideInInspector] public static ScreenFader instance;

    private SpriteRenderer spriteRenderer;

    private bool isFadingIn = false;
    private float fadeInRate = 0f;
    private bool isFadingOut = false;
    private float fadeOutRate = 0f;



    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (SceneManager.GetActiveScene().name.Equals("Eva Start Fractal Hub") ||
            SceneManager.GetActiveScene().name.Equals("HubWorld"))
        {
            return;
        }
        FadeIn();
    }

    // Update is called once per frame
    void Update()
    {
        if (isFadingIn)
        {
            // Update fade in
            float alpha = Mathf.Max(spriteRenderer.color.a - (fadeInRate * Time.deltaTime), 0f);
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);

            // End fade in
            if (alpha <= 0f)
            {
                isFadingIn = false;
                spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0f);
                spriteRenderer.enabled = false;
            }
        }

        if (isFadingOut)
        {
            // Update fade out
            float alpha = Mathf.Min(spriteRenderer.color.a + (fadeOutRate * Time.deltaTime), 1f);
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);

            // End fade out
            if (alpha >= 1f)
            {
                isFadingOut = false;
                spriteRenderer.color = fullyFadedColor;
                spriteRenderer.enabled = true;
            }
        }
    }



    public void FadeIn()
    {
        StartCoroutine(FadeInCoroutine(fadeInDelay, fadeInDuration));
    }

    public void FadeIn(float fadeInDelay, float fadeInDuration)
    {
        StartCoroutine(FadeInCoroutine(fadeInDelay, fadeInDuration));
    }

    private IEnumerator FadeInCoroutine(float fadeInDelay, float fadeInDuration)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = true;
        spriteRenderer.color = fullyFadedColor;
        yield return new WaitForSeconds(fadeInDelay);

        fadeInRate = 1f / fadeInDuration;
        isFadingIn = true;
    }



    public void FadeOut()
    {
        StartCoroutine(FadeOutCoroutine(fadeOutDelay, fadeOutDuration));
    }

    public void FadeOut(float fadeOutDelay, float fadeOutDuration)
    {
        StartCoroutine(FadeOutCoroutine(fadeOutDelay, fadeOutDuration));
    }

    private IEnumerator FadeOutCoroutine(float fadeOutDelay, float fadeOutDuration)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = true;
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0f);
        yield return new WaitForSeconds(fadeOutDelay);

        fadeOutRate = 1f / fadeOutDuration;
        isFadingOut = true;
    }
}
