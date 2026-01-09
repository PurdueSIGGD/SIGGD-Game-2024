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

    protected float backgroundDefaultAlpha;
    protected float iconFrameDefualtAlpha;
    protected float iconDefaultAlpha;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        backgroundDefaultAlpha = background.color.a;
        iconFrameDefualtAlpha = iconFrame.color.a;
        iconDefaultAlpha = icon.color.a;
    }

    // Update is called once per frame
    protected virtual void Update()
    {

    }



    /// <summary>
    /// Set the color of the ghost widget background.
    /// </summary>
    /// <param name="color">The color for the background. The alpha value is ignored.</param>
    public void setBackgroundColor(Color color, bool preserveAlpha = true)
    {
        setImageColor(background, color, preserveAlpha);
    }

    /// <summary>
    /// Set the color of the ghost icon frame.
    /// </summary>
    /// <param name="color">The color for the frame. The alpha value is ignored.</param>
    public void setIconFrameColor(Color color, bool preserveAlpha = true)
    {
        setImageColor(iconFrame, color, preserveAlpha);
    }

    /// <summary>
    /// Set the ghost character icon.
    /// </summary>
    /// <param name="iconSprite">The ghost character icon.</param>
    public void setIcon(Sprite iconSprite)
    {
        icon.sprite = iconSprite;
    }

    public void setIconColor(Color color, bool preserveAlpha = true)
    {
        setImageColor(icon, color, preserveAlpha);
    }

    public virtual void UpdateUIAlpha(float alpha)
    {
        setBackgroundColor(new Color(background.color.r, background.color.g, background.color.b, 
                           Mathf.Clamp(background.color.a + alpha, 0, backgroundDefaultAlpha)), false);
        setIconFrameColor(new Color(iconFrame.color.r, iconFrame.color.g, iconFrame.color.b,
                           Mathf.Clamp(iconFrame.color.a + alpha, 0, iconFrameDefualtAlpha)), false);
        setIconColor(new Color(icon.color.r, icon.color.g, icon.color.b, 
                           Mathf.Clamp(icon.color.a + alpha, 0, iconDefaultAlpha)), false);
    }



    // UTILITIES

    protected void setImageColor(Image image, Color color, bool preserveAlpha)
    {
        Color newColor = color;
        if (preserveAlpha) newColor.a = image.color.a;
        image.color = newColor;
    }
}
