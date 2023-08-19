using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory 
{
    private Dictionary<string, List<int>> itemPositions;

    private List<Item> items;

    [SerializeField, HideInInspector]
    private string inventoryName;
    [SerializeField, HideInInspector]
    InventoryUIManager InventoryUIManagerInstance;
    [SerializeField, HideInInspector]
    private GameObject InventoryUIManager;
    [SerializeField, HideInInspector]
    int size;
    [SerializeField, HideInInspector]
    bool saveInventory;
    public Inventory(GameObject InventoryUIManager,string name,int size, bool saveInventory)
    {
        this.InventoryUIManager = InventoryUIManager;
        this.inventoryName = name;
        items = new List<Item>(size);
        this.size = size;
        FillInventory(size);
        InventoryUIManagerInstance = InventoryUIManager.GetComponent<InventoryUIManager>();
        this.saveInventory = saveInventory;
    }
    public void Init()
    {
        itemPositions = new Dictionary<string, List<int>>();
        items = new List<Item>(size);
        FillInventory(size);
    }
    public void Resize(int newSize)
    {
        size = newSize; 
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
                    items[position].SetAmount(items[position].GetAmount() + item.GetAmount());
                    InventoryUIManager.GetComponent<InventoryUIManager>().UpdateSlot(position);
                    return;
                }
            }
            AddNewItem(item);
        }
        else
        {
            AddNewItem(item);
        }

    }
    public void AddItem(Item Item, int position)
    {
        if (items == null)
        {
            Debug.LogError("Items List Null");
            return;
        }
        Item newItem = new Item(Item);
        if (items[position].GetIsNull())
        {
            if (itemPositions.ContainsKey(newItem.GetItemType()))
            {
                itemPositions[newItem.GetItemType()].Add(position);
            }
            else
            {
                itemPositions.Add(newItem.GetItemType(), new List<int> { position });
            }    
            items[position] = newItem;
            InventoryUIManagerInstance.UpdateSlot(position);
        }
    }
    private void AddNewItem(Item item)
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
                    InventoryUIManagerInstance.UpdateSlot(i);
                }
                else
                {
                    itemPositions.Add(item.GetItemType(), new List<int>());
                    itemPositions[item.GetItemType()].Add(i);
                    InventoryUIManagerInstance.UpdateSlot(i);
                }
                break;
            }
        }
    }  

    public void ResetConnectedSlot(int position)
    {
        if (!items[position].GetIsNull())
        {
            if (itemPositions.ContainsKey(items[position].GetItemType()))
            {
                itemPositions[items[position].GetItemType()].Remove(position);
            }
            else
            {
                Debug.LogWarning("ItemPositions Dictitonary Setup Incorrectly");
            }
        }
        Item filler = new Item(true);
        items[position] = filler;
        InventoryUIManagerInstance.UpdateSlot(position);
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
        if(items == null)
        {
            Debug.LogError("Items List Null");
            return null;
        }
        return items[index];
    }
    public string GetName()
    {
        return inventoryName;
    }
    public void SetManager(GameObject manager)
    {
        this.InventoryUIManager= manager;
    }
    public List<Item> GetList()
    {
        return items;
    }
    public bool GetSaveInventory()
    {
        return saveInventory;
    }
}