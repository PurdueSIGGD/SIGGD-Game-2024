using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MasteryUpgradeBoxUI : MonoBehaviour
{
    public enum UpgradeType {

        // Tier 1
        HASTE,
        LETHALITY,
        FORTITUDE,

        // Tier 2
        DEXTERITY,
        PRECISION,
        TENACITY,

        // Tier 3
        PROFICIENCY,
        BRUTALITY,
        RECOVERY

    };

    [SerializeField] public UpgradeType upgradeType;
    [SerializeField] public Spirit.SpiritType spiritType;
    [SerializeField] public int tier; // 1, 2, or 3
    [SerializeField] private int statBoostIncrementPercent = 1;
    [SerializeField] private int upgradeStartPrice = 1;
    [SerializeField] private int upgradePriceIncrement = 1;

    [Header("UI")]
    [SerializeField] private TMP_Text upgradePercentText;
    [SerializeField] private TMP_Text upgradePriceText;
    [SerializeField] private TMP_Text upgradeNameText;
    [SerializeField] private Button upgradeButton;

    private int currentLevel = 0;
    private SpiritTracker spiritTracker;

    public void Start()
    {
        upgradeNameText.text = upgradeType.ToString();

        spiritTracker = PersistentData.Instance.GetComponent<SpiritTracker>();
        upgradeButton.onClick.AddListener(TryUpgradeLevel);

        UpdateUI();
    }

    /// <summary>
    /// Get power level of an upgrade from Save Manager
    /// </summary>
    /// <param name="upgradeBoxUI"></param>
    private int GetPowerLevel()
    {
        return SaveManager.data.masteryUpgrades.upgradeLevels[(int) upgradeType];
    }

    /// <summary>
    /// Update the UI according to the current level
    /// </summary>
    private void UpdateUI()
    {
        currentLevel = GetPowerLevel();

        if (currentLevel < MasteryUpgradeShopUI.MAX_POWER_LEVEL) {
            upgradePriceText.text = "UPGRADE " + GetCurrentPrice();
        }
        else
        {
            upgradePriceText.text = "MAXED";
            upgradeButton.onClick.RemoveListener(TryUpgradeLevel);
        }

        if (currentLevel == 0)
        {
            upgradePercentText.text = "0%";
        }
        else
        {
            upgradePercentText.text = "+" + GetStatBoostPercent() + "%";
        }
    }

    /// <summary>
    /// Spend spirits to upgrade, update UI and Save Manager if success
    /// </summary>
    public void TryUpgradeLevel()
    {
        // Check if level maxed
        if (currentLevel == MasteryUpgradeShopUI.MAX_POWER_LEVEL)
        {
            return;
        }

        // Try to spend
        if (!spiritTracker.SpendSecuredSpirits(spiritType, GetCurrentPrice()))
        {
            return;
        }

        currentLevel++;
        SaveManager.data.masteryUpgrades.upgradeLevels[(int)upgradeType]++;

        UpdateUI();

        // Success - apply upgrade
        ApplyUpgrade();

    }

    /// <summary>
    /// Apply the upgrade boost - does not do anything yet
    /// </summary>
    private void ApplyUpgrade()
    {
        Debug.Log("Purchased " + spiritType + " " + tier + " " + upgradeType + ": " + currentLevel + "/" + 20);
    }

    /// <summary>
    /// Return the current boost amount as a percent
    /// </summary>
    /// <returns></returns>
    public int GetStatBoostPercent()
    {
        return currentLevel * statBoostIncrementPercent;
    }

    /// <summary>
    /// Return the current price of the upgrade
    /// </summary>
    /// <returns></returns>
    public int GetCurrentPrice() {
        return upgradeStartPrice + upgradePriceIncrement * currentLevel;
    }
    
}
