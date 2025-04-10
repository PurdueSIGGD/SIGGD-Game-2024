using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelectedGhostUIManager : PlayerGhostUIManager
{
    public static PlayerSelectedGhostUIManager instance;

    [SerializeField] private Image healthBarFrame;

    private void Awake()
    {
        instance = this;
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
    public void setHealthBarFrameColor(Color color)
    {
        setImageColor(healthBarFrame, color, true);
    }
}
