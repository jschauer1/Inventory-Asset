using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TestAddToggleKey : MonoBehaviour
{
    bool check = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            if(check)
            {
                InventoryController.instance.AddToggleKey("Chest", 'E');
                check = false;
            }
            else
            {
                InventoryController.instance.RemoveToggleKey("Chest", 'E');
                check = true;
            }
        }
    }
}
