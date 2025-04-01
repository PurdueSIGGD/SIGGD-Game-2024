using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KingManager : GhostManager, ISelectable
{
    [SerializeField] public DamageContext specialDamage;
    [SerializeField] public GameObject specialExplosionVFX;

    private KingSpecial special;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        specialDamage.damage = stats.ComputeValue("Special Damage");
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }



    public override void Select(GameObject player)
    {
        Debug.Log("KING SELECTED");
        special = PlayerID.instance.AddComponent<KingSpecial>();
        special.manager = this;
        base.Select(player);
    }

    public override void DeSelect(GameObject player)
    {
        special?.stopSpecial(true, true);
        if (PlayerID.instance.GetComponent<KingSpecial>()) Destroy(PlayerID.instance.GetComponent<KingSpecial>());
        base.DeSelect(player);
    }
}
