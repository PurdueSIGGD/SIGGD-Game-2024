using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MasteryUpgradeBoxUI : MonoBehaviour
{
    [SerializeField] private string upgradeName = "UPGRADE NAME";
    [SerializeField] public Spirit.SpiritType type;
    [SerializeField] public int tier; // 1, 2, or 3
    [SerializeField] private int statBoostIncrementPercent = 1;
    [SerializeField] private int upgradeStartPrice = 1;
    [SerializeField] private int upgradePriceIncrement = 1;

    [Header("UI")]
    [SerializeField] private TMP_Text upgradePercentText;
    [SerializeField] private TMP_Text upgradePriceText;
    [SerializeField] private TMP_Text upgradeNameText;
    [SerializeField] private Button upgradeButton;

    public void Start()
    {
        upgradeNameText.text = upgradeName;
    }

    public void UpdateUI(int powerLevel)
    {

        upgradePriceText.text = "UPGRADE " + GetCurrentPrice(powerLevel);

        if (powerLevel == 0)
        {
            upgradePercentText.text = "%0";
        }
        else
        {
            upgradePercentText.text = "+" + GetStatBoostPercent(powerLevel) + "%";
        }
    }

    public int GetStatBoostPercent(int powerLevel)
    {
        return powerLevel * statBoostIncrementPercent;
    }

    public int GetCurrentPrice(int powerLevel) {
        return upgradeStartPrice + upgradePriceIncrement * powerLevel;
    }
    
}
