using System.Collections.Generic;
using UnityEngine;
/*Author: Jaxon Schauer
 * This class uses information from the inventory controller to build a UI interface. 
 */

public class InventoryUIManager : MonoBehaviour
{
    private GameObject previouslyHighlighted;

    [SerializeField]
    private string inventoryName;
    [SerializeField, HideInInspector]
    private Inventory inventory;//Holds a reference to the inventory object that is linked with the inventory UI.
    [SerializeField]
    private int col, row;
    [SerializeField]
    private GameObject slot;//Holds the prefab for the slot. The slot is used to hold the items added to the inventory.
    [SerializeField]
    private GameObject backGround;//Holds the prefab for the background image that will be used behind the slots.
    [SerializeField]
    private Vector2 slotGap;//Defines the space between each slot
    [SerializeField]
    private Vector2 slotOffSet;//Defines a offset between the slots and the background
    [SerializeField]
    private Vector2 backGroundBoarder;//Defines the expansion of the background as needed by the user
    [SerializeField]
    private bool draggable;//Boolean that when pressed, along with the boolean on the items, allows items to be dragged in chosen inventory
    [SerializeField]
    private bool highlightable;//Boolean that when pressed,along with the boolean on the items, allows items to be highlighed in chosen inventory
    [SerializeField]
    private char EnableDisableOnPress;//Defines a button that can be pressed to disable or enable an inventory, This button must be a Char
    [SerializeField]
    List<PressableSlot> highLightOnPress;//List of chars and inventory positions that allows for the highlighting on button press

    Dictionary<string, int> slotPress = new Dictionary<string, int>();//Links the string and the position to highlight on press.

    private Transform UI;




    //Hold previous values to be checked in CheckEditorChange
    private int previousRow = 0, previousCol = 0;
    private float previousWidth = 0, previousHeight = 0;
    private float previousOffSetx = 0, previousOffSety = 0;
    private float previousBorderx = 0, previousBordery = 0;



    [SerializeField, HideInInspector]
    private List<GameObject> slots = new List<GameObject>();
    private List<Vector2> slotsvec = new List<Vector2>();
    private Dictionary<Vector2, GameObject> slotPositionsVec = new Dictionary<Vector2, GameObject>();
    private Dictionary<int, GameObject> slotPos = new Dictionary<int, GameObject>();
    public void Awake()
    {
        createSlots();
    }

    public void Start()
    {
        if (!TestSetup()) return;
        UI = InventoryController.instance.GetUI();
        UpdateInventoryUI();
    }

    /// <summary>
    /// Constantly checks for input to pass to HighLightOnButtonPress
    /// </summary>
    private void Update()
    {
        HighLightOnButtonPress();
    }

