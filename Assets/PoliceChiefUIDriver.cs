using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceChiefUIDriver : GhostUIDriver
{
    private PoliceChiefManager manager;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        manager = GetComponent<PoliceChiefManager>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (!isInParty) return;
        updateBasicAbility();
        updateSpecialAbility();
    }

    private void updateBasicAbility()
    {
        setDefaultAbilityUI(basicAbilityUIManager, true);
    }

    private void updateSpecialAbility()
    {
        setDefaultAbilityUI(specialAbilityUIManager, true);
        specialAbilityUIManager.updateAbilityCooldownTime(manager.getSpecialCooldown(), stats.ComputeValue("Special Cooldown"));
    }
}
