using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
internal class InventoryData
{
    public Dictionary<string, List<ItemData>> inventories;

    public string itemType;
    public InventoryData(Dictionary<string, Inventory> inventoryManager)
    {
        inventories = new Dictionary<string, List<ItemData>>();
        foreach (var pair in inventoryManager)
        {
            int position = 0;
            if (!inventoryManager[pair.Key].GetSaveInventory())
            {
                continue;
            }
            List<ItemData> itemData = new List<ItemData>();
            Inventory inventory = pair.Value;
            foreach(InventoryItem item in inventory.GetList())
            {
                itemData.Add(new ItemData(item.GetAmount(),item.GetItemType(), position));
                position++;
            }
            inventories.Add(inventory.GetName(), itemData);
        }
    }
}
