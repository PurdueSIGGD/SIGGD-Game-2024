using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenFader : MonoBehaviour
{
    [SerializeField] public float fadeInDelay;
    [SerializeField] public float fadeInDuration;
    [SerializeField] public float fadeOutDelay;
    [SerializeField] public float fadeOutDuration;
    [SerializeField] private Color fullyFadedColor;
    [SerializeField] private Image image;

    [HideInInspector] public static ScreenFader instance;

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
            float alpha = Mathf.Max(image.color.a - (fadeInRate * Time.deltaTime), 0f);
            image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);

            // End fade in
            if (alpha <= 0f)
            {
                isFadingIn = false;
                image.color = new Color(image.color.r, image.color.g, image.color.b, 0f);
                image.enabled = false;
            }
        }

        if (isFadingOut)
        {
            // Update fade out
            float alpha = Mathf.Min(image.color.a + (fadeOutRate * Time.deltaTime), 1f);
            image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);

            // End fade out
            if (alpha >= 1f)
            {
                isFadingOut = false;
                image.color = fullyFadedColor;
                image.enabled = true;
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
        image.enabled = true;
        image.color = fullyFadedColor;
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
        image.enabled = true;
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0f);
        yield return new WaitForSeconds(fadeOutDelay);

        fadeOutRate = 1f / fadeOutDuration;
        isFadingOut = true;
    }
}
