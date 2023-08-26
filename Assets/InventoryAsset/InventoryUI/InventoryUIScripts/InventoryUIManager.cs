using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
/*Author: Jaxon Schauer
 * 
 */
/// <summary>
/// This class uses information given by the inventory controller to build a UI interface. This interface is linked with the <see cref="Inventory"/> class
/// displaying the items contained inside the associated object
/// </summary>
public class InventoryUIManager : MonoBehaviour
{
    private GameObject previouslyHighlighted;
    [Header("Inventory UI Setup")]
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
    private Vector2 slotSize;//Defines the space between each slot
    [SerializeField]
    private Vector2 slotImageSizeOffSet;//Defines the space between each slot
    [SerializeField]
    private Vector2 slotOffSet;//Defines a offset between the slots and the background
    [SerializeField]
    private Vector2 backGroundBoarder;//Defines the expansion of the background as needed by the user
    [Header("Start Point")]
    [SerializeField] 
    StartPositions slotStartPosition;
    [Header("Extra Options:")]
    [SerializeField]
    private bool draggable;//Boolean that when pressed, along with the boolean on the items, allows items to be dragged in chosen inventory
    [SerializeField]
    private bool highlightable;//Boolean that when pressed,along with the boolean on the items, allows items to be highlighed in chosen inventory
    [SerializeField] 
    private bool saveable;
    [SerializeField]
    private char EnableDisableOnPress;//Defines a button that can be pressed to disable or enable an inventory, This button must be a Char
    [SerializeField]
    List<PressableSlot> highLightOnPress;//List of chars and inventory positions that allows for the highlighting on button press
    [Header("Accept/Reject Items: ")]
    [SerializeField]
    private ItemAcceptance acceptance;//Takes input from the user on what level of item acceptance they want
    [SerializeField]
    private List<string> exceptions;//exceptions for the above level of acceptance, EX: An armor piece may be the only thing you want in an inventory, that is an exception


    Dictionary<string, int> slotPress = new Dictionary<string, int>();//Links the string and the position to highlight on press.

    private Transform UI;

    private RectTransform rectTransform;


    //Hold previous values to be checked in CheckEditorChange
    private int previousRow = 0, previousCol = 0, previousExceptionsSize=0;
    private float previousWidth = 0, previousHeight = 0;
    private float previousOffSetx = 0, previousOffSety = 0;
    private float previousBorderx = 0, previousBordery = 0;
    private StartPositions prevSlotPos;


