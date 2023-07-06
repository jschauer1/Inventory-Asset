using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

[System.Serializable]
public class Inventory 
{
    private Dictionary<string, List<int>> itemPositions;

    [SerializeField, HideInInspector]
    private List<Item> items;

    [SerializeField, HideInInspector]
    private string inventoryName;

    [SerializeField, HideInInspector]
    private GameObject InventoryUIManager;
    private int curInventoryLoc;
    public Inventory(GameObject InventoryUIManager,string name,int size)
    {
        this.InventoryUIManager = InventoryUIManager;
        this.inventoryName = name;
        items = new List<Item>(size);
        itemPositions = new Dictionary<string, List<int>>();
        FillInventory(size);
    }
    public void AddItem(Item item)
    {
        items[curInventoryLoc] = item;

/*        if (!itemPositions.ContainsKey(item.GetItemType()))
        {
            itemPositions.Add(item.GetItemType(), new List<int>());
            itemPositions[item.GetItemType()].Add(curInventoryLoc);
        }
        else
        {
            items[curInventoryLoc] = item;
        }*/
        InventoryUIManager.GetComponent<InventoryUI>().UpdateSlot(curInventoryLoc);
        curInventoryLoc++;

    }
    void FillInventory(int size)
    {
        for(int i = 0; i < size; i ++)
        {
            Item filler = new Item(false);
            items.Add(filler);
        }
    }
    public void AddItem(Item item, int position)
    {
    }
    public Item InventoryGetItem(int index)
    {
        return items[index];
    }
    public string getName()
    {
        return inventoryName;
    }
    public void setManager(GameObject manager)
    {
        this.InventoryUIManager= manager;
    }
    public List<Item> getList()
    {
        return items;
    }
}