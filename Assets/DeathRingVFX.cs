using System.Collections;
using System.Collections.Generic;
using UnityEditor.Playables;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Unity.PlasticSCM.Editor.WebApi;

public class DeathRingVFX : MonoBehaviour
{
    [SerializeField] private Volume postProcessingVolume;
    private ChromaticAberration chromaticAbberation;

    [SerializeField] private GameObject pulseVFX;
    [SerializeField] private Color pulseColor;

    [SerializeField] public float fadeInDelay;
    [SerializeField] public float fadeInDuration;

    [SerializeField] public float reviveSpinDelay;
    [SerializeField] public float reviveSpinCoefficient;
    [SerializeField] public float reviveSpinDegree;
    [SerializeField] public float reviveSpinDuration;

    [SerializeField] public float dropChromaticAbberationDelay;
    [SerializeField] public float dropChromaticAbberationDuration;

    [SerializeField] public float fadeOutDelay;
    [SerializeField] public float fadeOutDuration;

    [SerializeField] public float fullFadeOutDelay;
    [SerializeField] public float fullFadeOutDuration;

    [SerializeField] public float deathSpinInitialRotation;
    [SerializeField] public float deathSpinSpeed;

    [SerializeField] public float raiseChromaticAbberationDelay;
    [SerializeField] public float raiseChromaticAbberationDuration;

    [SerializeField] private Color fullyFadedColor;
    [SerializeField] private float partiallyFadedOpacity;
    [SerializeField] private float partiallyAbberatedIntensity;

    [HideInInspector] public static DeathRingVFX instance;

    private SpriteRenderer spriteRenderer;

    private bool isFadingIn = false;
    private float fadeInRate = 0f;

    private bool isDeathSpinning = false;

    private bool isFadingOut = false;
    private bool isFullFadingOut = false;
    private float fadeOutRate = 0f;

    private bool isReviveSpinning = false;
    private float reviveSpinTimer = 0f;

