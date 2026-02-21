using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GhostSlotVisualizer : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Image border;
    [SerializeField] private TextMeshProUGUI name;
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Color defaultColor;

    private GhostIdentity ghostIdentity;

    public void Visualize(GhostIdentity ghost)
    {
        ghostIdentity = ghost;
        CharacterSO info = ghost.GetCharacterInfo();
        image.sprite = info.characterIcon;
        image.color = Color.white;
        border.color = info.primaryColor;
        name.text = info.displayName;
    }

    public void ClearVisuals()
    {
        ghostIdentity = null;
        image.sprite = defaultSprite;
        image.color = defaultColor;
        border.color = Color.white;
        name.text = "";
    }
}
