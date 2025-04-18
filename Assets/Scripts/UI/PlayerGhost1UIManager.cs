using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGhost1UIManager : PlayerGhostUIManager
{
    public static PlayerGhost1UIManager instance;

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
}
