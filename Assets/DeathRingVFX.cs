using System.Collections;
using System.Collections.Generic;
using UnityEditor.Playables;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathRingVFX : MonoBehaviour
{
    [SerializeField] public float fadeInDelay;
    [SerializeField] public float fadeInDuration;

    [SerializeField] public float reviveSpinDelay;
    [SerializeField] public float reviveSpinCoefficient;
    [SerializeField] public float reviveSpinDegree;
    [SerializeField] public float reviveSpinDuration;

    [SerializeField] public float fadeOutDelay;
    [SerializeField] public float fadeOutDuration;

    [SerializeField] public float deathSpinInitialRotation;
    [SerializeField] public float deathSpinSpeed;

    [SerializeField] private Color fullyFadedColor;

    [HideInInspector] public static DeathRingVFX instance;

    private SpriteRenderer spriteRenderer;

    private bool isFadingIn = false;
    private float fadeInRate = 0f;

    private bool isDeathSpinning = false;

    private bool isFadingOut = false;
    private float fadeOutRate = 0f;

    private bool isReviveSpinning = false;
    private float reviveSpinTimer = 0f;



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
            PlayReviveAnimation();
        }
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
                isReviveSpinning = false;
                isFadingIn = false;
                spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0f);
                spriteRenderer.enabled = false;
            }
        }

        if (isDeathSpinning)
        {
            float rotation = transform.eulerAngles.z;
            rotation += (deathSpinSpeed * Time.deltaTime);
            //transform.rotation = new Quaternion(0f, 0f, Mathf.Min(rotation, 0f), transform.rotation.w);
            transform.eulerAngles = new Vector3(0f, 0f, rotation);
            /*
            if (rotation >= 0f)
            {
                isDeathSpinning = false;
                transform.eulerAngles = new Vector3(0f, 0f, 0f);
            }
            */
        }

        if (isFadingOut)
        {
            // Update fade out
            float alpha = Mathf.Min(spriteRenderer.color.a + (fadeOutRate * Time.deltaTime), 1f);
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);

            // End fade out
            if (alpha >= 1f)
            {
                isDeathSpinning = false;
                transform.eulerAngles = new Vector3(0f, 0f, 0f);
                isFadingOut = false;
                spriteRenderer.color = fullyFadedColor;
                spriteRenderer.enabled = true;
            }
        }

        if (isReviveSpinning)
        {
            reviveSpinTimer -= Time.deltaTime;
            if (reviveSpinTimer <= 0f && !isFadingIn)
            {
                isReviveSpinning = false;
                FadeIn();
                //return;
            }

            float zRotation = reviveSpinCoefficient * Mathf.Pow((reviveSpinDuration - reviveSpinTimer), reviveSpinDegree);
            //Quaternion rotation = new Quaternion(0f, 0f, transform.rotation.z, transform.rotation.w);
            //rotation.z = zRotation;
            //transform.rotation = rotation;
            transform.eulerAngles = new Vector3(0f, 0f, zRotation);
        }
    }





    public void PlayDeathAnimation()
    {
        FadeOut();
        StartDeathSpin();
    }



    public void PlayReviveAnimation()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = true;
        spriteRenderer.color = fullyFadedColor;
        ScreenFader.instance.FadeIn(2f, 1f);
        StartReviveSpin();
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



    private void StartReviveSpin()
    {
        StartCoroutine(ReviveSpinCoroutine());
    }

    private IEnumerator ReviveSpinCoroutine()
    {
        yield return new WaitForSeconds(reviveSpinDelay);
        reviveSpinTimer = reviveSpinDuration;
        isReviveSpinning = true;
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



    private void StartDeathSpin()
    {
        //transform.rotation = new Quaternion(0f, 0f, deathSpinInitialRotation, transform.rotation.w);
        transform.eulerAngles = new Vector3(0f, 0f, deathSpinInitialRotation);
        isDeathSpinning = true;
    }
}
