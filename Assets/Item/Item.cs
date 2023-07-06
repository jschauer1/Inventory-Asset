using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
[System.Serializable]
public class Item
{
    [SerializeField] string ItemType;
    [SerializeField] private Sprite itemImage;
    [SerializeField] private int maxStackAmount;
    [SerializeField] private bool isNull;

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
        return ItemType;
    }
    public Sprite GetItemImage()
    {
        return itemImage;
    }
    public int GetItemStackAmount()
    {
        return maxStackAmount;
    }
}
