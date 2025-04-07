using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastTracerHandler : MonoBehaviour
{

    [SerializeField] private float fadeOutDelayTime;
    [SerializeField] private float fadeOutDurationTime;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(fadeOutTracer());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator fadeOutTracer()
    {
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        yield return new WaitForSeconds(fadeOutDelayTime);
        int step = 20;
        for (int i = 0; i < step; i++)
        {
            Color endColor = lineRenderer.endColor;
            endColor.a -= 1f / (float)step;
            lineRenderer.endColor = endColor;
            Color startColor = lineRenderer.startColor;
            startColor.a -= 1f / (float)step;
            lineRenderer.startColor = startColor;
            yield return new WaitForSeconds(fadeOutDurationTime / (float)step);
        }
        Destroy(lineRenderer.gameObject);
    }
}
