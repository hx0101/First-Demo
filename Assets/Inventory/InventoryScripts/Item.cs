using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{ 
    Equipment,
    Ability,
    Prop,
    Useable
}

[CreateAssetMenu(fileName = "New Item",menuName = "Inventory/New Item")]
public class Item : ScriptableObject
{
    public Itemdata data;
}
[System.Serializable]
public class Itemdata
{
    public ItemType itemType;
    public string itemName;
    public Sprite itemIcon;
    public int itemAmount;

    [TextArea]
    public string description = "";
    public bool stackable;
}