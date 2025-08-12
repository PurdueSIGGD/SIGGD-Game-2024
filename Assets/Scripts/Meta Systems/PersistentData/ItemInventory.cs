using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInventory : MonoBehaviour
{

    public List<ItemSO> ownedItems;
    void Start()
    {
        ownedItems = new();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
