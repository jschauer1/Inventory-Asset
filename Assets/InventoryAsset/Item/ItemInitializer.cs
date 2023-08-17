using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;
using static UnityEditor.Progress;
using System.Collections.Concurrent;
using System;

[System.Serializable]
public class ItemInitializer
{
    private int amount = 1;

    [SerializeField] string itemType;
    [SerializeField] private Sprite itemImage;
    [SerializeField] private int maxStackAmount;
    [SerializeField] private bool draggable;
    [SerializeField] private bool highlightable;
    [SerializeField, HideInInspector]
    private UnityEvent myEvent;


    [SerializeField, HideInInspector]
    private bool isNull = false;
    public ItemInitializer(bool isNull)
    {
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
    public UnityEvent GetEvent()
    {
        return myEvent;
    }

}