using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGhost2UIManager : PlayerGhostUIManager
{
    public static PlayerGhost2UIManager instance;

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
