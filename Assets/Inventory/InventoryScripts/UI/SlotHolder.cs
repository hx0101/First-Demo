using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum SlotType
{
    Bag,
    Equipment,
    Ability
}

public class SlotHolder : MonoBehaviour,IPointerClickHandler
{
    public SlotType slotType;
    public ItemUI itemUI;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.clickCount % 2 == 0)
        {
            UseItem();
        }
    }

    public void UseItem()
    {
        if (itemUI.Bag.items[itemUI.Index].itemData != null)
        {
            if (itemUI.Bag.items[itemUI.Index].itemData.data.itemType == ItemType.Useable && itemUI.Bag.items[itemUI.Index].amount > 0)
            {
                itemUI.Bag.items[itemUI.Index].amount -= 1;
            }
            UpdateItem();
        }
    }

    public void UpdateItem()
    {
        switch (slotType)
        {
            case SlotType.Bag:
                itemUI.Bag = InventoryManagerInterface.Instance.inventoryData;
                break;
            case SlotType.Equipment:
                itemUI.Bag = InventoryManagerInterface.Instance.equipmentData;
                break;
            case SlotType.Ability:
                itemUI.Bag = InventoryManagerInterface.Instance.abilityData;
                break;
        }

        var item = itemUI.Bag.items[itemUI.Index];

        itemUI.SetupItemUI(item.itemData, item.amount);
    }
}
