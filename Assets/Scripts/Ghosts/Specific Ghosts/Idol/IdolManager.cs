using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IdolManager : GhostManager, ISelectable
{

    public IdolSpecial special;
    public IdolPassive passive;
    [SerializeField] public GameObject holojumpTracerVFX;
    [SerializeField] public GameObject holojumpPulseVFX;
    [SerializeField] public GameObject tempoPulseVFX;
    [SerializeField] public GameObject idolClone;
    [HideInInspector] public IdolClone activeClone;

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
        if (activeClone != null) special.activeClone = activeClone.gameObject;
        special.cloneAlive = (activeClone != null);
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