using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IdolManager : GhostManager, ISelectable
{

    private IdolSpecial special;

    [SerializeField] public GameObject idolClone;

    protected override void Start()
    {
        base.Start();
    }
    // ISelectable interface in use
    public override void Select(GameObject player)
    {
        Debug.Log("EVA SELECTED!");
        PlayerID.instance.AddComponent<IdolPassive>();
        special = PlayerID.instance.AddComponent<IdolSpecial>();
        special.manager = this;
        special.idolClone = idolClone;
    }

    public override void DeSelect(GameObject player)
    {
        if (PlayerID.instance.GetComponent<IdolPassive>()) Destroy(PlayerID.instance.GetComponent<IdolPassive>());
        if (PlayerID.instance.GetComponent<IdolSpecial>()) Destroy(PlayerID.instance.GetComponent<IdolSpecial>());
    }
}