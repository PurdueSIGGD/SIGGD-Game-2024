using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SamuraiManager : GhostManager, ISelectable
{
    [SerializeField] public DamageContext heavyDamageContext;

    [HideInInspector] public bool selected;
    [HideInInspector] public WrathHeavyAttack basic; // the heavy attack ability
    [HideInInspector] public SamuraiRetribution special; // the special ability

    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public override void Select(GameObject player)
    {
        Debug.Log("Akihito SELECTED!");
        selected = true;
        if (PlayerID.instance.GetComponent<HeavyAttack>()) Destroy(PlayerID.instance.GetComponent<HeavyAttack>());
        basic = PlayerID.instance.AddComponent<WrathHeavyAttack>();
        basic.manager = this;

        special = PlayerID.instance.AddComponent<SamuraiRetribution>();

        base.Select(player);
    }

    public override void DeSelect(GameObject player)
    {
        selected = false;
        if (basic) Destroy(basic);
        if (!PlayerID.instance.GetComponent<HeavyAttack>()) PlayerID.instance.AddComponent<HeavyAttack>();

        if (special) Destroy(special);

        base.DeSelect(player);
    }
}
