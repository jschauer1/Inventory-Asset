using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
//Author: Jaxon Schauer
/// <summary>
/// Controls the dragging of an item.
/// </summary>
internal class DragItem : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    /// The current slot associated with the drag item
    private Slot CurrentSlot;

    /// The inventory item being dragged
    private InventoryItem item;

    /// The text UI element for displaying item information
    [SerializeField] 
    private TextMeshProUGUI text;

    /// The text UI element for displaying item information
    GameObject prevslot;
    /// Initializes the CurrentSlot on start
    private void Start()
    {
        prevslot = null;

        CurrentSlot = transform.parent.GetComponent<Slot>();
    }

    /// <summary>
    /// Handles the drag event
    /// </summary>
    public void OnDrag(PointerEventData eventData)
    {
        if (Draggable()) return;
        Canvas canvas = InventoryController.instance.GetUI().GetComponent<Canvas>();
        transform.parent.gameObject.transform.SetSiblingIndex(100);
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)canvas.transform, eventData.position, canvas.worldCamera, out position);
        transform.position = canvas.transform.TransformPoint(position);

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        bool foundSlot = false;

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.CompareTag("Slot"))
            {
                prevslot = result.gameObject;
                Slot slot = result.gameObject.GetComponent<Slot>();
                if(slot.GetItem().GetIsNull() && slot.GetInventoryUI().GetInventory().CheckAcceptance(item.GetItemType()))
                {
                    slot.GetInventoryUI().Highlight(result.gameObject);
                    foundSlot = true;
                }

                break;
            }
        }

        if (!foundSlot)
        {
            if(prevslot != null)
            {
                prevslot.GetComponent<Slot>().GetInventoryUI().UnHighlight(prevslot);
                prevslot.GetComponent<Slot>().GetInventoryUI().ResetHighlight();


            }

        }
    }

    /// <summary>
    /// Handles the beginning of the drag event
    /// </summary>
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (Draggable()) return;
        if (CurrentSlot != null)
        {
            CurrentSlot.ResetSlot();
            transform.SetParent(CurrentSlot.GetInventoryUI().GetUI());
        }
        else
        {
            Debug.LogWarning("No Slot");
        }
    }

    /// <summary>
    /// Handles the end of the drag event
    /// </summary>
    public void OnEndDrag(PointerEventData eventData)
    {
        if (Draggable()) return;

        HandleEndDrag(eventData);
    }

    /// <summary>
    /// Processes the end of the drag event and checks for valid drop targets.
    /// </summary>
    private void HandleEndDrag(PointerEventData eventData)
    {
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        bool foundSlot = false;

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.CompareTag("Slot"))
            {
                HandleSlot(result);
                foundSlot = true;
                break;
            }
        }

        if (!foundSlot)
        {
            ReturnToOriginalPosition();
        }
    }

    /// <summary>
    /// Processes the slot result after dragging
    /// </summary>
    private void HandleSlot(RaycastResult result)
    {
        Slot slot = result.gameObject.GetComponent<Slot>();
        if(slot.GetInventoryUI())
        if ((slot.GetItem().GetIsNull()
           || (!slot.GetItem().GetIsNull()) && (slot.GetItem().GetItemType() == item.GetItemType()) && (slot.GetItem().GetAmount() + item.GetAmount()) < slot.GetItem().GetItemStackAmount())
           &&slot.GetInventoryUI().GetInventory().CheckAcceptance(item.GetItemType()))
        {
            InventoryController.instance.AddItemPos(slot.GetInventoryUI().GetInventoryName(), item, slot.GetPosition());
            slot.GetInventoryUI().UnHighlight(result.gameObject);
            prevslot.GetComponent<Slot>().GetInventoryUI().ResetHighlight();
            Destroy(gameObject);
        }
        else
        {
            ReturnToOriginalPosition();
        }
    }

    /// <summary>
    /// Returns the item to its original position if not placed in a valid slot
    /// </summary>
    private void ReturnToOriginalPosition()
    {
        InventoryController.instance.AddItemPos(CurrentSlot.GetInventoryUI().GetInventoryName(), item, CurrentSlot.GetPosition());
        CurrentSlot.GetInventoryUI().UnHighlight(CurrentSlot.gameObject);
        Destroy(gameObject);
    }

    /// <summary>
    /// Checks if the item is draggable
    /// </summary>
    private bool Draggable()
    {
        return CurrentSlot != null && (!item.GetDraggable() || !CurrentSlot.GetInventoryUI().GetDraggable());
    }

    /// <summary>
    /// Sets the inventory item for the drag item
    /// </summary>
    public void SetItem(InventoryItem newItem)
    {
        item = newItem;
    }

    /// <summary>
    /// Updates the text UI based on the item's properties
    /// </summary>
    public void SetText()
    {
        if (!item.GetIsNull())
        {
            text.gameObject.SetActive(item.GetDisplayAmount());
            if (item.GetDisplayAmount())
            {
                text.SetText(item.GetAmount().ToString());
            }
        }
    }

    public void SetTextTestImage(int amount)
    {
        text.SetText(amount.ToString());
    }

    /// <summary>
    /// Sets the offset for the text position
    /// </summary>
    public void SetTextPositionOffset(Vector3 offset)
    {
        text.gameObject.transform.position += offset;
    }

    /// <summary>
    /// Sets the font size for the text UI
    /// </summary>
    public void SetTextSize(float size)
    {
        text.fontSize = size;
    }

    public void SetImageSize(Vector2 size)
    {
        RectTransform imageRect = GetComponent<RectTransform>();
        imageRect.sizeDelta = size;
    }

    public void SetImage(Sprite image)
    {
        GetComponent<Image>().sprite = image;
    }
    public float GetTextSize()
    {
        return text.fontSize;
    }

    public Vector2 GetTextPosition()
    {
        return text.transform.localPosition;
    }

    public void SetTextPosition(Vector2 textposition)
    {
        text.transform.localPosition = textposition;
    }
}
