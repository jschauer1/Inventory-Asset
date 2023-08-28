using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInventoryDict : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] string inventoryName;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Inventory inventory = InventoryController.instance.GetInventory(inventoryName);
            inventory.TestPrintItemPosDict();
        }
    }
}
