using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilasUIDriver : GhostUIDriver
{
    private SilasManager manager;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        manager = GetComponent<SilasManager>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (!isInParty) return;
        updateBasicAbility();
        updateSpecialAbility();
        updateSkill1();
        if (ghostIdentity.IsSelected()) updateMeter();

    }

    private void updateBasicAbility()
    {

    }

    private void updateSpecialAbility()
    {
        specialAbilityUIManager.setAbilityCooldownTime(manager.getSpecialCooldown(), stats.ComputeValue("Special Cooldown"));
    }

    private void updateSkill1()
    {

    }

    private void updateMeter()
    {

    }
}