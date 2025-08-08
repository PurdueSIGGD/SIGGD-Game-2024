using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPool : MonoBehaviour
{
    private List<ItemSO> usedItems; // When item is bought, cannot be bought for rest of run

    [SerializeField]
    public ItemSO.ItemType type;

    [SerializeField]
    public List<ItemSO> itemList;


    public ItemSO PickRandomItem()
    {
        return itemList[0];
    }

    /// <summary>
    /// After run, return all the items to the pool (can be bought again
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
}
