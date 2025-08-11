using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SilasManager : GhostManager
{
    public GameObject blightPotion;
    public float initialSpeed;

    [HideInInspector] public PlagueDoctorSpecial special;

    public override void Select(GameObject player)
    {
        Debug.Log("SILAS SELECTED!");

        special = PlayerID.instance.AddComponent<PlagueDoctorSpecial>();
        special.manager = this;

        base.Select(player);
    }

    public override void DeSelect(GameObject player)
    {
        if (special) Destroy(special);

        base.DeSelect(player);
    }
}
