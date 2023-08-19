using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Item 
{
    private int amount;

    string itemType;
    private Sprite itemImage;
    private int maxStackAmount;
    private bool draggable;
    private bool highlightable;
    private UnityEvent myEvent;
    private bool isNull = false;
    public Item(ItemInitializer init)
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
    public Item(Item other)
    {
        if (other == null)
        {
            throw new ArgumentNullException(nameof(other), "Passed argument is null");
        }

        this.amount = other.amount;
        this.itemType = other.itemType != null ? string.Copy(other.itemType) : null;
        this.itemImage = other.itemImage;

        this.maxStackAmount = other.maxStackAmount;
        this.draggable = other.draggable;
        this.highlightable = other.highlightable;
        this.myEvent = other.myEvent;
        this.isNull = other.isNull;
    }

    public Item(bool isNull)
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
