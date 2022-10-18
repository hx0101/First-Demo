using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCollect : MonoBehaviour
{
    public Item itemData;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerFSM playerFsm = other.gameObject.GetComponent<PlayerFSM>();
        
        if (playerFsm != null)
        {
            InventoryManagerInterface.Instance.inventoryData.AddItem(itemData, itemData.data.itemAmount);
            gameObject.SetActive(false);
        }

    }
}
