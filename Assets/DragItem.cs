using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class DragItem : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    // Start is called before the first frame update
    private Image image;
    private Slot CurSlot;

    void Awake()
    {
        CurSlot = transform.parent.GetComponent<Slot>();
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnDrag(PointerEventData eventData)
    {
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
      //  throw new System.NotImplementedException();
    }
}
