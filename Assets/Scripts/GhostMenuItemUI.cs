using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GhostMenuItemUI : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI textComponent;
    [SerializeField] public Image imageComponent;
    [SerializeField] public Image borderComponent;
    [SerializeField] public Button buttonComponent;

    public GhostIdentity identity;
    public PartyManagerUI menu;

    private bool initialized;

    void Update()
    {
        if (!initialized && identity != null && menu != null)
        {
            initialized = true;
            Init();
        }
    }

    void Init()
    {
        textComponent.SetText(identity.name);
        buttonComponent.onClick.AddListener(() =>
        {
            //menu.Select(this);
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
        //menu.Select(this);
    }

    public void Visualize(GhostIdentity ghost)
    {

    }
}
