using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class InventoryInitializer
{
    [SerializeField] private string inventoryName;
    [SerializeField] private int row;
    [SerializeField] private int col;
    [SerializeField] private bool draggable;
    [SerializeField] private bool highlightable;
    [SerializeField] private bool SaveInventory;
    [SerializeField] private char EnableDisableOnPress;
    [SerializeField, HideInInspector]
    private bool initialized = false;
    public string GetInventoryName()
    {
        return inventoryName;
    }
    public int GetRow()
    {
        return row;
    }
    public int GetCol()
    {
        return col;
    }
    public bool GetHightlightable()
    {
        return highlightable;
    }
    public bool GetDraggable()
    {
        return draggable;
    }
    public void SetRow(int row)
    {
        this.row = row;
    }
    public void SetCol(int col)
    {
        this.col = col;
    }
    public void SetInventoryName(string inventoryName)
    {
        this.inventoryName = inventoryName;
    }
    public bool GetInitialized()
    {
        return initialized;
    }
    public void SetInitialized(bool initialized)
    {
        this.initialized = initialized;
    }
    public void SetEnableDisable(string EnableDisableOnPress)
    {
        this.EnableDisableOnPress = EnableDisableOnPress.ToCharArray()[0];
    }
    public string GetEnableDisable()
    {
        return EnableDisableOnPress.ToString();
    }
    public bool GetSaveInventory()
    {
        return SaveInventory;
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(inventoryName, row, col);
    }

    public override bool Equals(object obj)
    {
        return obj is InventoryInitializer initializer &&
               inventoryName == initializer.inventoryName &&
               row == initializer.row &&
               col == initializer.col;
    }
    public void Copy(InventoryInitializer initilizer)
    {
        inventoryName = initilizer.inventoryName;  
        row = initilizer.row;
        col = initilizer.col;
    }
}
