using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using static UnityEditor.Progress;

/*
 * This class defines an inventory controller, which allows for creating new inventories and defining valid types of objects.
 * Only one InventoryController should be instantiated within a project. Multiple inventories can be created from one controller.
 * This controller manages all information transfered between the inventories.
 */

public class InventoryController : MonoBehaviour
{
    [SerializeField] 
    private Transform UI; // UI canvas to build inventories on.
    [SerializeField] 
    public List<Item> items; // Accepted items that can be added by name to the inventory.
    [SerializeField] 
    private List<InventoryInitializer> initializeInventory; // Information about the inventory specified through the manager.

    [SerializeField, HideInInspector]
    private List<InventoryInitializer> prevInventoryTracker; // Previously initialized inventories, so they are not initialized again.


    [SerializeField] 
    private GameObject inventoryManagerObj; // Prefab for the inventory manager.

    [SerializeField, HideInInspector]
    private List<GameObject> allInventoryUI = new List<GameObject>(); // Holds all inventory UI instances for each inventory created.
    private Dictionary<string, Inventory> inventoryManager= new Dictionary<string, Inventory>(); // Dictionary to map inventory names to their objects.
    private Dictionary<string, Item> itemManager = new Dictionary<string, Item>(); // Dictionary to map item names to their objects.

    public static InventoryController instance; // Shared instance of the InventoryController to enforce only one being created.

    // TODO: rename this
    [SerializeField] 
    private bool isInstance = false; // Whether to use this object as the sole instance of InventoryController (cannot have multiple set to true).

    /// <summary>
    /// Check whether an instance of InventoryController has already been created. If it has, delete this instance.
    /// Initialize inventories specified by the user in the controller.
    /// </summary>
    private void Awake()
    {
        if (isInstance)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        InitializeInventories();
    }

    /// <summary>
    /// 
    /// </summary>
    public void InitializeInventories()
    {
        RemoveDeletedInventories();
        InitializeNewInventories();
        UpdateInventoryTracker();
        AllignDictionaries();
        InitializeItems();
    }
    public Inventory GetInventory(string name)
    {
        return inventoryManager[name];
    }
    private void UpdateInventoryTracker()
    {
        prevInventoryTracker.Clear();
        for (int i = 0; i < initializeInventory.Count; i++)
        {
            InventoryInitializer InitilizerCopy = new InventoryInitializer();
            InitilizerCopy.Copy(initializeInventory[i]);
            prevInventoryTracker.Add(InitilizerCopy);
        }
    }
    private void InitializeNewInventories()
    {
        foreach (InventoryInitializer initializer in initializeInventory)
        {
            if (!prevInventoryTracker.Contains(initializer))
            {
                initializer.SetInitialized(true);
                GameObject tempinventoryUI = Instantiate(inventoryManagerObj, transform.position, Quaternion.identity, UI);
                tempinventoryUI.SetActive(true);
                tempinventoryUI.name = initializer.GetInventoryName();
                allInventoryUI.Add(tempinventoryUI);

                string inventoryName = initializer.GetInventoryName();
                int InventorySize = initializer.GetRow() * initializer.GetCol();
                Inventory curInventory = new Inventory(tempinventoryUI,inventoryName, InventorySize);

                inventoryManager.Add(inventoryName, curInventory);

                InventoryUI inventoryUI = tempinventoryUI.GetComponent<InventoryUI>();
               
                inventoryUI.SetInventory(ref curInventory);
                inventoryUI.SetHighlightable(initializer.GetHightlightable());
                inventoryUI.SetDraggable(initializer.GetDraggable());
                inventoryUI.SetRowCol(initializer.GetRow(), initializer.GetCol());
                inventoryUI.SetInventoryName(initializer.GetInventoryName());
                inventoryUI.UpdateInventoryDisplay();
            }
        }
        foreach(GameObject inObjects in allInventoryUI)
        {
            inObjects.GetComponent<InventoryUI>().UpdateInventoryDisplay();
        }
    }
    private void RemoveDeletedInventories()
    {

        List<GameObject> toremove = new List<GameObject>();
        foreach (InventoryInitializer initializer in prevInventoryTracker)
        {
            if (!initializeInventory.Contains(initializer))
            {
                foreach (GameObject UI in allInventoryUI)
                {
                    InventoryUI UIInstance = UI.GetComponent<InventoryUI>();
                    if (UIInstance.GetInventoryName() == initializer.GetInventoryName())
                    {
                        toremove.Add(UI);
                        inventoryManager.Remove(UIInstance.GetInventoryName());
                    }
                }
            }
        }

        foreach (GameObject remove in toremove)
        {
            allInventoryUI.Remove(remove);
            DestroyImmediate(remove);

        }
    }
    private void InitializeItems()
    {
        itemManager.Clear();
        foreach (Item item in items)
        {
            itemManager.Add(item.GetItemType(), item);
        }
    }
    public void AddItem(string inventoryName, string itemType)
    {
        Inventory inventory = inventoryManager[inventoryName];
        Item item = itemManager[itemType];
        inventory.AddItem(item);
    }
    public void AddItem(string inventoryName, string itemType, int position)
    {
        Inventory inventory = inventoryManager[inventoryName];
        Item item = itemManager[itemType];
        inventory.AddItem(item, position);
    }
    public void ResetInventory()
    {
        inventoryManager.Clear();
        itemManager.Clear();
        prevInventoryTracker.Clear();
        foreach (Item item in items)
        {
            itemManager.Add(item.GetItemType(), item);
        }
        foreach (GameObject obj in allInventoryUI)
        {
            DestroyImmediate(obj);
        }
        allInventoryUI.Clear();
    }
    public void AllignDictionaries()
    {
        inventoryManager.Clear();
        foreach (GameObject inventories in allInventoryUI)
        {
            InventoryUI inventoryInstance = inventories.GetComponent<InventoryUI>();
            inventoryManager.Add(inventoryInstance.GetInventoryName(), inventoryInstance.GetInventory());
        }
    }
    public Transform GetUI()
    {
        return UI;
    }
    public List<Item> GetItems()
    {
        return items;
    }
}
