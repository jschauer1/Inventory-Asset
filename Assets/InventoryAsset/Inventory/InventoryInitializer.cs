using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Author: Jaxon Schauer
/// <summary>
/// Represents the initial configuration for an inventory.
/// This class defines various properties such as dimensions, behavior, and interactive elements for an inventory system.
/// </summary>
[System.Serializable]
public class InventoryInitializer
{
    [Header("========[ Basic Inventory Info ]========")]

    [Tooltip("Descriptive name or identifier for the inventory.")]
    [SerializeField]
    private string inventoryName;

    [Tooltip("Number of rows for the inventory layout.")]
    [SerializeField]
    private int row;

    [Tooltip("Number of columns for the inventory layout.")]
    [SerializeField]
    private int col;

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
