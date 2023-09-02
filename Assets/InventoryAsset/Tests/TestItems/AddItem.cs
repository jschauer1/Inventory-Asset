using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddItem : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private string inventory;
    [SerializeField] private string item;
    [SerializeField] private int amount;

    void Start()
    {
        if(InventoryController.instance != null)
        InventoryController.instance.AddItemLinearly(inventory, item, amount);
    }
}
