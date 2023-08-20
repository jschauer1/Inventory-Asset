using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class InventoryData
{
    public Dictionary<string, List<string>> inventories;

    public string itemType;
    public InventoryData(Dictionary<string, Inventory> inventoryManager)
    {
        inventories = new Dictionary<string, List<string>>();
        foreach (var pair in inventoryManager)
        {
            if (!inventoryManager[pair.Key].GetSaveInventory())
            {
                continue;
            }
            List<string> itemsStr = new List<string>();
            Inventory inventory = pair.Value;
            foreach(Item item in inventory.GetList())
            {
                itemsStr.Add(item.GetItemType());
            }
            inventories.Add(inventory.GetName(), itemsStr);
        }
    }
}
