using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ItemUI))]
public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    ItemUI currentItemUI;
    SlotHolder currentHolder;
    SlotHolder targetHolder;

    void Awake()
    {
        currentItemUI = GetComponent<ItemUI>();
        Debug.Log(currentItemUI);
        currentHolder = GetComponentInParent<SlotHolder>();
        Debug.Log(currentHolder);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        InventoryManager.Instance.currentDrag = new InventoryManager.DragData();
        InventoryManager.Instance.currentDrag.originalHolder = GetComponentInParent<SlotHolder>();
        InventoryManager.Instance.currentDrag.originalParent = (RectTransform)transform.parent;
        transform.SetParent(InventoryManager.Instance.MovePanel.transform);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            if (InventoryManager.Instance.CheckInInventoryUI(eventData.position)
               || InventoryManager.Instance.CheckInActionUI(eventData.position)
               || InventoryManager.Instance.CheckInEquipmentUI(eventData.position))
            {

                if (eventData.pointerEnter.gameObject.GetComponent<SlotHolder>())
                {
                    targetHolder = eventData.pointerEnter.gameObject.GetComponent<SlotHolder>();
                }
                else
                {
                    targetHolder = eventData.pointerEnter.gameObject.GetComponentInParent<SlotHolder>();
                }
                
                Debug.Log(targetHolder);
                switch (targetHolder.slotType)
                {
                    case SlotType.Bag:
                        SwapItem();
                        break;
                    case SlotType.Ability:
                        if (currentItemUI.Bag.items[currentItemUI.Index].itemData.data.itemType == ItemType.Ability)
                        {
                            SwapItem();
                        }
                        break;
                    case SlotType.Equipment:
                        if (currentItemUI.Bag.items[currentItemUI.Index].itemData.data.itemType == ItemType.Equipment)
                        {
                            SwapItem();
                        }
                        break;
                }

                currentHolder.UpdateItem();
                targetHolder.UpdateItem();
            }
        }

        transform.SetParent(InventoryManager.Instance.currentDrag.originalParent);
        RectTransform t = transform as RectTransform;
        t.offsetMax = -Vector2.one * 5;
        t.offsetMin = Vector2.one * 5;
    }

    public void SwapItem()
    {
        var targetItem = targetHolder.itemUI.Bag.items[targetHolder.itemUI.Index];
        var tempItem = currentHolder.itemUI.Bag.items[currentHolder.itemUI.Index];

        bool isSameItem = tempItem.itemData == targetItem.itemData;

        if (isSameItem && targetItem.itemData.data.stackable)
        {
            targetItem.amount += tempItem.amount;
            tempItem.itemData = null;
            tempItem.amount = 0;
        }
        else
        {
            currentHolder.itemUI.Bag.items[currentHolder.itemUI.Index] = targetItem;
            targetHolder.itemUI.Bag.items[targetHolder.itemUI.Index] = tempItem;
        }
    }
}
