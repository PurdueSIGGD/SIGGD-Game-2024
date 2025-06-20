using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YumeUIDriver : GhostUIDriver
{
    private SeamstressManager manager;

    protected override void Start()
    {
        base.Start();
        manager = GetComponent<SeamstressManager>();
    }
}
