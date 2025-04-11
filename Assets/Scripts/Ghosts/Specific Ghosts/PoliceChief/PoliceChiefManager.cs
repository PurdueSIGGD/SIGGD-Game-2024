using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PoliceChiefManager : GhostManager, ISelectable
{
    [SerializeField] public DamageContext specialDamage;
    [SerializeField] public GameObject specialRailgunTracer;

    [HideInInspector] public PoliceChiefBasic basic;
    [HideInInspector] public PoliceChiefSpecial special;

    protected override void Start()
    {
        base.Start();
        specialDamage.damage = stats.ComputeValue("Special Damage");
    }

    protected override void Update()
    {
        base.Update();
    }

    // ISelectable interface in use
    public override void Select(GameObject player)
    {
        Debug.Log("NORTH SELECTED!");
        basic = PlayerID.instance.AddComponent<PoliceChiefBasic>();
        basic.SetVars(stats, GetComponent<LineRenderer>());
        //manager
        special = PlayerID.instance.AddComponent<PoliceChiefSpecial>();
        special.manager = this;
        Destroy(PlayerID.instance.GetComponent<LightAttack>());
		base.Select(player);
    }

    public override void DeSelect(GameObject player)
    {
        if (basic) Destroy(basic);
        if (special) special.endSpecial(false);
        if (special) Destroy(special);
		if (!PlayerID.instance.GetComponent<LightAttack>()) PlayerID.instance.AddComponent<LightAttack>();
		base.DeSelect(player);
    }

}