    //Holds and organizes slots
    [SerializeField, HideInInspector]
    private List<GameObject> slots = new List<GameObject>();
    private List<Vector2> slotsvec = new List<Vector2>();
    private Dictionary<Vector2, GameObject> slotPositionsVec = new Dictionary<Vector2, GameObject>();
    private Dictionary<int, GameObject> slotPos = new Dictionary<int, GameObject>();
    public void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        UpdateInventoryUI(true);
    }

    public void Start()
    {
        if (!TestSetup()) return;
        UI = InventoryController.instance.GetUI();

    }

    /// <summary>
    /// Constantly checks for input to pass to HighLightOnButtonPress
    /// </summary>
    private void Update()
    {
        HighLightOnButtonPress();
    }
    /// <summary>
    /// Called only when Initialize Inventories is pressed
    /// </summary>
    public void SetVarsOnInit()
    {
        slotSize = slot.GetComponent<RectTransform>().sizeDelta;
        slotGap = slotSize;
        GameObject slotChild = slot.GetComponent<Slot>().GetSlotChildInstance();
        RectTransform slotChildRect = slotChild.GetComponent<RectTransform>();
        slotImageSizeOffSet = new Vector2(slotChildRect.sizeDelta.x/slot.GetComponent<RectTransform>().sizeDelta.x, slotChildRect.sizeDelta.y/slot.GetComponent<RectTransform>().sizeDelta.y);

    }
    /// <summary>
    /// Checks if any meaningful variables have been changed and if so calls the functions, creating the expected UI
    /// </summary>
    public void UpdateInventoryUI(bool isOverride = false)
    {

        if (CheckEditorChange() || isOverride)
        {
            inventory.Init();

            SetSaveInventory();
            inventory.Resize(row * col);

            InventoryUIReset();

            createSlots();
            SetSlotPositions();
            setBackground();
            UpdateInventory();

        }
    }
    /// <summary>
    /// Sets all the values to be checked for change in <see cref="CheckEditorChange"/> and resets any values that may cary over after a change
    /// </summary>
    private void InventoryUIReset()
    {
        previousExceptionsSize = exceptions.Count;
        previousCol = col;
        previousRow = row;
        previousHeight = slotGap.y;
        previousWidth = slotGap.x;
        previousOffSetx = slotOffSet.x;
        previousOffSety = slotOffSet.y;
        prevSlotPos = slotStartPosition;
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
    /// Checks if any values have changed, determining if the UI needs to be updated.
    /// </summary>
    private bool CheckEditorChange()
    {
        return true;
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
        rectTransform = GetComponent<RectTransform>();

        // Start from the top-left corner of the InventoryUI
        Vector2 startPlacementPos = rectTransform.right;

        for (int curRow = 0; curRow < row; curRow++)
        {
            for (int curCol = 0; curCol < col; curCol++)
            {
                // Calculate the x and y position for each slot using the original slotGap and slotOffset values
                float placeMentPosX = startPlacementPos.x + (curCol * slotGap.x);
                float placeMentPosY = startPlacementPos.y - (curRow * slotGap.y);

                Vector2 placeMentPos = new Vector2(placeMentPosX, placeMentPosY);

                GameObject slotObjectInstance = Instantiate(slot, rectTransform);
                slotObjectInstance.GetComponent<RectTransform>().localPosition = placeMentPos;
                slotObjectInstance.GetComponent<RectTransform>().sizeDelta = slotSize;
                slotObjectInstance.GetComponent<Slot>().GetSlotChildInstance().GetComponent<RectTransform>().sizeDelta = new Vector2(slotSize.x * slotImageSizeOffSet.x, slotSize.y * slotImageSizeOffSet.y);

                slotPositionsVec.Add(new Vector2(curRow, curCol), slotObjectInstance);
                slots.Add(slotObjectInstance);
                slotsvec.Add(placeMentPos);
            }
        }
    }
    /// <summary>
    /// Checks chosen StartPosition and uses <see cref="SetSlotOrder"/> to fill order each slot, counting in a understandable fashion
    /// </summary>
    public void SetSlotPositions()
    {
        switch (slotStartPosition)
        {
            case StartPositions.TopRight:
                SetSlotOrder(new Vector2(0, col-1), "down", "left");
                break;
            case StartPositions.TopLeft:
                SetSlotOrder(new Vector2(0, 0), "down", "right");
                break;
            case StartPositions.BottomLeft:
                SetSlotOrder(new Vector2(row - 1, 0), "up", "right");
                break;
            case StartPositions.BottomRight:
                SetSlotOrder(new Vector2(row - 1, col - 1), "up", "left");
                break;
            default:
                SetSlotOrder(new Vector2(row-1, 0), "up", "right");
                break;
        }

    }
    /// <summary>
    /// Loads information into <see cref="Inventory"/> class about what items to accept or reject
    /// </summary>
    private void UpdateInventory()
   {
        if(acceptance == ItemAcceptance.AcceptAll)
        {
            inventory.SetupItemAcceptance(true, false, exceptions);
        }
        else
        {
            inventory.SetupItemAcceptance(false, true, exceptions);
        }
    }

    /// <summary>
    /// Takes as input a startPosition, and a movement
    /// Gives logical order to inventories
    /// </summary>
    private void SetSlotOrder(Vector2 startPosition, string moveVerticle, string moveHorizontal)
    {
        if (slotPos == null)
             Debug.LogError("SlotPos Null");


        if (slotPositionsVec == null)
            Debug.LogError("SlotPositionVec Null");

        slotPos.Clear();

        int rowChange = 0;
        int colChange = 0;

        switch (moveVerticle.ToLower())
        {
            case "up":
                rowChange = -1;
                break;
            case "down":
                rowChange = 1;
                break;
        }

        switch (moveHorizontal.ToLower())
        {
            case "right":
                colChange = 1;
                break;
            case "left":
                colChange = -1;
                break;
        }

        Vector2 currentPos = startPosition;
        int currentPosition = 0;

        for (int curRow = 0; curRow < row; curRow++)
        {
            for (int curCol = 0; curCol < col; curCol++)
            {
                if (slotPositionsVec.ContainsKey(currentPos))
                {
                    GameObject slot = slotPositionsVec[currentPos];
                    slot.GetComponent<Slot>().SetPosition(currentPosition);
                    slotPos[currentPosition] = slot;
                    currentPosition++;
                }
                currentPos += new Vector2(0, colChange);  // Move horizontally first
            }
            currentPos = new Vector2(currentPos.x + rowChange, startPosition.y); // Reset the horizontal position and move vertically
        }
    }
    /// <summary>
    /// takes as input a Slot
    /// Highlights said slot and deslects any other currently highlighted slots.
    /// </summary>
    public void SetHightlighted(GameObject slot)
    {
        Slot slotInstance = slot.GetComponent<Slot>();
        InventoryItem item = slotInstance.GetItem();
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
            Debug.LogError("Inventory Controller Instance Null. Destroying Inventory: " + inventoryName);
            Destroy(gameObject);
            return false;
        }
        if (!InventoryController.instance.TestinventoryManagerObjSetup())
        {
            Debug.LogError("Inventory Object Setup Incorrect. Destroying Inventory: " + inventoryName);
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
    /// <summary>
    /// Returns the item at a given inventory position
    /// </summary>
    public InventoryItem GetInventoryItem(int index)
    {
        return inventory.InventoryGetItem(index);
    }
    public void SetSave(bool save)
    {
        saveable = save;
    }
    public void SetSaveInventory()
    {
        inventory.SetSave(saveable);
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
    public void SetInventory(ref Inventory inventory)
    {
        this.inventory = inventory;
    }
    public void UpdateSlot(int location)
    {
        if(slotPos.ContainsKey(location))
        {
            slotPos[location].GetComponent<Slot>().UpdateSlot();

        }
        else
        {
            Debug.LogError("Dictionary does not contain slot at: "  + location);
        }
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
    private enum StartPositions
    {
        BottomLeft,
        TopLeft,
        TopRight,
        BottomRight,
    }
    private enum ItemAcceptance
    {
        AcceptAll,
        RejectAll
    }

}
