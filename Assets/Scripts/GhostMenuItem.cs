using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GhostMenuItem : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI textComponent;
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

    }
}
