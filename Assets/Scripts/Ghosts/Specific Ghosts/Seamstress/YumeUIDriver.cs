using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YumeUIDriver : GhostUIDriver
{
    private SeamstressManager manager;

    protected override void Start()
    {
        base.Start();
        manager = GetComponent<SeamstressManager>();
    }

    protected override void Update()
    {
        base.Update();
        if (!isInParty) return;
        updateSpecialAbility();
    }

    private void updateSpecialAbility()
    {
        specialAbilityUIManager.setAbilityCooldownTime(manager.getSpecialCooldown(), stats.ComputeValue("Special Cooldown"));
    }
}