    /// <summary>
    /// Checks if any meaningful variables have been changed and if so calls the functions, creating the expected UI
    /// </summary>
    public void UpdateInventoryUI()
    {
        if (CheckEditorChange())
        {
            inventory.Resize(row * col);

            InventoryUIReset();

            createSlots();

            setBackground();
        }
    }
    /// <summary>
    /// Sets all the values to be checked for change in <see cref="CheckEditorChange"/> and resets any values that may cary over after a change
    /// </summary>
    private void InventoryUIReset()
    {
        previousCol = col;
        previousRow = row;
        previousHeight = slotGap.y;
        previousWidth = slotGap.x;
        previousOffSetx = slotOffSet.x;
        previousOffSety = slotOffSet.y;
        if (slot == null) return;
        if (Application.isPlaying)
        {
            foreach (GameObject slot in slots)
            {
                Destroy(slot);
            }
        }
        else
        {
            foreach (GameObject slot in slots)
            {
                DestroyImmediate(slot);
            }
        }
        slots.Clear();
        slotsvec.Clear();
        slotPositionsVec.Clear();
        slotPos.Clear();
        slotPress.Clear();
    }
    /// <summary>
    /// Aligns the background with the slots while account of other user based offsets in relationship to the background vs the slots
    /// </summary>
    private void setBackground()
    {
        if (backGround == null)
        {
            return;
        }
        if (slotsvec.Count <= 0)
        {
            return;
        }
        RectTransform rectTransform = backGround.GetComponent<RectTransform>();
        Vector2 backGroundArea = slotsvec[0] - slotsvec[slotsvec.Count - 1];
        rectTransform.sizeDelta = new Vector2((Mathf.Abs(backGroundArea.x) + backGroundBoarder.x), Mathf.Abs(backGroundArea.y) + backGroundBoarder.y);
        backGround.transform.position = new Vector2(transform.position.x, transform.position.y);
    }
    /// <summary>
    /// Creates all slots on the inventory UI, applying the slot gap and other offsets accordingly
    /// Adds the slots into dictionaries and lists to track them
    /// Uses <see cref="SetSlotOrder"/> 
    /// </summary>
    private void createSlots()
    {
        InventoryUIReset();
        InitSlotPressDict();
        Vector2 inventoryPosVec = transform.position;
        Vector2 placeMentPos = new Vector2(inventoryPosVec.x, inventoryPosVec.y);

        placeMentPos.y = transform.position.y + slotOffSet.y;
        for (int curRow = 0; curRow < row; curRow++)
        {
            placeMentPos.x = transform.position.x + slotOffSet.x;
            for (int curCol = 0; curCol < col; curCol++)
            {
                GameObject slotObjectInstance = Instantiate(slot, placeMentPos, Quaternion.identity, transform);
                placeMentPos.x += slotGap.x;
                slotPositionsVec.Add(new Vector2(curRow, curCol), slotObjectInstance);
                slots.Add(slotObjectInstance);
                slotsvec.Add(slotObjectInstance.transform.localPosition);
            }
            placeMentPos.y -= (slotGap.y);
        }
        SetSlotOrder(new Vector2(19, 0), "Up", "Right");
    }

