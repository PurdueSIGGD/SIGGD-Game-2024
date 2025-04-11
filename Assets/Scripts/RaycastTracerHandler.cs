using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastTracerHandler : MonoBehaviour
{
    [SerializeField] private float fadeOutDelayTime;
    [SerializeField] private float fadeOutDurationTime;

    private LineRenderer lineRenderer;

    void Awake()
    {
        //StartCoroutine(fadeOutTracer());
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playTracerFade(Vector2 startPosition, Vector2 endPosition, Color startColor, Color endColor)
    {
        lineRenderer.enabled = true;
        lineRenderer.startColor = startColor;
        lineRenderer.endColor = endColor;
        lineRenderer.SetPosition(0, startPosition);
        lineRenderer.SetPosition(1, endPosition);
        StartCoroutine(fadeOutTracer());
    }

    private IEnumerator fadeOutTracer()
    {
        yield return new WaitForSeconds(fadeOutDelayTime);
        int step = 20;
        for (int i = 0; i < step; i++)
        {
            Color endColor = lineRenderer.endColor;
            endColor.a -= 1f / (float) step;
            lineRenderer.endColor = endColor;
            Color startColor = lineRenderer.startColor;
            startColor.a -= 1f / (float) step;
            lineRenderer.startColor = startColor;
            yield return new WaitForSeconds(fadeOutDurationTime / (float) step);
        }
        Destroy(gameObject);
    }
}
