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
    

    private void Start()
    {
        pool = gameObject.GetComponent<ItemPool>();
        rerollButton.onClick.AddListener(RerollButtonOnClick);
        buyButton.onClick.AddListener(BuyButtonOnClick);
    }

    /// <summary>
    /// Update the appearance of the item box with item's information
    /// </summary>
    private void UpdateItemBoxUI()
    {
        priceText.text = "BUY FOR " + item.price.ToString();
        itemNameText.text = item.displayName;
        itemDescription.text = item.itemDescription;

        itemIcon.sprite = item.itemIcon;

        UpdateRerollButtonText();

        // TODO: temporary text color, add icons later
        switch (item.itemType)
        {
            case Spirit.SpiritType.Red:
                itemNameText.color = Color.red;
                break;
            case Spirit.SpiritType.Yellow:
                itemNameText.color = Color.yellow;
                break;
            case Spirit.SpiritType.Blue:
                itemNameText.color = Color.blue;
                break;
        }
    }

    /// <summary>
    /// Update the text for the reroll button
    /// </summary>
    private void UpdateRerollButtonText()
    {
        // Update text
        rerollButtonText.text = "REROLL FOR " + pool.currentRerollPrice;
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
