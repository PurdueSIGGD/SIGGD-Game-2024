using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemPool : MonoBehaviour
{

    // ==============================
    //       Serialized Fields
    // ==============================

    [SerializeField]
    public Spirit.SpiritType type;

    [SerializeField]
    public List<ItemSO> itemList; // items current available during run

    [SerializeField] private int rerollStartPrice;
    [SerializeField] private int rerollPriceIncrement;

    // ==============================
    //        Other Variables
    // ==============================

    [HideInInspector]
    public int currentRerollPrice = 0; // Current reroll price

    private List<ItemSO> ownedItems; // When item is bought, cannot be bought for rest of run
 
    private SpiritTracker spiritTracker;

    private void Start()
    {
        currentRerollPrice = rerollStartPrice;
        spiritTracker = PersistentData.Instance.GetComponent<SpiritTracker>(); // reference
        ownedItems = PersistentData.Instance.GetComponent<ItemInventory>().ownedItems; // reference
    }

    /// <summary>
    /// Returns a random item from the pool
    /// </summary>
    /// <returns></returns>
    public ItemSO PickRandomItem()
    {
        int randomIndex = Random.Range(0, itemList.Count);
        Debug.Log(randomIndex);
        return itemList[randomIndex];
    }


    public ItemSO RerollRandomItem()
    {
        // Remove price of reroll, return null if not enough

        if (!spiritTracker.SpendSpirits(type, currentRerollPrice))
        {
            return null;
        }

        // Return random new item and increase price

        currentRerollPrice += rerollPriceIncrement;
        return PickRandomItem();

    }

    /// <summary>
    /// Spend spirits if enough and move item out of pool into items owned list
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool BuyItem(ItemSO item)
    {
        bool success = spiritTracker.SpendSpirits(type, item.price);

        if (success)
        {
            itemList.Remove(item);
            ownedItems.Add(item);
            item.owned = true;
        }

        Debug.Log("Items owned " + ownedItems.Count);

        return success;
    }

    /// <summary>
    /// After run, return all the items to the pool (can be bought again)
    /// </summary>
    public void ReturnItemsToPool()
    {
        foreach (ItemSO item in ownedItems)
        {
            item.owned = false;

            ownedItems.Remove(item);
            itemList.Add(item);
        }
    }

}
