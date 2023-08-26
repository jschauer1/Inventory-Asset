using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class DragItem : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    // Start is called before the first frame update
    private Slot CurrentSlot;
    private InventoryItem item;
    [SerializeField] TextMeshProUGUI text;

    void Start()
    {
        CurrentSlot = transform.parent.GetComponent<Slot>();
    }

    // Update is called once per frame
    public void OnDrag(PointerEventData eventData)
    {
        if (Draggable()) return;
        Canvas canvas = InventoryController.instance.GetUI().GetComponent<Canvas>();
        transform.parent.gameObject.transform.SetSiblingIndex(100);
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)canvas.transform, eventData.position, canvas.worldCamera, out position);
        transform.position = canvas.transform.TransformPoint(position);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (Draggable()) return;
        if (CurrentSlot != null)
        {
            transform.SetParent(CurrentSlot.GetInventoryUI().GetUI());
            CurrentSlot.ResetSlot();
        }
        else
        {
            Debug.LogWarning("No Slot");
            return;
        }

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (Draggable()) return;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        bool foundSlot = false;
        foreach (RaycastResult result in results)
        {
            if(result.gameObject.tag == "Slot")
            {
                Slot slot = result.gameObject.GetComponent<Slot>();
                if(slot.GetItem().GetIsNull() && slot.GetInventoryUI().GetInventory().CheckAcceptance(item.GetItemType()))
                {
                    InventoryController.instance.AddItem(slot.GetInventoryUI().GetInventoryName(), item, slot.GetPosition());
                    Destroy(gameObject);
                }
                else
                {
                    Return();
                }
                foundSlot = true;
                break;
            }
        }       
        if(!foundSlot)
        {
            Return();
        }
    }
    private void Return()
    {
        InventoryController.instance.AddItem(CurrentSlot.GetInventoryUI().GetInventoryName(), item, CurrentSlot.GetPosition());
        Destroy(gameObject);
    }
    private bool Draggable()
    {
        if(CurrentSlot != null)
        {
            return (!item.GetDraggable() || !CurrentSlot.GetInventoryUI().GetDraggable());
        }
        return false;

    }
    public void SetItem(InventoryItem item)
    {
        this.item = item;
    }
    public void _SetText()
    {
        if (!item.GetIsNull())
        {
            text.SetText(item.GetAmount().ToString());
        }
    }

}
