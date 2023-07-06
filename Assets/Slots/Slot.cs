using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class Slot : MonoBehaviour
{
    [SerializeField]private int position;
    [SerializeField]private Item item;
    [SerializeField]private Image image;
    [SerializeField]private GameObject slotChild;
    private void Start()
    {
        item = transform.parent.GetComponent<InventoryUI>().GetInventoryItem(position);
        if (item.GetIsNull())
        {
            slotChild.SetActive(true);

            image.sprite = item.GetItemImage();
        }
        else
        {
            slotChild.SetActive(false);

        }
    }
    public void SetItem(Item item)
    {
        this.item = item;
        image.sprite = item.GetItemImage();
    }
    public Item GetItem()
    {
        return item;
    }
    public void SetPosition(int position)
    {
        this.position = position;   
    }
    public int GetPosition()
    {
        return position;
    }
    public void UpdateSlot()
    {

        item = transform.parent.GetComponent<InventoryUI>().GetInventoryItem(position);
        if (item.GetIsNull())
        {
            print("At Slot UPdate");

            slotChild.SetActive(true);

            image.sprite = item.GetItemImage();
        }
        else
        {
            slotChild.SetActive(false);

        }
    }

}
