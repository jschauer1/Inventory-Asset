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
    private Sprite itemImage;
    private int maxStackAmount;
    private bool draggable;
    private bool highlightable;
    private UnityEvent myEvent;
    private bool isNull = false;
    public InventoryItem(ItemInitializer init)
    {
        this.amount = 1;
        this.itemType = init.GetItemType();
        this.itemImage = init.GetItemImage();
        this.maxStackAmount = init.GetItemStackAmount();
        this.draggable = init.GetDraggable();
        this.highlightable = init.GetHighlightable();
        this.myEvent = init.GetEvent();
        this.isNull = init.GetIsNull();
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
        this.highlightable = other.highlightable;
        this.myEvent = other.myEvent;
        this.isNull = other.isNull;
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
        if (myEvent != null)
            myEvent.Invoke();
    }
    public bool GetHighlightable()
    {
        return highlightable;
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
}
