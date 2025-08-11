using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ItemSO", order = 1)]
public class ItemSO : ScriptableObject
{

    [HideInInspector]
    public bool owned = false;
    [HideInInspector]
    public bool used = false;

    // Item Attributes

    public string displayName;

    public Sprite itemIcon;

    [Header("Attributes")]

    public Spirit.SpiritType itemType;

    public int price;

    [TextArea]
    public string itemDescription;


}
