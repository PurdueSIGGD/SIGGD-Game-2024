using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KingManager : GhostManager, ISelectable
{
    [SerializeField] public DamageContext specialDamage;
    [SerializeField] public GameObject shieldCircleVFX;
    [SerializeField] public GameObject shieldExplosionVFX;
    [SerializeField] public GameObject specialExplosionVFX;

    public float currentShieldHealth;

    private KingBasic basic;
    private KingSpecial special;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        specialDamage.damage = stats.ComputeValue("Special Damage");
        currentShieldHealth = stats.ComputeValue("Shield Max Health");
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }



    public override void Select(GameObject player)
    {
        Debug.Log("KING SELECTED");
        basic = PlayerID.instance.AddComponent<KingBasic>();
        basic.manager = this;
        special = PlayerID.instance.AddComponent<KingSpecial>();
        special.manager = this;
        base.Select(player);
    }

    public override void DeSelect(GameObject player)
    {
        if (basic) Destroy(basic);
        special?.stopSpecial(true, true);
        if (special) Destroy(special);
        base.DeSelect(player);
    }
}
