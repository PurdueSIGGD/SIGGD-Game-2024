using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PoliceChiefManager : GhostManager, ISelectable
{
    [SerializeField] public DamageContext specialDamage;
    [SerializeField] public GameObject specialRailgunTracer;



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
        PlayerID.instance.AddComponent<PoliceChiefSpecial>().manager = this;
        PlayerID.instance.AddComponent<PoliceChiefBasic>().SetVars(stats, GetComponent<LineRenderer>());
        Destroy(PlayerID.instance.GetComponent<LightAttack>());
		base.Select(player);
    }

    public override void DeSelect(GameObject player)
    {
        if (PlayerID.instance.GetComponent<PoliceChiefSpecial>()) Destroy(PlayerID.instance.GetComponent<PoliceChiefSpecial>());
        if (PlayerID.instance.GetComponent<PoliceChiefBasic>()) Destroy(PlayerID.instance.GetComponent<PoliceChiefBasic>());
		if (!PlayerID.instance.GetComponent<LightAttack>()) PlayerID.instance.AddComponent<LightAttack>();
		base.DeSelect(player);
    }

}
