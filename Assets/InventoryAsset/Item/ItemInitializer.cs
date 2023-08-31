using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ItemInitializer
{
    private int amount = 1;

    // Essential Item Properties
    [Header("========[ Item Properties ]========")]

    [Tooltip("Type or classification of the item.")]
    [SerializeField]
    private string itemType;

    [Tooltip("Visual representation of the item.")]
    [SerializeField]
    private Sprite itemImage;

    [Tooltip("Maximum number of this item that can be stacked together.")]
    [SerializeField]
    private int maxStackAmount;

    // This event is likely linked to item-specific behavior and isn't meant for direct modification.
    [Tooltip("Event triggered in relation to this item.")]
    [SerializeField, HideInInspector]
    private InventoryItemEvent myEvent;

    [Tooltip("Display the quantity/amount of the item on its icon.")]
    [SerializeField]
    private bool displayItemAmount;
    // Optional Item Configurations
    [Header("========[ Optional Configurations ]========")]

    [Tooltip("Game object that may be linked or affected by the item.")]
    [SerializeField]
    private GameObject GameObjectAction;

    [Tooltip("Determines if the item can be dragged within the inventory or UI.")]
    [SerializeField]
    private bool draggable;

    [Tooltip("Determines if the item can be highlighted when selected.")]
    [SerializeField]
    private bool highlightable;

    [SerializeField, HideInInspector]
    private bool isNull = false;

    public ItemInitializer(bool isNull)
    {
        this.isNull = isNull;
    }
    public void SetIsNull(bool isNull)
    {
        this.isNull = isNull;
    }
    public bool GetIsNull()
    {
        return isNull;
    }
    public string GetItemType()
    {
        return itemType;
    }
    public Sprite GetItemImage()
    {
        return itemImage;
    }
    public int GetItemStackAmount()
    {
        return maxStackAmount;
    }
    public bool GetHighlightable()
    {
        return highlightable;
    }
    public bool GetDraggable()
    {
        return draggable;
    }
    public int GetAmount()
    {
        return amount;
    }
    public void SetAmount(int amount)
    {
        this.amount = amount;
    }
    public InventoryItemEvent GetEvent()
    {
        return myEvent;
    }
    public GameObject GetGameObjectAction()
    {
        return GameObjectAction;
    }
    public bool GetDisplayAmount()
    {
        return displayItemAmount;
    }
}
