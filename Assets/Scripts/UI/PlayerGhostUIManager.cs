using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGhostUIManager : MonoBehaviour
{
    [SerializeField] protected Image background;
    [SerializeField] protected Image iconFrame;
    [SerializeField] protected Image icon;
    [SerializeField] public PlayerAbilityUIManager basicAbilityUIManager;
    [SerializeField] public PlayerAbilityUIManager specialAbilityUIManager;
    [SerializeField] public PlayerAbilityUIManager skill1UIManager;
    [SerializeField] public PlayerAbilityUIManager skill2UIManager;

    // Start is called before the first frame update
    protected virtual void Start()
    {

    }

    // Update is called once per frame
    protected virtual void Update()
    {

    }



    /// <summary>
    /// Set the color of the ghost widget background.
    /// </summary>
    /// <param name="color">The color for the background. The alpha value is ignored.</param>
    public void setBackgroundColor(Color color)
    {
        setImageColor(background, color, true);
    }

    /// <summary>
    /// Set the color of the ghost icon frame.
    /// </summary>
    /// <param name="color">The color for the frame. The alpha value is ignored.</param>
    public void setIconFrameColor(Color color)
    {
        setImageColor(iconFrame, color, true);
    }

    /// <summary>
    /// Set the ghost character icon.
    /// </summary>
    /// <param name="iconSprite">The ghost character icon.</param>
    public void setIcon(Sprite iconSprite)
    {
        icon.sprite = iconSprite;
    }



    // UTILITIES

    protected void setImageColor(Image image, Color color, bool preserveAlpha)
    {
        Color newColor = color;
        if (preserveAlpha) newColor.a = image.color.a;
        image.color = newColor;
    }
}
