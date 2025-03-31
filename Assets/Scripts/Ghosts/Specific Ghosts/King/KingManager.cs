using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KingManager : GhostManager, ISelectable
{
    [SerializeField] public DamageContext specialDamage;
    [SerializeField] public GameObject specialExplosionVFX;

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
        PlayerID.instance.AddComponent<KingSpecial>().manager = this;
        base.Select(player);
    }

    public override void DeSelect(GameObject player)
    {
        if (PlayerID.instance.GetComponent<KingSpecial>()) Destroy(PlayerID.instance.GetComponent<KingSpecial>());
        base.DeSelect(player);
    }
}
