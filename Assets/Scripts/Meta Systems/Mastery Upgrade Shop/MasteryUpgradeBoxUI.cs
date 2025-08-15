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
        upgradeButton.onClick.AddListener(TryUpgradeLevel);

        spiritTracker = PersistentData.Instance.GetComponent<SpiritTracker>();
        currentLevel = GetPowerLevel();

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

    private void UpdateUI()
    {
        if (currentLevel < MasteryUpgradeShopUI.MAX_POWER_LEVEL) {
            upgradePriceText.text = "UPGRADE " + GetCurrentPrice();
            upgradeButton.enabled = true;
        }
        else
        {
            upgradePriceText.text = "MAXED";
            upgradeButton.enabled = false;
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

        // Success
        Debug.Log("Purchased " + spiritType + " " + tier + " " + upgradeType + ": " + currentLevel + "/" + 20);
        UpdateUI();

        currentLevel++;
        SaveManager.data.masteryUpgrades.upgradeLevels[(int) upgradeType]++;

    }

    public int GetStatBoostPercent()
    {
        return currentLevel * statBoostIncrementPercent;
    }

    public int GetCurrentPrice() {
        return upgradeStartPrice + upgradePriceIncrement * currentLevel;
    }
    
}
