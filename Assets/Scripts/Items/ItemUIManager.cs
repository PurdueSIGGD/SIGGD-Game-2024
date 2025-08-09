using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemUIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text priceText;
    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private TMP_Text itemDescription;

    [SerializeField] private Image itemIcon;

    private ItemPool pool;

    private ItemSO item;

    private void Start()
    {
        pool = gameObject.GetComponent<ItemPool>();
    }

    /// <summary>
    /// Update the appearance of the item box with item's information
    /// </summary>
    private void UpdateItemBoxUI()
    {
        priceText.text = "Buy for " + item.price.ToString();
        itemNameText.text = item.displayName;
        itemDescription.text = item.itemDescription;

        itemIcon.sprite = item.itemIcon;

        switch (item.itemType)
        {
            case ItemSO.ItemType.RED:
                priceText.color = Color.red;
                break;
            case ItemSO.ItemType.BLUE:
                priceText.color = Color.blue;
                break;
            case ItemSO.ItemType.YELLOW:
                priceText.color = Color.yellow;
                break;
        }
    }

    /// <summary>
    /// Picks a random available item from the pool and displays it in the UI
    /// </summary>
    public void DisplayRandomItem()
    {
        item = pool.PickRandomItem();
        UpdateItemBoxUI();
    }

}
