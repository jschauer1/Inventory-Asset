using UnityEngine;
using UnityEngine.UI;

public class chest : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private RectTransform inventoryRectTransform; // Changed from GameObject to RectTransform
    [SerializeField] private float distance;

    private Camera mainCamera;
    private Canvas canvas; // The canvas that the inventory is a child of

    private void Start()
    {
        mainCamera = Camera.main;

        // Assuming the parent of the inventory is the canvas
        canvas = inventoryRectTransform.GetComponentInParent<Canvas>();
    }

    private void Update()
    {
        Vector2 screenPoint = mainCamera.WorldToScreenPoint(transform.position);

        // Convert screen point to canvas space
        Vector2 canvasPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), screenPoint, canvas.worldCamera, out canvasPoint);

        inventoryRectTransform.anchoredPosition = canvasPoint; // Use anchoredPosition for UI elements

        if ((player.transform.position - transform.position).magnitude < distance)
        {
            InventoryController.instance.AddToggleKey("Chest", 'e');
        }
        else
        {
            InventoryController.instance.RemoveToggleKey("Chest", 'e');
        }
    }
}
