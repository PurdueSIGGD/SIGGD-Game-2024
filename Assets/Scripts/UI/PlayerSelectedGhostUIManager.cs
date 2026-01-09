using UnityEngine;
using UnityEngine.UI;

public class PlayerSelectedGhostUIManager : PlayerGhostUIManager
{
    public static PlayerSelectedGhostUIManager instance;

    [SerializeField] private Image healthBarFrame;

    [SerializeField] private Image BackgroundFrame2;

    private float healthBarFrameDeafultAlpha;
    private float backgroundFrame2DeafultAlpha;

    private void Awake()
    {
        instance = this;
        healthBarFrameDeafultAlpha = healthBarFrame.color.a;
        backgroundFrame2DeafultAlpha = BackgroundFrame2.color.a;
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    /// <summary>
    /// Set the color of the health bar frame.
    /// </summary>
    /// <param name="color">The color for the frame. The alpha value is ignored.</param>
    public void setHealthBarFrameColor(Color color, bool preserveAlpha = true)
    {
        setImageColor(healthBarFrame, color, preserveAlpha);
    }


    public void setBackground2Color(Color color, bool preserveAlpha = true)
    {
        setImageColor(BackgroundFrame2, color, preserveAlpha);
    }

    public override void UpdateUIAlpha(float alpha)
    {
        base.UpdateUIAlpha(alpha);
        setHealthBarFrameColor(new Color(healthBarFrame.color.r, healthBarFrame.color.g, healthBarFrame.color.b,
                                         Mathf.Clamp(healthBarFrame.color.a + alpha, 0, healthBarFrameDeafultAlpha)), false);
        setBackground2Color(new Color(BackgroundFrame2.color.r, BackgroundFrame2.color.g, BackgroundFrame2.color.b,
                                 Mathf.Clamp(BackgroundFrame2.color.a + alpha, 0, backgroundFrame2DeafultAlpha)), false);
    }
}
