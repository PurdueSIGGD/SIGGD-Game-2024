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

    [SerializeField] private int rerollStartPrice;
    [SerializeField] private int rerollPriceIncrement;

    // ==============================
    //        Other Variables
    // ==============================

    [HideInInspector]
    public List<ItemSO> itemList; // items current available during run, reference
    [HideInInspector]
    public int currentRerollPrice = 0; // Current reroll price

    private List<ItemSO> ownedItems; // When item is bought, cannot be bought for rest of run
 
    private SpiritTracker spiritTracker;

    private void Start()
    {
        currentRerollPrice = rerollStartPrice;
        spiritTracker = PersistentData.Instance.GetComponent<SpiritTracker>(); // reference
        ownedItems = PersistentData.Instance.GetComponent<ItemInventory>().ownedItems; // reference

        switch (type)
        {
            case Spirit.SpiritType.Red:
                itemList = PersistentData.Instance.GetComponent<ItemInventory>().redItemPool;
                break;
            case Spirit.SpiritType.Blue:
                itemList = PersistentData.Instance.GetComponent<ItemInventory>().blueItemPool;
                break;
            case Spirit.SpiritType.Yellow:
                itemList = PersistentData.Instance.GetComponent<ItemInventory>().yellowItemPool;
                break;
        }
    }

    /// <summary>
    /// Returns a random item from the pool
    /// </summary>
    /// <returns></returns>
    public ItemSO PickRandomItem()
    {
        int randomIndex = Random.Range(0, itemList.Count);
        return itemList[randomIndex];
    }


    public ItemSO RerollRandomItem()
    {
        // Remove price of reroll, return null if not enough

        if (!spiritTracker.SpendRunSpirits(type, currentRerollPrice))
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
        bool success = spiritTracker.SpendRunSpirits(type, item.price);

        if (success)
        {
            itemList.Remove(item);
            ownedItems.Add(item);
            item.owned = true;
        }
        return success;
    }

}
