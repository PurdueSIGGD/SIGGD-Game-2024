using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasteryShopInteract : InRangeInteract
{
    // ==============================
    //       Serialized Fields
    // ==============================

    [SerializeField] MasteryUpgradeShopUI upgradeShop;

    protected override InteractOption[] GetMenuOptions()
    {
        InteractOption opt1 = new InteractOption("Open Shop", upgradeShop.OpenUI);
        InteractOption[] options = { opt1 };
        return options;
    }


}
