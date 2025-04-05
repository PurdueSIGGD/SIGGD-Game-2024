using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUIManager : MonoBehaviour
{
    public GameObject itemUIBox;
    public Transform Panel;

    void Start()
    {
        createItemBox("Item 1");
    }

    public void createItemBox(string itemName) {
        GameObject item = Instantiate(itemUIBox, Vector3.zero, Quaternion.identity);
        item.transform.SetParent(Panel);
        item.GetComponent<ItemUI>().SetItemName(itemName);
    }
}
