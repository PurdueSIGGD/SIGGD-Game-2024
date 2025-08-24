using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MasteryUpgradeBoxUI : MonoBehaviour
{
    public enum UpgradeType { // DO NOT CHANGE ORDER

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
    //[SerializeField] private Sprite masteryUpgradeIcon;

    [Header("UI")]
    [SerializeField] private TMP_Text upgradeLevelText;
    [SerializeField] private TMP_Text upgradePriceText;
    [SerializeField] private TMP_Text upgradeNameText;
    [SerializeField] private TMP_Text upgradeDescriptionText;
    [SerializeField] private Button upgradeButton;
    //[SerializeField] private Image upgradeImageUI;

    private int currentLevel = 0;
    private SpiritTracker spiritTracker;

    private string[] upgradeDescriptions = {
        "Increased move speed.",
        "Increased attack damage.",
        "Increased maximum health.",
        "Increased chance to dodge attacks.",
        "Increased critical hit chance.",
        "Reduced elite enemy attack damage.",
        "Reduced ability cooldown times.",
        "Increased attack stun duration.",
        "Increased healing from all sources."
    };

    public void Start()
    {
        upgradeNameText.text = upgradeType.ToString();

        //upgradeImageUI.sprite = masteryUpgradeIcon;

        spiritTracker = PersistentData.Instance.GetComponent<SpiritTracker>();
        upgradeButton.onClick.AddListener(TryUpgradeLevel);

        if (MasteryUpgradeShopUI.boughtUpgradeEvent == null)
        {
            MasteryUpgradeShopUI.boughtUpgradeEvent = new();
        }

        MasteryUpgradeShopUI.boughtUpgradeEvent.AddListener(UpdateUI);

        UpdateUI();
    }

    /// <summary>
    /// Set upgrade description based on current percent
    /// </summary>
    /// <returns></returns>
    private void SetUpgradeDescription()
    {
        string des = upgradeDescriptions[(int) upgradeType] + "\n";

        if (currentLevel == MasteryUpgradeShopUI.MAX_POWER_LEVEL)
        {
            des += "(You have maxed this upgrade.)";
        } else
        {
            des += "(Next: " + (GetStatBoostPercent() + statBoostIncrementPercent) + "%)";
        }
        upgradeDescriptionText.text = des;
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
    public void UpdateUI()
    {
        currentLevel = GetPowerLevel();

        SetUpgradeDescription();

        if (currentLevel < MasteryUpgradeShopUI.MAX_POWER_LEVEL) {
            upgradePriceText.text = "" + GetCurrentPrice();
            upgradeButton.interactable = spiritTracker.HasEnoughSpirits(true, spiritType, GetCurrentPrice());
            Color priceColor = upgradePriceText.color;
            priceColor.a = (spiritTracker.HasEnoughSpirits(true, spiritType, GetCurrentPrice())) ? 1f : 0.4f;
            upgradePriceText.color = priceColor;
        }
        else
        {
            upgradePriceText.text = "MAX";
            upgradeButton.onClick.RemoveListener(TryUpgradeLevel);
            upgradeButton.interactable = false;
        }

        upgradeLevelText.text = "+" + GetStatBoostPercent() + "%";// currentLevel + "/" + MasteryUpgradeShopUI.MAX_POWER_LEVEL;
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

        MasteryUpgradeShopUI.boughtUpgradeEvent?.Invoke();

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
