using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ItemSO", order = 1)]
public class ItemSO : ScriptableObject
{

    [HideInInspector]
    public bool owned = false;
    [HideInInspector]
    public bool used = false;

    public enum ItemType
    {
        RED,
        BLUE,
        YELLOW
    }

    // Item Attributes

    public string displayName;

    public Sprite itemIcon;

    [Header("Attributes")]

    public ItemType itemType;

    public int price;

    [TextArea]
    public string itemDescription;


}
