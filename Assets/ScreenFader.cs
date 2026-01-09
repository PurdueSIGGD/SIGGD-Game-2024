using System.Collections;
using System.Collections.Generic;
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

    private List<PlayerGhostUIManager> playerGhostUI;

    private SpriteRenderer spriteRenderer;

    private bool isFadingIn = false;
    private float fadeInRate = 0f;
    private bool isFadingOut = false;
    private float fadeOutRate = 0f;



    private void Awake()
    {
        instance = this;
        playerGhostUI = new();
        if (PlayerSelectedGhostUIManager.instance) playerGhostUI.Add(PlayerSelectedGhostUIManager.instance);
        if (PlayerGhost1UIManager.instance) playerGhostUI.Add(PlayerGhost1UIManager.instance);
        if (PlayerGhost2UIManager.instance) playerGhostUI.Add(PlayerGhost2UIManager.instance);
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
            FadeInPlayerUI(fadeInRate * Time.deltaTime);

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
            FadeInPlayerUI(-fadeOutRate * Time.deltaTime);

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
        if (fadeInDuration == 0) fadeInDuration = 1;

        FadeInPlayerUI(-9999f); // force playerui to be invisible first
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = true;
        spriteRenderer.color = fullyFadedColor;
        yield return new WaitForSeconds(fadeInDelay);

        fadeInRate = 1f / fadeInDuration;
        isFadingIn = true;
    }

    private void FadeInPlayerUI(float alpha)
    {
        foreach (PlayerGhostUIManager ui in playerGhostUI)
        {
            if (ui.gameObject.activeSelf)
            {
                ui.UpdateUIAlpha(alpha);
            }
        }
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
