using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/*Author: Jaxon Schauer
 * This class makes an inventoryItem that is used to track the items throughout the inventory
 */
public class InventoryItem 
{
    private int amount;

    string itemType;
    private Sprite itemImage;//Holds image of item
    private int maxStackAmount;
    private bool draggable;
    private bool pressable;
    private InventoryItemEvent itemEvent;
    private GameObject relatedGameObject;
    private bool isNull = false;//Checks if item exists
    private bool displayAmount;
    private int position;
    private string inventory;
    internal InventoryItem(ItemInitializer init)
    {
        this.amount = 1;
        this.itemType = init.GetItemType();
        this.itemImage = init.GetItemImage();
        this.maxStackAmount = init.GetItemStackAmount();
        this.draggable = init.GetDraggable();
        this.pressable = init.GetPressable();
        this.itemEvent = init.GetEvent();
        this.isNull = init.GetIsNull();
        this.relatedGameObject=init.GetRelatedGameObject();
        this.displayAmount = init.GetDisplayAmount();
    }
    public InventoryItem(InventoryItem other, int amount = 1)
    {
        if (other == null)
        {
            throw new ArgumentNullException(nameof(other), "Passed argument is null");
        }

        this.amount = amount;
        this.itemType = other.itemType != null ? string.Copy(other.itemType) : null;
        this.itemImage = other.itemImage;

        this.maxStackAmount = other.maxStackAmount;
        this.draggable = other.draggable;
        this.pressable = other.pressable;
        this.itemEvent = other.itemEvent;
        this.isNull = other.isNull;
        this.relatedGameObject = other.GetRelatedGameObject();
        this.displayAmount = other.GetDisplayAmount();
        this.inventory = other.inventory;
        this.position = other.position;
    }

    public InventoryItem(bool isNull)
    {
        amount = 1;
        this.isNull = isNull;
    }
    public void SetIsNull(bool isNull)
    {
        this.isNull = isNull;
    }
    public bool GetIsNull()
    {
        return isNull;
    }
    public string GetItemType()
    {
        return itemType;
    }
    public Sprite GetItemImage()
    {
        return itemImage;
    }
    public int GetItemStackAmount()
    {
        return maxStackAmount;
    }
    public void Selected()
    {
        if (itemEvent != null)
            itemEvent.Invoke(this);
    }
    public bool GetPressable()
    {
        return pressable;
    }
    public bool GetDraggable()
    {
        return draggable;
    }
    public int GetAmount()
    {
        return amount;
    }
    public void SetAmount(int amount)
    {
        this.amount = amount;
    }
    public bool GetDisplayAmount()
    {
        return displayAmount;
    }
    public GameObject GetRelatedGameObject()
    {
        return relatedGameObject;
    }
    public void SetPressable(bool pressable)
    {
        this.pressable = pressable; 
    }
    public void SetPosition(int position)
    {
        this.position = position;
    }
    public int GetPosition()
    {
        return position;
    }
    public void SetInventory(string inventory)
    {
        this.inventory = inventory;
    }
    public string GetInventory()
    {
        return inventory;
    }
    public override string ToString()
    {
        string result = $@"
        ItemType: {itemType}
        Inventory: {inventory}
        Item Position: {position}
        Item Amount: {amount}
        Max Item Amount: {maxStackAmount}";
        return result;



    }
}
