using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

[System.Serializable]
public class Item
{
    public string itemName;
    // ... other item properties ...
}
public class DropDownList : MonoBehaviour
{
    [SerializeField]
    public List<Item> items = new List<Item>();

    [HideInInspector]
    public int selectedItemIndex = 0;
}

