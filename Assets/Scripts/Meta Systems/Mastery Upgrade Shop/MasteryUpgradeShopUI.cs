using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasteryUpgradeShopUI : MonoBehaviour
{
    [SerializeField] private List<MasteryTierRowUI> rows;
    [SerializeField] private List<SpiritCounterUI> spiritCounters;

    // Start is called before the first frame update
    void Start()
    {
        SetupUI(); // TODO - shop open
    }

    public void SetupUI()
    {
        int rowsUnlocked = SaveManager.data.masteryUpgrades.numRowsUnlocked;

        for (int i = 0; i < rows.Count; i++)
        {
            MasteryTierRowUI row = rows[i];

            if (i < rowsUnlocked)
            {
                // Row unlocked
                row.SetUnlocked();
            }
            else if (i == rowsUnlocked)
            {
                // Row is unlockable
                row.SetUnlockable();
            }
            else
            {
                // Row is locked
                row.SetLocked();
            }
        }
    }

    public void UpdateSpiritCounters()
    {
        foreach (SpiritCounterUI ui in spiritCounters)
        {
            ui.UpdateText();
        }
    }

}
