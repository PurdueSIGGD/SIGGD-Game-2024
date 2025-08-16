using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IrisController : BossController
{
    [SerializeField] bool startSpawn;

    public new void Start()
    {
        base.Start();
    }
    public new void Update()
    {
        base.Update();
        if (startSpawn)
        {
            StartSpawning();
            startSpawn = false;
        }
    }
}
