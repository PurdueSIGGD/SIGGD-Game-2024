using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IdolManager : GhostManager, ISelectable
{

    private IdolSpecial special;
    private IdolPassive passive;
    [SerializeField] public GameObject idolClone;

    protected override void Start()
    {
        base.Start();
        passive = GetComponent<IdolPassive>();
        passive.manager = this;
    }
    // ISelectable interface in use
    public override void Select(GameObject player)
    {
        Debug.Log("EVA SELECTED!");

        special = PlayerID.instance.AddComponent<IdolSpecial>();
        special.manager = this;
        special.idolClone = idolClone;
        passive.ApplyBuffOnSwap();

        base.Select(player);
    }

    public override void DeSelect(GameObject player)
    {
        passive.RemoveBuffOnSwap();

        if (PlayerID.instance.GetComponent<IdolSpecial>()) Destroy(PlayerID.instance.GetComponent<IdolSpecial>());
        base.DeSelect(player);
    }
}