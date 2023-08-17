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
    private Image image;
    private Slot CurSlot;
    private Item item;
    [SerializeField] TextMeshProUGUI text;

    void Start()
    {
        CurSlot = transform.parent.GetComponent<Slot>();
    }

    // Update is called once per frame
    public void OnDrag(PointerEventData eventData)
    {
        if (draggable()) return;
        Canvas canvas = GameObject.Find("UI").GetComponent<Canvas>();
        transform.parent.gameObject.transform.SetSiblingIndex(100);
        PointerEventData pointerData = eventData;
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)canvas.transform, pointerData.position, canvas.worldCamera, out position);
        transform.position = canvas.transform.TransformPoint(position);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (draggable()) return;
        if (CurSlot != null)
        {
            transform.SetParent(CurSlot.GetInventoryUI().GetUI());
            CurSlot.ResetSlotChild();
            CurSlot = null;

        }
        else
        {
            Debug.Log("No Slot");
        }

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (draggable()) return;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (RaycastResult result in results)
        {
            if(result.gameObject.tag == "Slot")
            {
                Slot slot = result.gameObject.GetComponent<Slot>();
                InventoryController.instance.AddItem(slot.GetInventoryUI().GetInventoryName(), item, slot.GetPosition()); 
                Destroy(gameObject);
                break;
            }
        }        
    }
    private bool draggable()
    {
        if(CurSlot != null)
        {
            return (!item.GetDraggable() || !CurSlot.GetInventoryUI().GetDraggable());
        }
        return false;

    }
    public void SetItem(Item item)
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
