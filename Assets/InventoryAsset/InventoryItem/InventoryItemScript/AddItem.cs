using System.Collections.Generic;
using UnityEngine;
namespace InventorySystem
{
    public class AddItem : MonoBehaviour
    {
        [HideInInspector]
        public List<ItemInitializer> items;

        [HideInInspector]
        public List<InventoryInitializer> inventories;

        [HideInInspector]
        public int selectedItemIndex = 0;

        [HideInInspector]
        public int SelectedInventoryIndex = 0;
        [SerializeField, HideInInspector]
        public ItemInitializer item;
        [SerializeField, HideInInspector]
        public InventoryInitializer inventory;
        [SerializeField, HideInInspector]
        GameObject controller;

        public void FindController()
        {
            controller = GameObject.Find("InventoryController");

        }
        public void GetItemList()
        {
            if (controller == null)
            {
                FindController();
            }
            items = controller.GetComponent<InventoryController>().items;
        }
        public void GetInventoryList()
        {
            if (controller == null)
            {
                FindController();
            }
            inventories = controller.GetComponent<InventoryController>().initializeInventory;
        }
        public void SetItem(ItemInitializer init)
        {
            item = init;
        }
        public void SetInventory(InventoryInitializer init)
        {
            inventory = init;
        }
        public void Start()
        {
            InventoryController.instance.AddItemLinearly(inventory.GetInventoryName(), item.GetItemType());
        }
    }
}

