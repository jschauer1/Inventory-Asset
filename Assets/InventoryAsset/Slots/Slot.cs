using System.Drawing;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/*Author: Jaxon Schauer
 * This class creates a slot gameObject that displays an image of the item when notified by the assigned inventory
 */
internal class Slot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private int position;//The position if the inventories items list
    [SerializeField]
    private GameObject slotChildPrefab;//This holds the prefab for the image allowing it to be instantiated when the child object is dragged to a new location
    [SerializeField]
    private GameObject slotChildInstance;//This is a child object that is used to display an image of the object

    private InventoryItem item;//This is the current item in the inventory, there is always an item however item.GetIsNull() determines if the object contains a real item
    private UnityEngine.Color color;//This is the color of the slot
    private Image slotImage;//This is the image of the slot
    private InventoryUIManager inventoryUIManager;
    private Vector3 initialChildScale;//holds the scale for the slot child to allow for it to be instantiated with the correct size
    private Vector3 initialSlotChildPosition;//This holds the position of the slot child so it can be instantiated with the correct location
    float textSize;
    Vector2 textOffset;
    Vector2 slotChildImageSize;
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
        if (item != null)
        {
            if (!item.GetIsNull())
            {
                slotChildInstance.SetActive(true);
                slotChildInstance.GetComponent<DragItem>().SetItem(item);
                slotChildInstance.GetComponent<Image>().sprite = item.GetItemImage();
                slotChildInstance.GetComponent<DragItem>().SetText();
            }
            else
            {
                slotChildInstance.SetActive(false);

            }
        }
        else
        {
            Debug.LogError("Item is null");
        }

    }
    /// <summary>
    /// Adds a new slotchild when slot child is dragged away, and resets the slot to empty
    /// </summary>
    public void ResetSlot()
    {
        GameObject newInstance = Instantiate(slotChildPrefab, initialSlotChildPosition, Quaternion.identity);
        newInstance.transform.SetParent(transform);
        newInstance.transform.localScale = initialChildScale;
        Vector2 before = slotChildInstance.GetComponent<DragItem>().GetTextPosition();
        slotChildInstance = newInstance;
        SetChildImageSize(slotChildImageSize);
        slotChildInstance.GetComponent<DragItem>().SetTextPosition(before);
        inventoryUIManager.GetInventory().RemoveItemInPosition(position);
        SetTextSize(textSize);
        slotChildInstance.SetActive(false);


    }
    public void OnPointerClick(PointerEventData eventData)
    {
        inventoryUIManager.GetComponent<InventoryUIManager>().SetSelected(gameObject);
    }
    public void SetTextSize(float size)
    {
        textSize = size;
        if(slotChildInstance != null)
        {
            slotChildInstance.GetComponent<DragItem>().SetTextSize(size);

        }
        else
        {
            Debug.LogError("Slot Child Null");
        }
    }
    public void SetTextOffset(Vector3 offset)
    {
        textOffset = offset;
        if (slotChildInstance != null)
        {
            slotChildInstance.GetComponent<DragItem>().SetTextPositionOffset(offset);

        }
        else
        {
            Debug.LogError("Slot Child Null");

        }
    }
    public void SetChildImageSize(Vector2 size)
    {
        slotChildInstance.GetComponent<DragItem>().SetImageSize(size);
        slotChildImageSize = size;
    }
    public float GetTextSize()
    {
        return slotChildInstance.GetComponent<DragItem>().GetTextSize();
    }
    public Image GetSlotImage()
    {
        return slotImage;
    }
    public void SetSlotImage(Image newImage)
    {
        slotImage = newImage;
    }
    public GameObject GetSlotChildInstance()
    {
        return slotChildInstance;
    }
    public InventoryUIManager GetInventoryUI()
    {
        return inventoryUIManager;
    }
    public UnityEngine.Color GetColor()
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
