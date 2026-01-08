using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NorthUltChargeReset : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Door.OnDoorOpened += ResetNorth;
    }

    private void ResetNorth()
    {
       SaveManager.data.north.specialEnergy = 0;
        Door.OnDoorOpened -= ResetNorth;
    }
}
