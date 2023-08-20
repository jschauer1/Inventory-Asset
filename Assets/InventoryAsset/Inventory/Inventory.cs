using System.Collections.Generic;
using UnityEngine;
/* 
 * Author: Jaxon Schauer
 * This class creates an Inventory that tracks and controls the inventory list. This class tells the InventoryUIManager what objects each slot 
 * holds
 */

[System.Serializable]
public class Inventory 
{
    private Dictionary<string, List<int>> itemPositions;//Holds all positions of a given itemType in the list

    private List<InventoryItem> inventoryList;//Holds all inventory items in a list

    [SerializeField, HideInInspector]
    private string inventoryName;
    [SerializeField, HideInInspector]
    private GameObject InventoryUIManager;//holds the linked InventoryUIManager GameObject
    [SerializeField, HideInInspector]
    InventoryUIManager InventoryUIManagerInstance;//Holds an instance of the linked InventoryUIManager class
    [SerializeField, HideInInspector]
    int size;
    [SerializeField, HideInInspector]
    bool saveInventory;//is true if the user decides to save the inventory

    /// <summary>
    /// Assigns essential variables for the Inventory
    /// </summary>
    public Inventory(GameObject InventoryUIManager,string name,int size, bool saveInventory)
    {
        this.InventoryUIManager = InventoryUIManager;
        this.inventoryName = name;
        inventoryList = new List<InventoryItem>(size);
        this.size = size;
        FillInventory(size);
        InventoryUIManagerInstance = InventoryUIManager.GetComponent<InventoryUIManager>();
        this.saveInventory = saveInventory;
    }
    /// <summary>
    /// Initializes aspects of the inventory that do not transfer into play mode.
    /// </summary>
    public void Init()
    {
        itemPositions = new Dictionary<string, List<int>>();
        inventoryList = new List<InventoryItem>(size);
        FillInventory(size);
    }
    /// <summary>
    /// Resizes the inventory when <see cref="InventoryUIManager.UpdateInventoryUI"/> is called
    /// </summary>
    public void Resize(int newSize)
    {
        size = newSize; 
        if (inventoryList == null)
        {
            return;
        }
        for (int i = size; i < newSize; i++)
        {
            InventoryItem filler = new InventoryItem(true);
            inventoryList.Add(filler);
        }
    }
    /// <summary>
    /// Adds an item to a specified position, updating the <see cref="itemPositions"/> for efficient tracking of the items
    /// </summary>
    public void AddItem(int position,InventoryItem Item, int amount = 1)
    {
        if (inventoryList == null)
        {
            Debug.LogError("Items List Null");
            return;
        }
        InventoryItem newItem = new InventoryItem(Item, Item.GetAmount());
        if (inventoryList[position].GetIsNull())
        {
            if (itemPositions.ContainsKey(newItem.GetItemType()))
            {
                itemPositions[newItem.GetItemType()].Add(position);
            }
            else
            {
                itemPositions.Add(newItem.GetItemType(), new List<int> { position });
            }    
            inventoryList[position] = newItem;
            InventoryUIManagerInstance.UpdateSlot(position);
        }
    }
    /// <summary>
    /// Takes an item as input
    /// Adds the item at the lowest possible inventory location, adding it into the <see cref="itemPositions"/> to allow for efficient tracking of the inventory items
    /// </summary>
    public void AddItem(InventoryItem item, int amount = 1)
    {
        if (itemPositions.ContainsKey(item.GetItemType()))
        {
            for (int i = 0; i < itemPositions[item.GetItemType()].Count; i++)
            {
                int position = itemPositions[item.GetItemType()][i];
                if (inventoryList[position].GetItemStackAmount() > inventoryList[position].GetAmount())
                {
                    inventoryList[position].SetAmount(inventoryList[position].GetAmount() + item.GetAmount());
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
    /// <summary>
    /// Adds a new item in the lowest possible inventoryList position
    /// </summary>
    private void AddNewItem(InventoryItem item, int amount = 1)
    {

        for (int i = 0; i < inventoryList.Count; i++)
        {

            if (inventoryList[i].GetIsNull())
            {

                InventoryItem newItem = new InventoryItem(item, amount);
                inventoryList[i] = newItem;
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
    /// <summary>
    /// Takes as input a position, remove the item from the given inventory position.
    /// </summary>
    public void RemoveItemInPosition(int position)
    {
        if (!inventoryList[position].GetIsNull())
        {
            if (itemPositions.ContainsKey(inventoryList[position].GetItemType()))
            {
                itemPositions[inventoryList[position].GetItemType()].Remove(position);
            }
            else
            {
                Debug.LogWarning("ItemPositions Dictitonary Setup Incorrectly");
            }
        }
        InventoryItem filler = new InventoryItem(true);
        inventoryList[position] = filler;
        InventoryUIManagerInstance.UpdateSlot(position);
    }
    /// <summary>
    /// Fills the inventory with empty items 
    /// </summary>
    void FillInventory(int size)
    {
        if (inventoryList == null)
        {
            return;
        }
        for (int i = 0; i < size; i ++)
        {
            InventoryItem filler = new InventoryItem(true);
            inventoryList.Add(filler);
        }
    }
    public InventoryItem InventoryGetItem(int index)
    {
        if(inventoryList == null)
        {
            Debug.LogError("Items List Null");
            return null;
        }
        return inventoryList[index];
    }
    public string GetName()
    {
        return inventoryName;
    }
    public void SetManager(GameObject manager)
    {
        this.InventoryUIManager= manager;
    }
    public List<InventoryItem> GetList()
    {
        return inventoryList;
    }
    public bool GetSaveInventory()
    {
        return saveInventory;
    }
}