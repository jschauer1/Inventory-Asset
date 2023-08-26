using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/*Author: Jaxon Schauer
 * This class creates a slot gameObject that displays an image of the item when notified by the assigned inventory
 */
public class Slot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private int position;//The position if the inventories items list
    [SerializeField]
    private GameObject slotChildPrefab;//This holds the prefab for the image allowing it to be instantiated when the child object is dragged to a new location
    [SerializeField]
    private GameObject slotChildInstance;//This is a child object that is used to display an image of the object

    private InventoryItem item;//This is the current item in the inventory, there is always an item however item.GetIsNull() determines if the object contains a real item
    private Color color;//This is the color of the slot
    private Image slotImage;//This is the image of the slot
    private InventoryUIManager inventoryUIManager;
    private Vector3 initialChildScale;//holds the scale for the slot child to allow for it to be instantiated with the correct size
    private Vector3 initialSlotChildPosition;//This holds the position of the slot child so it can be instantiated with the correct location
    /// <summary>
    /// Sets essential variables for the inventory slot
    /// </summary>
    private void Awake()
    {
        slotImage = GetComponent<Image>();
        color = slotImage.color;

        inventoryUIManager = transform.parent.GetComponent<InventoryUIManager>();
        initialChildScale = slotChildInstance.transform.localScale;
    }
    /// <summary>
    /// Initializes slot child, calling <see cref="UpdateSlot"/>
    /// </summary>
    private void Start()
    {

        item = inventoryUIManager.GetInventoryItem(position);
        UpdateSlot();
        initialSlotChildPosition = slotChildInstance.transform.position;

    }
    /// <summary>
    /// Updates the slot to display the item in the slots associated position
    /// </summary>
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
    /// <summary>
    /// Adds a new slotchild when slot child is dragged away, and resets the slot to empty
    /// </summary>
    public void ResetSlot()
    {
        GameObject newInstance = Instantiate(slotChildPrefab, initialSlotChildPosition,Quaternion.identity);
        newInstance.transform.SetParent(transform);
        newInstance.transform.localScale = initialChildScale;
        slotChildInstance = newInstance;
        inventoryUIManager.GetInventory().RemoveItemInPosition(position);
        slotChildInstance.SetActive(false);

    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (item.GetIsNull() || !item.GetHighlightable())
        {
            return;
        }
        inventoryUIManager.GetComponent<InventoryUIManager>().SetHightlighted(gameObject);
    }
    public Image GetSlotImage()
    {
        return slotImage;
    }
    public GameObject GetSlotChildInstance()
    {
        return slotChildInstance;
    }
    public InventoryUIManager GetInventoryUI()
    {
        return inventoryUIManager;
    }
    public Color GetColor()
    {
        return color;
    }
    public InventoryItem GetItem()
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
}
