using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManagerInterface : Singleton<InventoryManagerInterface>
{
    [Header("Inventory Data")]
    public Inventory inventoryData;
    public Inventory abilityData;
    public Inventory equipmentData;
}
