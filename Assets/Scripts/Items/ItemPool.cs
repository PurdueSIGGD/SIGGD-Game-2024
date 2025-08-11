using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemPool : MonoBehaviour
{
    private List<ItemSO> usedItems; // When item is bought, cannot be bought for rest of run

    [SerializeField] private SpiritShopManager spiritShopManager;

    [SerializeField]
    public Spirit.SpiritType type;

    [SerializeField]
    public List<ItemSO> itemList;    

    [SerializeField] private int rerollStartPrice;
    [SerializeField] private int rerollPriceIncrement;

    public int currentRerollPrice = 0; // Current reroll price

    private void Start()
    {
        currentRerollPrice = rerollStartPrice;
    }

    /// <summary>
    /// Returns a random item from the pool and sets the currently displayed item
    /// </summary>
    /// <returns></returns>
    public ItemSO PickRandomItem()
    {
        int randomIndex = Random.Range(0, itemList.Count);
        Debug.Log(randomIndex);
        return itemList[randomIndex];
    }

    public ItemSO 

    /// <summary>
    /// After run, return all the items to the pool (can be bought again)
    /// </summary>
    public void ReturnItemsToPool()
    {
        foreach (ItemSO item in usedItems)
        {
            item.used = false;
            item.owned = false;

            usedItems.Remove(item);
            itemList.Add(item);
        }
    }

    /// <summary>
    /// Handles reroll button
    /// </summary>
    private void Reroll()
    {
        
    }
}
