using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class PoliceChiefManager : GhostManager, ISelectable
{
    [SerializeField] public DamageContext basicDamage;
    [SerializeField] public DamageContext specialDamage;
    [SerializeField] public GameObject basicShot;
    [SerializeField] public GameObject basicTracerVFX;
    [SerializeField] public GameObject basicImpactExplosionVFX;
    [SerializeField] public GameObject specialShot;
    [SerializeField] public GameObject specialTracerVFX;
    [SerializeField] public GameObject specialImpactExplosionVFX;
    [SerializeField] public ActionContext sidearmActionContext;
    [SerializeField] public ActionContext policeChiefRailgun;

    [HideInInspector] public PoliceChiefBasic basic;
    [HideInInspector] public PoliceChiefSpecial special;

    protected override void Start()
    {
        base.Start();
        basicDamage.damage = stats.ComputeValue("Basic Damage");
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

        if (PlayerID.instance.GetComponent<HeavyAttack>()) Destroy(PlayerID.instance.GetComponent<HeavyAttack>());
        basic = PlayerID.instance.AddComponent<PoliceChiefBasic>();
        basic.manager = this;

        special = PlayerID.instance.AddComponent<PoliceChiefSpecial>();
        special.manager = this;

		base.Select(player);
    }

    public override void DeSelect(GameObject player)
    {
        if (basic) Destroy(basic);
        if (!PlayerID.instance.GetComponent<HeavyAttack>()) PlayerID.instance.AddComponent<HeavyAttack>();

        if (special) special.endSpecial(false);
        if (special) Destroy(special);

		base.DeSelect(player);
    }

}
