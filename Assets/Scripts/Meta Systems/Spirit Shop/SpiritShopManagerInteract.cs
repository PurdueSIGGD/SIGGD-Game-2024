using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritManagerInteract : InRangeInteract
{
    // ==============================
    //       Serialized Fields
    // ==============================

    [SerializeField] SpiritShopManager spiritShopManager;


    // ==============================
    //        Other Variables
    // ==============================


    // ==============================
    //        Unity Functions
    // ==============================


    // ==============================
    //       Private Functions
    // ==============================


    protected override InteractOption[] GetMenuOptions()
    {
        if (spiritShopManager.turnCompleted)
        {
            InteractOption[] options = { };
            return options;
        }
        else
        {
            InteractOption opt1 = new InteractOption("Open Shop", OpenSpiritShopUI);
            InteractOption[] options = { opt1 };
            return options;
        }
    }

    private void OpenSpiritShopUI()
    {
        spiritShopManager.OpenShopUI();
    }

    // ==============================
    //        Other Functions
    // ==============================
}
