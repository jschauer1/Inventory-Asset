using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGetItems : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            Debug.Log(InventoryController.instance.GetItem("Inventory", 26).GetItemType());
            Debug.Log(InventoryController.instance.GetItem("Inventory", -1).GetItemType());

        }
    }
}
