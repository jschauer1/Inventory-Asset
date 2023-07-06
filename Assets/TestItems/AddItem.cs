using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddItem : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private string inventory;
    [SerializeField] private string item;

    void Start()
    {
        InventoryController.instance.AddItem(inventory, item);
    }
}
