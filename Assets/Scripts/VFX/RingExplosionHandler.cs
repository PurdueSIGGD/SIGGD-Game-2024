using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingExplosionHandler : MonoBehaviour
{
    [SerializeField] private float explosionDurationTime;
    [SerializeField] private float fadeOutDelayTime;
    [SerializeField] private float fadeOutDurationTime;

    private float initialScale;
    private float finalScale;
    private float timer;

    // Start is called before the first frame update
    void Awake()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0f)
        {
            timer -= Time.deltaTime;
            float currentScale = Mathf.Lerp(initialScale, finalScale, (explosionDurationTime - timer) / explosionDurationTime);
            transform.localScale = new Vector3(currentScale, currentScale, 1f);
            if (timer <= 0f)
            {
                Destroy(gameObject);
            }
        }
    }

    /// <summary>
    /// Plays the ring explosion animation. This GameObject will remain inactive until this function is called, and will be destroyed shortly after.
    /// </summary>
    /// <param name="explosionRadius">The ring explosion's radius.</param>
    /// <param name="color">The ring explosion's color.</param>
    public void playRingExplosion(float explosionRadius, Color color)
    {
        gameObject.SetActive(true);
        animateRingExplosion(explosionRadius, color);
        StartCoroutine(fadeOutRing());
    }

    private void animateRingExplosion(float explosionRadius, Color color)
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = new Color(color.r, color.g, color.b, spriteRenderer.color.a);
        initialScale = 0.05f;
        finalScale = explosionRadius / 4.5f;
        transform.localScale = new Vector3(initialScale, initialScale, 1f);
        /*
        int step = 20;
        for (int i = 0; i < step; i++)
        {
            float currentScale = Mathf.Lerp(initialScale, finalScale, ((float) i / (float) step));
            transform.localScale = new Vector3(currentScale, currentScale, 1f);
            yield return new WaitForSeconds(explosionDurationTime / (float)step);
        }
        Destroy(gameObject);
        */
        timer = explosionDurationTime;
    }

    private IEnumerator fadeOutRing()
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
            yield return new WaitForSeconds(fadeOutDurationTime / (float)step);
        }
    }
}
