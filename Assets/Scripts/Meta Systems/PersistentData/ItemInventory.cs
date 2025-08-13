using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInventory : MonoBehaviour
{

    public List<ItemSO> ownedItems;

    [SerializeField]
    public List<ItemSO> redItemPool;
    [SerializeField]
    public List<ItemSO> blueItemPool;
    [SerializeField]
    public List<ItemSO> yellowItemPool;

    void Start()
    {
        ownedItems = new();
    }

    /// <summary>
    /// After run, return all the items to the pool (can be bought again)
    /// </summary>
    public void ReturnItemsToPool()
    {

        foreach (ItemSO item in ownedItems)
        {
            item.owned = false;

            switch(item.itemType)
            {
                case Spirit.SpiritType.Blue:
                    blueItemPool.Add(item); 
                    break;
                case Spirit.SpiritType.Red:
                    redItemPool.Add(item);
                    break;
                case Spirit.SpiritType.Yellow:
                    yellowItemPool.Add(item);
                    break;

            }
        }

        ownedItems.Clear();

    }

}
