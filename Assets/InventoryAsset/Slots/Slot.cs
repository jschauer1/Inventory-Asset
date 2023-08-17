using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Slot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]private int position;
    [SerializeField]private GameObject slotChildPrefab;
    [SerializeField]private GameObject slotChildInstance;

    private Item item;
    private Color color;
    Vector3 initalScale;
    private Image slotImage;

    private InventoryUIManager InvUI;
    private Vector3 initialSlotChildPosition;
    private void Awake()
    {
        slotImage = GetComponent<Image>();
        color = slotImage.color;

        InvUI = transform.parent.GetComponent<InventoryUIManager>();
        initialSlotChildPosition = slotChildInstance.transform.position;
        initalScale = slotChildInstance.transform.localScale;
    }
    private void Start()
    {
        item = InvUI.GetInventoryItem(position);
        UpdateSlot();
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

        item = transform.parent.GetComponent<InventoryUIManager>().GetInventoryItem(position);
        if (!item.GetIsNull())
        {
            slotChildInstance.SetActive(true);
            slotChildInstance.GetComponent<DragItem>().SetItem(item);
            slotChildInstance.GetComponent<Image>().sprite = item.GetItemImage();
            slotChildInstance.GetComponent<DragItem>()._SetText();
        }
        else
        {
            slotChildInstance.SetActive(false);

        }
    }
    public void ResetSlotChild()
    {
        GameObject newInstance = Instantiate(slotChildPrefab, initialSlotChildPosition,Quaternion.identity);
        newInstance.transform.SetParent(transform);
        newInstance.transform.localScale = initalScale;
        slotChildInstance = newInstance;
        InvUI.GetInventory().ResetConnectedSlot(position);
        slotChildInstance.SetActive(false);

    }
    public Image GetSlotImage()
    {
        return slotImage;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (item.GetIsNull() || !item.GetHighlightable())
        {
            return;
        }
        InvUI.GetComponent<InventoryUIManager>().SetHightlighted(gameObject);
    }
    public InventoryUIManager GetInventoryUI()
    {
        return InvUI;
    }
    public Color GetColor()
    {
        return color;
    }
}
