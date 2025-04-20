using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverMove : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] RectTransform abilityPanel;
    [SerializeField] Vector3 hidePoint;
    [SerializeField] float moveDuration;
    [SerializeField] float hoverDelay;

    private float t;
    private bool hovered;
    private Vector3 showPoint;
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
        showPoint = abilityPanel.anchoredPosition;
        abilityPanel.anchoredPosition = hidePoint;
        t = 1;
    }

    void Update()
    {
        abilityPanel.anchoredPosition = Vector3.Lerp(hidePoint, showPoint, t);

        float val = (1f / moveDuration) * Time.deltaTime;

        t += (hovered) ? val : -1 * val;
        t = Mathf.Clamp(t, 0, 1);
    }

    IEnumerator HoverDelay()
    {
        yield return new WaitForSeconds(hoverDelay);
        hovered = true;
    }
}
