using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MasteryUpgradeShopUI : MonoBehaviour
{
    public static int MAX_POWER_LEVEL = 20;

    [SerializeField] private List<MasteryTierRowUI> rows;
    [SerializeField] private List<SpiritCounterUI> spiritCounters;

    [SerializeField] private List<MasteryUpgradeBoxUI> upgradeBoxes;

    [SerializeField] private Button closeButton;

    // Start is called before the first frame update
    void Start()
    {
        closeButton.onClick.AddListener(CloseUI);
        gameObject.SetActive(false);
    }

    public void CloseUI()
    {
        gameObject.SetActive(false);
        SaveManager.instance.Save();
    }

    public void OpenUI()
    {
        gameObject.SetActive(true);

        int rowsUnlocked = SaveManager.data.masteryUpgrades.numRowsUnlocked;

        // Tier Rows 

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