    private bool isChangingChromaticAbberation = false;
    private bool isRaisingChromaticAbberation = false;
    private float chromaticAbberationRate = 0f;



    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        postProcessingVolume.profile.TryGet(out chromaticAbberation);
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
            if (alpha >= partiallyFadedOpacity)
            {
                isDeathSpinning = false;
                transform.eulerAngles = new Vector3(0f, 0f, 0f);
                isFadingOut = false;
                //spriteRenderer.color = fullyFadedColor;
                spriteRenderer.enabled = true;
                FullFadeOut();

                // VFX
                //CameraShake.instance.Shake(0.15f, 10f, 0f, 10f, new Vector2(Random.Range(-0.5f, 0.5f), 1f));
                PlayerID.instance.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                GameObject explosion = Instantiate(pulseVFX, PlayerID.instance.gameObject.transform.position, Quaternion.identity);
                explosion.GetComponent<RingExplosionHandler>().playRingExplosion(20f, pulseColor);
            }
        }

        if (isFullFadingOut)
        {
            // Update fade out
            float alpha = Mathf.Min(spriteRenderer.color.a + (fadeOutRate * Time.deltaTime), 1f);
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);

            // End fade out
            if (alpha >= 1f)
            {
                //isDeathSpinning = false;
                //transform.eulerAngles = new Vector3(0f, 0f, 0f);
                isFullFadingOut = false;
                //spriteRenderer.color = fullyFadedColor;
                spriteRenderer.enabled = true;

                // VFX
                //CameraShake.instance.Shake(0.15f, 10f, 0f, 10f, new Vector2(Random.Range(-0.5f, 0.5f), 1f));
                //PlayerID.instance.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                //GameObject explosion = Instantiate(pulseVFX, PlayerID.instance.gameObject.transform.position, Quaternion.identity);
                //explosion.GetComponent<RingExplosionHandler>().playRingExplosion(20f, pulseColor);
            }
        }

        if (isReviveSpinning)
        {
            reviveSpinTimer -= Time.deltaTime;
            if (reviveSpinTimer <= 0f && !isFadingIn)
            {
                isReviveSpinning = false;
                FadeIn();

                // VFX
                //CameraShake.instance.Shake(0.15f, 10f, 0f, 10f, new Vector2(Random.Range(-0.5f, 0.5f), 1f));
                PlayerID.instance.gameObject.GetComponent<SpriteRenderer>().enabled = true;
                PlayerID.instance.gameObject.GetComponent<Move>().PlayerGo();
                GameObject explosion = Instantiate(pulseVFX, PlayerID.instance.gameObject.transform.position, Quaternion.identity);
                explosion.GetComponent<RingExplosionHandler>().playRingExplosion(20f, pulseColor);
            }

            float zRotation = reviveSpinCoefficient * Mathf.Pow((reviveSpinDuration - reviveSpinTimer), reviveSpinDegree);
            //Quaternion rotation = new Quaternion(0f, 0f, transform.rotation.z, transform.rotation.w);
            //rotation.z = zRotation;
            //transform.rotation = rotation;
            transform.eulerAngles = new Vector3(0f, 0f, zRotation);
        }

        if (isChangingChromaticAbberation)
        {
            // Update fade out
            float intensity = Mathf.Clamp01(chromaticAbberation.intensity.value + (chromaticAbberationRate * Time.deltaTime));
            chromaticAbberation.intensity.Override(intensity);
            if (intensity <= 0f)
            {
                chromaticAbberation.intensity.Override(0f);
                chromaticAbberation.active = false;
                isChangingChromaticAbberation = false;
                return;
            }
            if (intensity >= partiallyAbberatedIntensity && isRaisingChromaticAbberation)
            {
                //chromaticAbberation.intensity.Override(1f);
                //isChangingChromaticAbberation = false;
                chromaticAbberationRate = 1f / dropChromaticAbberationDuration;
                isRaisingChromaticAbberation = false;
                return;
            }

            if (intensity >= 1f)
            {
                chromaticAbberation.intensity.Override(1f);
                isChangingChromaticAbberation = false;
                return;
            }
        }
    }





    public void PlayDeathAnimation()
    {
        FadeOut();
        StartDeathSpin();
        RaiseChromaticAbberation();
    }



    public void PlayReviveAnimation()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = true;
        spriteRenderer.color = fullyFadedColor;
        ScreenFader.instance.FadeIn(1.5f, 1f);
        StartReviveSpin();
        DropChromaticAbberation();
        PlayerID.instance.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        PlayerID.instance.gameObject.GetComponent<Move>().PlayerStop();
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

        fadeOutRate = partiallyFadedOpacity / fadeOutDuration;
        isFadingOut = true;
    }



    public void FullFadeOut()
    {
        StartCoroutine(FullFadeOutCoroutine(fullFadeOutDelay, fullFadeOutDuration));
    }

    public void FullFadeOut(float fadeOutDelay, float fadeOutDuration)
    {
        StartCoroutine(FullFadeOutCoroutine(fadeOutDelay, fadeOutDuration));
    }

    private IEnumerator FullFadeOutCoroutine(float fadeOutDelay, float fadeOutDuration)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = true;
        //spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0f);
        yield return new WaitForSeconds(fadeOutDelay);

        fadeOutRate = (1f - spriteRenderer.color.a) / fadeOutDuration;
        isFullFadingOut = true;
    }



    private void StartDeathSpin()
    {
        //transform.rotation = new Quaternion(0f, 0f, deathSpinInitialRotation, transform.rotation.w);
        transform.eulerAngles = new Vector3(0f, 0f, deathSpinInitialRotation);
        isDeathSpinning = true;
    }





    private void DropChromaticAbberation()
    {
        StartCoroutine(DropChromaticAbberationCoroutine());
    }

    private IEnumerator DropChromaticAbberationCoroutine()
    {
        chromaticAbberation.active = true;
        chromaticAbberation.intensity.Override(0.99f);
        yield return new WaitForSeconds(dropChromaticAbberationDelay);
        chromaticAbberationRate = -1f / dropChromaticAbberationDuration;
        isChangingChromaticAbberation = true;
    }



    private void RaiseChromaticAbberation()
    {
        StartCoroutine(RaiseChromaticAbberationCoroutine());
    }

    private IEnumerator RaiseChromaticAbberationCoroutine()
    {
        yield return new WaitForSeconds(raiseChromaticAbberationDelay);
        chromaticAbberation.active = true;
        chromaticAbberation.intensity.Override(0.01f);
        chromaticAbberationRate = partiallyAbberatedIntensity / raiseChromaticAbberationDuration;
        isChangingChromaticAbberation = true;
        isRaisingChromaticAbberation = true;
    }
}
