using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

[System.Serializable]
public class Inventory 
{
    Dictionary<string, List<int>> itemPositions;

    [SerializeField, HideInInspector]
    List<Item> items;

    [SerializeField, HideInInspector]
    private string name;

    [SerializeField, HideInInspector]
    GameObject manager;
    int curInventoryLoc;
    public Inventory(string name,int size)
    {
        this.name = name;
        itemPositions = new Dictionary<string, List<int>>();
        items = new List<Item>(size);
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
        manager.GetComponent<InventoryUI>().UpdateSlot(curInventoryLoc);
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
        return name;
    }
    public void setManager(GameObject manager)
    {
        this.manager= manager;
    }
    public List<Item> getList()
    {
        return items;
    }
}