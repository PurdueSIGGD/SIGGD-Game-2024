using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class OrionManager : GhostManager
{
    [HideInInspector] public bool isDashEnabled = true;
    [HideInInspector] public bool isAirbornePostDash = false;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (isAirbornePostDash && animator.GetBool("p_grounded"))
        {
            isAirbornePostDash = false;
        }
        isDashEnabled = (!(getSpecialCooldown() > 0f  || isAirbornePostDash));
    }
}
