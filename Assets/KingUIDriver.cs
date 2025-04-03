using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingUIDriver : GhostUIDriver
{
    private KingManager manager;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        manager = GetComponent<KingManager>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (!ghostIdentity.IsSelected()) return;
        updateMeterValue();
        updateMeterColors();
        updateMeterActive();
    }

    private void updateMeterValue()
    {
        float value = manager.currentShieldHealth;
        float maxValue = stats.ComputeValue("Shield Max Health");
        meterUIManager.updateMeterValue(value, maxValue);
    }

    private void updateMeterColors()
    {
        Color color = ghostIdentity.GetCharacterInfo().primaryColor;
        meterUIManager.updateBackgroundColor(color);
        meterUIManager.updateMeterColor(color);
        if (manager.getBasicCooldown() > 0f) meterUIManager.resetMeterColor();
    }

    public void updateMeterActive()
    {
        if (manager.basic == null) return;
        if (manager.basic.isShielding || manager.currentShieldHealth < stats.ComputeValue("Shield Max Health"))
        {
            meterUIManager.activateWidget(0f);
            return;
        }
        meterUIManager.deactivateWidget(0.3f);
    }

}
