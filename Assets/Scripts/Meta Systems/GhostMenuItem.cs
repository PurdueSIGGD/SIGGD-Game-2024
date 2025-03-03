using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GhostMenuItem : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI textComponent;
    [SerializeField] public Image imageComponent;
    [SerializeField] public Button buttonComponent;

    public GhostIdentity identity;
    public PartyEditor menu;

    void Start()
    {
        textComponent.SetText(identity.name);
        buttonComponent.onClick.AddListener(() =>
        {
            menu.Select(this);
        });
    }

    public void SetSelected(bool selected)
    {
        if (selected)
        {
            imageComponent.color = Color.green;
        }
        else
        {
            imageComponent.color = Color.gray;
        }
    }

    public void UISelect()
    {
        menu.Select(this);
    }
}
