using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class Slot : MonoBehaviour
{
    [SerializeField]private int position;
    [SerializeField]private Item item;
    [SerializeField] private GameObject slotChildPrefab;

    [SerializeField]private GameObject slotChildInstance;
    Vector3 initalScale;
    private InventoryUI InvUI;
    private Vector3 initialSlotChildPosition;
    private void Awake()
    {
        InvUI = transform.parent.GetComponent<InventoryUI>();
        initialSlotChildPosition = slotChildInstance.transform.position;
        initalScale = slotChildInstance.transform.localScale;
    }
    private void Start()
    {
        item = InvUI.GetInventoryItem(position);
        if (!item.GetIsNull())
        {
            slotChildInstance.SetActive(true);
            slotChildInstance.GetComponent<Image>().sprite = item.GetItemImage();
        }
        else
        {
            slotChildInstance.SetActive(false);

        }
    }
    public void SetItem(Item item)
    {
        this.item = item;
        slotChildInstance.GetComponent<Image>().sprite = item.GetItemImage();

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
        if (!item.GetIsNull())
        {
            slotChildInstance.SetActive(true);
            slotChildInstance.GetComponent<DragItem>().SetItem(item);
            slotChildInstance.GetComponent<Image>().sprite = item.GetItemImage();
        }
        else
        {
            slotChildInstance.SetActive(false);

        }
    }
    public void ResetSlotChild()
    {
        InvUI.GetInventory().ResetPosition(position); 
        GameObject newInstance = Instantiate(slotChildPrefab, initialSlotChildPosition,Quaternion.identity);
        newInstance.transform.SetParent(transform);
        newInstance.transform.localScale = initalScale;
        slotChildInstance = newInstance;
        slotChildInstance.SetActive(false);

    }
    public InventoryUI GetInventoryUI()
    {
        return InvUI;
    }

}
