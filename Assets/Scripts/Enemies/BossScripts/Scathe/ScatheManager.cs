using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScatheManager : EnemyStateManager
{
    void Start()
    {
        base.Start();
        MoveState = new CrowMoveState();
    }
}
