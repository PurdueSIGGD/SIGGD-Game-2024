using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemUIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text priceText;
    [SerializeField] private Button buyButton;

    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private TMP_Text itemDescription;

    [SerializeField] private Button rerollButton;
    [SerializeField] private TMP_Text rerollButtonText;

    [SerializeField] private Image itemIcon;

    [SerializeField] private SpiritShopManager shopManager;

    private ItemPool pool;
    private ItemSO item; // The currently displayed item
    private SpiritTracker spiritTracker; // reference
    

    private void OnEnable()
    {
        pool = gameObject.GetComponent<ItemPool>();
        rerollButton.onClick.AddListener(RerollButtonOnClick);
        buyButton.onClick.AddListener(BuyButtonOnClick);
        spiritTracker = PersistentData.Instance.GetComponent<SpiritTracker>();
    }

    /// <summary>
    /// Update the appearance of the item box with item's information
    /// </summary>
    private void UpdateItemBoxUI()
    {
        priceText.text = item.price.ToString();
        itemNameText.text = item.displayName.ToUpper();
        itemDescription.text = item.itemDescription;

        // Buttons disabled or enabled
        buyButton.interactable = spiritTracker.HasEnoughSpirits(false, pool.type, item.price);
        rerollButton.interactable = spiritTracker.HasEnoughSpirits(false, pool.type, pool.currentRerollPrice);

        //itemIcon.sprite = item.itemIcon;

        UpdateRerollButtonText();

    }

    /// <summary>
    /// Update the text for the reroll button
    /// </summary>
    private void UpdateRerollButtonText()
    {
        // Update text
        rerollButtonText.text = "REROLL " + pool.currentRerollPrice;
    }

    /// <summary>
    /// Picks a random available item from the pool and displays it in the UI
    /// </summary>
    public void PickAndDisplayRandomItem()
    {
        item = pool.PickRandomItem();
        UpdateItemBoxUI();
    }

    /// <summary>
    /// Call this when the reroll button is clicked
    /// </summary>
    public void RerollButtonOnClick()
    {
        ItemSO newItem = pool.RerollRandomItem();
        if (newItem != null)
        {
            item = newItem;
            UpdateItemBoxUI();

            // Update text
            UpdateRerollButtonText();
            shopManager.UpdateSpiritCountText();
        } else
        {
            // not enough spirits
            return; // TODO
        }
    }

    /// <summary>
    /// Called when the buy button is clicked
    /// </summary>
    public void BuyButtonOnClick()
    {
        bool success = pool.BuyItem(item); // try to buy the current item

        if (success)
        {
            Debug.Log("item bought!");
            shopManager.CloseShopUI();
        }
    }

}
