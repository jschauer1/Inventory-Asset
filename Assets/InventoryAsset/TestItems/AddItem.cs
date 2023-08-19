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
        for (int i = 0; i < amount; i++)
        {
            InventoryController.instance.AddItem(inventory, item);
        }
    }
}
