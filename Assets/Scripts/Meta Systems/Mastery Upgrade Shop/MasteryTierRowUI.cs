using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MasteryTierRowUI : MonoBehaviour
{
    [SerializeField] private int unlockPrice = 1;

    [Header("UI")]
    [SerializeField] private MasteryUpgradeShopUI masteryShop;
    [SerializeField] private Button unlockButton;
    [SerializeField] private TMP_Text unlockButtonText;
    [SerializeField] private Canvas masteryTierPanel;

    private SpiritTracker spiritTracker;
    private void Start()
    {
        spiritTracker = PersistentData.Instance.GetComponent<SpiritTracker>();
    }


    /// <summary>
    /// Hides the unlock button and shows the upgrades
    /// </summary>
    public void SetUnlocked()
    {
        unlockButton.gameObject.SetActive(false);
        masteryTierPanel.enabled = true;
    }

    /// <summary>
    /// Hides the upgrades and shows the unlock button
    /// </summary>
    public void SetUnlockable()
    {
        unlockButton.gameObject.SetActive(true);
        masteryTierPanel.enabled = false;

        unlockButtonText.text = "UNLOCK " + unlockPrice;
        unlockButton.onClick.AddListener(HandleUnlockButtonClick);
    }

    /// <summary>
    /// Hides both the unlock button and the mastery tier upgrades
    /// </summary>
    public void SetLocked()
    {
        unlockButton.gameObject.SetActive(false);
        masteryTierPanel.enabled = false;
    }

    /// <summary>
    /// Unlock row and update UI if spending pink spirits was a success
    /// </summary>
    private void HandleUnlockButtonClick()
    {
        // Unlock if spending spirits was a success

        if (spiritTracker.SpendSecuredSpirits(Spirit.SpiritType.Pink, unlockPrice)) {
            SaveManager.data.masteryUpgrades.numRowsUnlocked++;
            masteryShop.OpenUI();
            masteryShop.UpdateSpiritCounters();
        }
    }

    
}
