using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

[System.Serializable]
public class Inventory 
{
    private Dictionary<string, List<int>> itemPositions;

    private List<Item> items;

    [SerializeField, HideInInspector]
    private string inventoryName;

    [SerializeField, HideInInspector]
    private GameObject InventoryUIManager;
    private int curInventoryLoc;
    [SerializeField, HideInInspector]
    int size;
    public Inventory(GameObject InventoryUIManager,string name,int size)
    {
        this.InventoryUIManager = InventoryUIManager;
        this.inventoryName = name;
        items = new List<Item>(size);
        this.size = size;
        FillInventory(size);
    }
    public void init()
    {
        itemPositions = new Dictionary<string, List<int>>();
        items = new List<Item>(size);
        FillInventory(size);
    }
    public void ReSize(int newSize)
    {
        if (items == null)
        {
            return;
        }
        for (int i = size; i < newSize; i++)
        {
            Item filler = new Item(true);
            items.Add(filler);
        }
    }
    public void AddItem(Item item)
    {
        if(itemPositions.ContainsKey(item.GetItemType()))
        {
            for(int i = 0; i < itemPositions[item.GetItemType()].Count; i++)
            {
                int position = itemPositions[item.GetItemType()][i];
                if (items[position].GetItemStackAmount() > items[position].GetAmount())
                {
                    items[position].SetAmount(items[position].GetAmount() + 1);
                    InventoryUIManager.GetComponent<InventoryUI>().UpdateSlot(position);
                    return;
                }
            }
            newItemInit(item);
        }
        else
        {
            newItemInit(item);
        }

    }
    public void AddItem(Item item, int position)
    {
        if (items == null)
        {
            return;
        }
        if (items[position].GetIsNull())
        {
            items[position] = item;
            InventoryUIManager.GetComponent<InventoryUI>().UpdateSlot(position);
        }
    }
    private void newItemInit(Item item)
    {
        for (int i = 0; i < items.Count; i++)
        {

            if (items[i].GetIsNull())
            {
                Item newItem = new Item(item);
                items[i] = newItem;
                if(itemPositions.ContainsKey(item.GetItemType()))
                {
                    itemPositions[item.GetItemType()].Add(i);
                    InventoryUIManager.GetComponent<InventoryUI>().UpdateSlot(i);
                }
                else
                {
                    itemPositions.Add(item.GetItemType(), new List<int>());
                    itemPositions[item.GetItemType()].Add(i);
                    InventoryUIManager.GetComponent<InventoryUI>().UpdateSlot(i);
                }
                break;
            }
        }
    }

    public void ResetPosition(int position)
    {
        Item filler = new Item(true);
        items[position] = filler;
        InventoryUIManager.GetComponent<InventoryUI>().UpdateSlot(position);
    }
    void FillInventory(int size)
    {
        if (items == null)
        {
            return;
        }
        for (int i = 0; i < size; i ++)
        {
            Item filler = new Item(true);
            items.Add(filler);
        }
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