    /// <summary>
    /// Takes as input a startPosition, and a movement
    /// Gives logical order to inventories
    /// +TODO: Add flexibility
    /// </summary>
    private void SetSlotOrder(Vector2 startPosition, string moveVerticle, string moveHorizontal)
    {
        int rowChange = 0;
        int colChange = 0;
        switch (moveVerticle)
        {
            case "up":
            case "Up":
            case "UP":
                rowChange = -1;
                break;
            case "down":
            case "Down":
            case "DOWN":
                rowChange = 1;
                break;
            default:
                break;
        }
        switch (moveHorizontal)
        {
            case "right":
            case "Right":
            case "RIGHT":
                colChange = 1;
                break;
            case "left":
            case "Left":
            case "LEFT":
                colChange = -1;
                break;
            default:
                break;
        }
        Vector2 offSet = new Vector2(row - 1, 0);
        int currentPosition = 0;
        for (int curRow = 0; curRow < row; curRow++)
        {
            offSet.y = startPosition.y;
            for (int curCol = 0; curCol < col; curCol++)
            {
                GameObject slot = slotPositionsVec[new Vector2((offSet.x), (offSet.y))];
                slot.GetComponent<Slot>().SetPosition(currentPosition);
                slotPos.Add(currentPosition, slot);
                offSet.y += colChange;

                currentPosition++;
            }
            offSet.x += rowChange;

        }
    }
    /// <summary>
    /// Checks if any values have changed, determining if the UI to be updated.
    /// </summary>
    private bool CheckEditorChange()
    {
        if (previousRow != row || previousCol != col || previousWidth != slotGap.x ||
        previousHeight != slotGap.y || previousOffSetx != slotOffSet.x || previousOffSety != slotOffSet.y
        || backGroundBoarder.x != previousBorderx || backGroundBoarder.y != previousBordery)
        {
            return true;
        }
        else
        {
            return false;
        }

    }
    /// <summary>
    /// takes as input a Slot
    /// Highlights said slot and deslects any other currently highlighted slots.
    /// </summary>
    public void SetHightlighted(GameObject slot)
    {
        Slot slotInstance = slot.GetComponent<Slot>();
        Item item = slotInstance.GetItem();
        if (item.GetHighlightable() && highlightable)
        {
            if (previouslyHighlighted != null)
            {
                Slot prevSlotInstance = previouslyHighlighted.GetComponent<Slot>();

                if (previouslyHighlighted == slot)
                {
                    return;
                }
                prevSlotInstance.GetSlotImage().color = prevSlotInstance.GetColor();
            }
            slotInstance.GetSlotImage().color = Color.grey;
            slotInstance.GetItem().Selected();
            previouslyHighlighted = slot;
        }
    }
    /// <summary>
    /// Uses <see cref="SetHightlighted"/>  to highlight a user defined slot on the press of a user defined button.
    /// </summary>
    private void HighLightOnButtonPress()
    {
        if (Input.anyKeyDown)
        {
            string input = Input.inputString;
            if (slotPress.ContainsKey(input))
            {
                int position = slotPress[input];
                SetHightlighted(slotPos[position]);
            }
        }
    }
    /// <summary>
    /// Defines the <see cref="slotPress"/> dictionary, allowing for <see cref="HighLightOnButtonPress"/> to efficiently detect a button click
    /// </summary>
    private void InitSlotPressDict()
    {
        foreach (PressableSlot press in highLightOnPress)
        {
            if (!slotPress.ContainsKey(press.buttonPress.ToString()))
            {
                slotPress.Add(press.buttonPress.ToString(), press.position);

            }
            else
            {
                Debug.LogWarning("Each press position must be unique");
            }
        }
    }
    /// <summary>
    /// Tests that InventoryController is Setup to support the InventoryUIManager, destroying the InventoryUIManager if not
    /// </summary>
    public bool TestSetup()
    {
        if (InventoryController.instance == null)
        {
            Debug.LogError("Inventory Controller Instance Null Destroying Inventory: " + inventoryName);
            Destroy(gameObject);
            return false;
        }
        if (!InventoryController.instance.TestinventoryManagerObjSetup())
        {
            Debug.LogError("Inventory Object Setup Incorrect Destroying Inventory: " + inventoryName);
            Destroy(gameObject);
            return false;
        }
        return true;
    }
    /// <summary>
    /// Connects the button press and position values.
    /// </summary>
    /// 
    [System.Serializable]
    private struct PressableSlot
    {
        public int position;
        public char buttonPress;
    }
    public void SetRowCol(int row, int col)
    {
        this.row = row;
        this.col = col;
        UpdateInventoryUI();
    }
    public void SetInventoryName(string inventoryName)
    {
        this.inventoryName = inventoryName;
    }
    public string GetInventoryName()
    {
        return inventory.GetName();

    }
    public ref Inventory GetInventory()
    {
        return ref inventory;
    }
    public Item GetInventoryItem(int index)
    {
        return inventory.InventoryGetItem(index);
    }
    public void SetInventory(ref Inventory inventory)
    {
        this.inventory = inventory;
    }
    public void UpdateSlot(int location)
    {
        slotPos[location].GetComponent<Slot>().UpdateSlot();
    }
    public Transform GetUI()
    {
        return UI;
    }
    public void SetDraggable(bool draggable)
    {
        this.draggable = draggable;
    }
    public void SetHighlightable(bool highlightable)
    {
        this.highlightable = highlightable;
    }
    public bool GetDraggable()
    {
        return draggable;
    }
    private void OnDestroy()
    {
        inventory = null;
    }

    public void SetEnableDisable(string EnableDisableOnPress)
    {
        this.EnableDisableOnPress = EnableDisableOnPress.ToCharArray()[0];
    }
    public string GetEnableDisable()
    {
        return EnableDisableOnPress.ToString();
    }

}
