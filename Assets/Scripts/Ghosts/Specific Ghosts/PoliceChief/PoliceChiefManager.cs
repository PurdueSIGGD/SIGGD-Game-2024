using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UIElements;

public class PoliceChiefManager : GhostManager, ISelectable
{
    //private Animator animator;

    //[SerializeField] public StatManager.Stat[] statList;
    //[SerializeField] public RuntimeAnimatorController defaultController;
    //[SerializeField] public RuntimeAnimatorController policeChiefController;

    //private StatManager stats;
    private PoliceChiefSpecial special;



    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    // ISelectable interface in use
    public override void Select(GameObject player)
    {
        Debug.Log("NORTH SELECTED!");
        special = PlayerID.instance.AddComponent<PoliceChiefSpecial>();
        special.manager = this;
        base.Select(player);
    }

    public override void DeSelect(GameObject player)
    {
        if (PlayerID.instance.GetComponent<PoliceChiefSpecial>()) Destroy(PlayerID.instance.GetComponent<PoliceChiefSpecial>());
        base.DeSelect(player);
    }

}
