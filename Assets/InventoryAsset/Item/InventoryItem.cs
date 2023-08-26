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
    private bool highlightable;
    private UnityEvent itemEvent;//Calls function on invoke
    private GameObject linkedGameObject;
    private bool isNull = false;//Checks if item exists
    public InventoryItem(ItemInitializer init)
    {
        this.amount = 1;
        this.itemType = init.GetItemType();
        this.itemImage = init.GetItemImage();
        this.maxStackAmount = init.GetItemStackAmount();
        this.draggable = init.GetDraggable();
        this.highlightable = init.GetHighlightable();
        this.itemEvent = init.GetEvent();
        this.isNull = init.GetIsNull();
        this.linkedGameObject=init.GetGameObjectAction();   
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
        this.itemEvent = other.itemEvent;
        this.isNull = other.isNull;
        this.linkedGameObject = other.GetLinkedGameObject();

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
            itemEvent.Invoke();
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
    public GameObject GetLinkedGameObject()
    {
        if(linkedGameObject == null)
        {
            //Debug.LogError("Linked GameObject null");
        }
        return linkedGameObject;
    }
}
