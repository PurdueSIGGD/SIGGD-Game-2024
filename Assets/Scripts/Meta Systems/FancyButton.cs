using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FancyButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static readonly float FADE_DURATION = 0.5f;
    public static readonly float DIAMOND_RESCALE = 1.25f;

    [SerializeField] TextMeshProUGUI textUI;
    [SerializeField] Image diamondUI;
    [SerializeField] Image baseUI;

    [SerializeField] Color textHoverColor;
    [SerializeField] Color diamondHoverColor;
    [SerializeField] Color baseHoverColor;

    // Private Fields
    private Color textNormColor;
    private Color diamondNormColor;
    private Color baseNormColor;
    private float t;
    private bool hovered;

    void Start()
    {
        textNormColor = textUI.color;
        diamondNormColor = diamondUI.color;
        baseNormColor = baseUI.color;
        t = 0;
        hovered = false;
    }

    void Update()
    {
        textUI.color = Color.Lerp(textNormColor, textHoverColor, t);
        diamondUI.color = Color.Lerp(diamondNormColor, diamondHoverColor, t);
        baseUI.color = Color.Lerp(baseNormColor, baseHoverColor, t);
        diamondUI.gameObject.transform.localScale = Vector3.Lerp(Vector3.one, DIAMOND_RESCALE * Vector3.one, t);

        float val = (1f / FADE_DURATION) * Time.deltaTime;

        t += (hovered) ? val : -1 * val;
        t = Mathf.Clamp(t, 0, 1);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hovered = false;
    }
}
