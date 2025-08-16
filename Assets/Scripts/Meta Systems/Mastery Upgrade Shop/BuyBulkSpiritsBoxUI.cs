using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyBulkSpiritsBoxUI : MonoBehaviour
{
    [SerializeField] public Spirit.SpiritType spiritType;
    [SerializeField] private static int bulkAmount = 1000; // how much to buy
    [SerializeField] private static int bulkPrice = 50; // Price in Pink spirits

    [Header("UI")]
    [SerializeField] private TMP_Text bulkAmountText;
    [SerializeField] private Button buySpiritsButton;
    [SerializeField] private TMP_Text buySpiritsButtonText;

    private SpiritTracker spiritTracker;

    // Start is called before the first frame update
    void Start()
    {
        bulkAmountText.text = "+" + bulkAmount;
        spiritTracker = PersistentData.Instance.GetComponent<SpiritTracker>();
        buySpiritsButton.onClick.AddListener(TryBuySpirits);
        buySpiritsButtonText.text = "BUY " + bulkPrice;
    }
    public void TryBuySpirits()
    {
        bool success = spiritTracker.SpendSecuredSpirits(Spirit.SpiritType.Pink, bulkPrice);
        if (success) {
            SaveManager.data.spiritCounts[(int) spiritType] += bulkAmount;
        }
    }
}
