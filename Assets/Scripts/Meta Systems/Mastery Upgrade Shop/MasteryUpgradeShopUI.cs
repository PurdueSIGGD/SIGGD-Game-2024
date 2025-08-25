using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MasteryUpgradeShopUI : MonoBehaviour
{
    public static int MAX_POWER_LEVEL = 20;
    public static UnityEvent boughtUpgradeEvent;

    [SerializeField] private List<MasteryTierRowUI> rows;
    [SerializeField] private Button closeButton;

    // Start is called before the first frame update
    void Start()
    {
        if (boughtUpgradeEvent == null)
        {
            boughtUpgradeEvent = new();
        }

        closeButton.onClick.AddListener(CloseUI);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Hide the shop UI
    /// </summary>
    public void CloseUI()
    {
        gameObject.SetActive(false);
        SaveManager.instance.Save();
    }

    /// <summary>
    /// Open the UI, using SaveManager to figure out which rows to unlock
    /// </summary>
    public void OpenUI()
    {
        SpiritTracker spiritTracker = PersistentData.Instance.GetComponent<SpiritTracker>();
        gameObject.SetActive(true);

        int rowsUnlocked = SaveManager.data.masteryUpgrades.numRowsUnlocked;

        // Tier Rows 

        for (int i = 0; i < rows.Count; i++)
        {
            MasteryTierRowUI row = rows[i];

            row.unlockButton.interactable = spiritTracker.HasEnoughSpirits(true, Spirit.SpiritType.Pink, row.unlockPrice);

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

}
