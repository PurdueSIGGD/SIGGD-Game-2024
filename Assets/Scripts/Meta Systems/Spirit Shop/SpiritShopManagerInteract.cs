using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiriShopManagerInteract : InRangeInteract
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
            InteractOption opt1 = new InteractOption("Heal", OpenSpiritShopUI);
            InteractOption[] options = { opt1 };
            return options;
        }
    }

    private void OpenSpiritShopUI()
    {
        spiritShopManager.OpenShopUI();

        Health playerHealth = PlayerID.instance.GetComponent<Health>();
        playerHealth.currentHealth = Mathf.Min((playerHealth.currentHealth + (playerHealth.GetStats().ComputeValue("Max Health") * playerHealth.GetStats().ComputeValue("Mortal Wound Threshold"))),
                                               playerHealth.GetStats().ComputeValue("Max Health"));
        CloseMenu();
    }

}
