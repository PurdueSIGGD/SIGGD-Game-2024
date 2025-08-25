using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastTracerHandler : MonoBehaviour
{
    [SerializeField] private float travelSpeed;
    [SerializeField] private float fadeOutDelayTime;
    [SerializeField] private float fadeOutDurationTime;
    private float timer;
    private Vector2 startPos;
    private Vector2 endPos;
    private float travelTime;

    private LineRenderer lineRenderer;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            lineRenderer.SetPosition(1, Vector2.Lerp(startPos, endPos, (travelTime - timer) / travelTime));
        }
    }

    public void playTracer(Vector2 startPosition, Vector2 endPosition, float travelSpeed, Color startColor, Color endColor)
    {
        this.travelSpeed = travelSpeed;
        lineRenderer.enabled = true;
        lineRenderer.startColor = startColor;
        lineRenderer.endColor = endColor;
        extendTracer(startPosition, endPosition);
        StartCoroutine(fadeOutTracer());
    }

    public void playTracer(Vector2 startPosition, Vector2 endPosition, Color startColor, Color endColor)
    {
        playTracer(startPosition, endPosition, travelSpeed, startColor, endColor);
    }

    public void playTracer(Vector2 startPosition, Vector2 endPosition, bool travelsInstantly, Color startColor, Color endColor)
    {
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, startPosition);
        lineRenderer.SetPosition(1, endPosition);
        lineRenderer.startColor = startColor;
        lineRenderer.endColor = endColor;
        StartCoroutine(fadeOutTracer());
    }

    private void extendTracer(Vector2 startPosition, Vector2 endPosition)
    {
        float distance = Vector2.Distance(startPosition, endPosition);
        travelTime = distance / travelSpeed;
        lineRenderer.SetPosition(0, startPosition);
        lineRenderer.SetPosition(1, startPosition);
        startPos = startPosition;
        endPos = endPosition;
        timer = travelTime;
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
