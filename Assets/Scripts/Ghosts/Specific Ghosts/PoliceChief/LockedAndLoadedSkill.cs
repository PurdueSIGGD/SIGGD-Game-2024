using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class LockedAndLoadedSkill : Skill
{
    int[] reserveCharges = {0, 3, 6, 9, 12};
    private int pointIndex;
    private int reservedCount;
    private Camera cam;
    private Animator camAnim;

    PoliceChiefSpecial policeChiefSpecial;

    public override void AddPointTrigger()
    {
        pointIndex = GetPoints();
    }

    public override void ClearPointsTrigger()
    {
    }

    public override void RemovePointTrigger()
    {
        pointIndex = GetPoints();
    }

    // Start is called before the first frame update
    void Start()
    {
        AddPoint();
        reservedCount = reserveCharges[pointIndex];
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        camAnim = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (policeChiefSpecial == null)
        {
            policeChiefSpecial = PlayerID.instance.GetComponent<PoliceChiefSpecial>();
            return;
        }
        if (reservedCount>0)
        {
            policeChiefSpecial.hasReserves = true;
        }
    }
}
