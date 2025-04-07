using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SilasManager : GhostManager
{
    public GameObject blightPotion;
    public float initialSpeed;

    private PlagueDoctorSpecial specialRef;

    public override void Select(GameObject player)
    {
        Debug.Log("Plague doctor selected");
        base.Select(player);
        specialRef = PlayerID.instance.AddComponent<PlagueDoctorSpecial>();
        specialRef.manager = this;
    }

    public override void DeSelect(GameObject player)
    {
        if (specialRef) Destroy(specialRef);
    }
}
