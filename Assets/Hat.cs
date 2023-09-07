using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hat : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    string hatName;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            InventoryController.instance.AddItemLinearly("Hotbar", hatName);
            Destroy(gameObject);
        }
    }
}
