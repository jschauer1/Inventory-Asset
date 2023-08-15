using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;
using static UnityEditor.Progress;

[System.Serializable]
public class Item
{
    [SerializeField] string itemType;
    [SerializeField] private Sprite itemImage;
    [SerializeField] private int maxStackAmount;
    [SerializeField]private bool draggable;
    [SerializeField]private bool highlightable;
    [SerializeField, HideInInspector]
    private UnityEvent myEvent;

    [SerializeField,HideInInspector] 
    private bool isNull = false;
    public Item(bool isNull)
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
}
