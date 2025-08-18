using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverShow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] RectTransform panel;
    [SerializeField] float showDuration;
    [SerializeField] float hoverDelay = 0.15f;

    private float t;
    private bool hovered;
    private IEnumerator coroutine;

    public void OnPointerEnter(PointerEventData eventData)
    {
        coroutine = HoverDelay();
        StartCoroutine(coroutine);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hovered = false;
        StopCoroutine(coroutine);
        coroutine = null;
    }

    void Start()
    {
        t = 0;
    }

    void Update()
    {
        panel.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t);

        float val = (1f / showDuration) * Time.deltaTime;

        t += (hovered) ? val : -1 * val;
        t = Mathf.Clamp(t, 0, 1);
    }

    IEnumerator HoverDelay()
    {
        yield return new WaitForSeconds(hoverDelay);
        hovered = true;
    }
}
