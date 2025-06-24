using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CreditsManager : MonoBehaviour
{
    [SerializeField] SpriteRenderer blackBG;
    [SerializeField] SpriteRenderer logo;
    [SerializeField] TextMeshProUGUI spaceEscapeText;
    [SerializeField] GameObject credits;
    [SerializeField] float fadeOutSpeed;
    [SerializeField] float fadeInSpeed;
    [SerializeField] float scrollSpeed;
    [SerializeField] float logoFadeSpeed;
    [SerializeField] float escapeFadeSpeed;

    private bool escapeFadeIn = true;
    private bool escapeFadeOut = false;
    private bool fadeOutBG = true;
    private bool isScrolling = false;
    private bool fadeInBG = false;
    private bool logoFadeIn = false;
    private bool logoFadeOut = false;
    private bool done1 = false;
    private bool done2 = false;
    private float alpha;
    private float alpha2;
    private float startTime;
    public float logoHoldTime;
    public float escapeHoldTime;
    public float logoBeforePause;
    public float logoAfterPause;

    // Start is called before the first frame update
    void Start()
    {
        alpha = blackBG.color.a;
        alpha2 = spaceEscapeText.color.a;
    }

    // Update is called once per frame
    void Update()
    {
        if (escapeFadeIn || escapeFadeOut) FadeEscapeText();
        if (fadeOutBG) FadeOutBlackBG();
        if (isScrolling) StartScrolling();
        if (fadeInBG) FadeInBlackBG();
        if (logoFadeIn || logoFadeOut) LogoFading();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("TitleScene");
        }
        
        if (logoFadeOut && !done2 && startTime + logoHoldTime <= Time.time)
        {
            done2 = true;
        }

        if (!escapeFadeIn && !escapeFadeOut && !done1 && startTime + escapeHoldTime <= Time.time)
        {
            escapeFadeOut = true;
            done1 = true;
        }
    }

    /// <summary>
    /// Let the black BG fade out
    /// </summary>
    void FadeOutBlackBG()
    {
        blackBG.color = new Color(blackBG.color.r, blackBG.color.g, blackBG.color.b,
                        alpha - fadeOutSpeed * Time.deltaTime);
        alpha = blackBG.color.a;

        if (blackBG.color.a <= 0)
        {
            fadeOutBG = false;
            isScrolling = true;
        }
    }

    /// <summary>
    /// Start rolling the credits
    /// </summary>
    void StartScrolling()
    {
        credits.transform.position = new Vector2(credits.transform.position.x,
                        credits.transform.position.y + scrollSpeed * Time.deltaTime);

        // TODO: when it reaches someplace, turn scrolling off and fade in
        if (credits.transform.localPosition.y >= 14000)
        {
            isScrolling = false;
            fadeInBG = true;
        }
    }

    /// <summary>
    /// Let the black BG fade in
    /// </summary>
    void FadeInBlackBG()
    {
        blackBG.color = new Color(blackBG.color.r, blackBG.color.g, blackBG.color.b,
                        alpha + fadeInSpeed * Time.deltaTime);
        alpha = blackBG.color.a;

        if (blackBG.color.a >= 1)
        {
            fadeInBG = false;
            StartCoroutine(LogoBeforePause());
            alpha2 = logo.color.a;
        }
    }

    /// <summary>
    /// Let the SIGGD logo fade in and out
    /// </summary>
    void LogoFading()
    {
        if (logoFadeIn)
        {
            logo.color = new Color(logo.color.r, logo.color.g, logo.color.b,
                        alpha2 += logoFadeSpeed * Time.deltaTime);
            alpha2 = logo.color.a;

            if (alpha2 >= 1)
            {
                logoFadeIn = false;
                logoFadeOut = true;
                startTime = Time.time;
            }
        }

        if (logoFadeOut && done2)
        {
            logo.color = new Color(logo.color.r, logo.color.g, logo.color.b,
                        alpha2 -= logoFadeSpeed * Time.deltaTime);
            alpha2 = logo.color.a;

            if (alpha2 <= 0)
            {
                StartCoroutine(LogoAfterPause());
            }
        }
    }

    /// <summary>
    /// Fade the text that says the space bar can escape the credits scene in and out
    /// </summary>
    void FadeEscapeText()
    {
        if (escapeFadeIn)
        {
            spaceEscapeText.color = new Color(spaceEscapeText.color.r, spaceEscapeText.color.g, 
                        spaceEscapeText.color.b, alpha2 += escapeFadeSpeed * Time.deltaTime);
            alpha2 = spaceEscapeText.color.a;

            if (alpha2 >= 1)
            {
                escapeFadeIn = false;
                startTime = Time.time;
            }
        }

        if (escapeFadeOut)
        {
            spaceEscapeText.color = new Color(spaceEscapeText.color.r, spaceEscapeText.color.g,
                        spaceEscapeText.color.b, alpha2 -= escapeFadeSpeed * Time.deltaTime);
            alpha2 = spaceEscapeText.color.a;

            if (alpha2 <= 0)
            {
                escapeFadeOut = false;
            }
        }
    }

    IEnumerator LogoBeforePause()
    {
        yield return new WaitForSeconds(logoBeforePause);
        logoFadeIn = true;
    }

    IEnumerator LogoAfterPause()
    {
        yield return new WaitForSeconds(logoAfterPause);
        SceneManager.LoadScene("TitleScene");
    }
}
