using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
//Author: Jaxon Schauer
/// <summary>
/// This class uses information given by the inventory controller to build a UI interface. This interface is linked with the <see cref="Inventory"/> class
/// displaying the items contained inside the associated object
/// </summary>
public class InventoryUIManager : MonoBehaviour
{
    private GameObject previouslyHighlighted;

    // Inventory UI Configuration
    [Header("============[ Inventory UI Setup ]============")]

    [Tooltip("Name of the inventory for identification purposes.")]
    [SerializeField]
    private string inventoryName;

    // This field isn't intended for direct modifications hence hidden.
    [SerializeField, HideInInspector]
    private Inventory inventory; // Reference to the associated inventory object.

    [Tooltip("Number of columns for the inventory layout.")]
    [SerializeField]
    private int col;

    [Tooltip("Number of rows for the inventory layout.")]
    [SerializeField]
    private int row;

    [Tooltip("Prefab representing an individual item slot.")]
    [SerializeField]
    private GameObject slot;

    [Tooltip("Sets the slots image, selected image is not necessary")]
    [SerializeField]
    private slotImages SlotImage;

    [Tooltip("Spacing between individual slots in the inventory.")]
    [SerializeField]
    private Vector2 slotGap;

    [Tooltip("Size dimensions of each slot.")]
    [SerializeField]
    private Vector2 slotSize;

    [Tooltip("Size factor for the item image displayed in a slot. Gets multiplied by the slot size.")]
    [SerializeField]
    private Vector2 ItemImageSizeFactor;

    [SerializeField] private float textSize;

    [SerializeField] private Vector3 textPosition;

    [Header("========[Background Settings]========")]

    [Tooltip("Prefab for the background behind the inventory slots.")]
    [SerializeField]
    private GameObject background;

    [Tooltip("Active and deactivate background")]
    [SerializeField] bool activeBackground;

    [Tooltip("Dimensions for adjusting the background size.")]
    [SerializeField]
    private Vector2 backgroundBoarder;

    [Tooltip("Offset for positioning slots with respect to the background.")]
    [SerializeField]
    private Vector2 slotOffSetToBackground;

    // Item Acceptance Configuration
    [Header("========[ Item Acceptance Configuration ]========")]

    [Tooltip("Specify the general rules for item acceptance in this inventory.")]
    [SerializeField]
    private ItemAcceptance acceptance;

    [Tooltip("Define any exceptions to the general item acceptance rules.")]
    [SerializeField]
    private List<string> exceptions;
    // Additional Configuration Options
    [Header("========[ Additional Options ]========")]

    [Tooltip("Determines the starting point for slot arrangement.")]
    [SerializeField]
    StartPositions slotStartPosition;

    [Tooltip("Enable dragging items within the inventory. NOTE: Must also be enabled in item.")]
    [SerializeField]
    private bool draggable;

    [Tooltip("Allow items to be highlighted when selected. NOTE: Must also be enabled in item.")]
    [SerializeField]
    private bool clickable;
    [Tooltip("Enable saving the state of the inventory.")]
    [SerializeField]
    private bool saveInventory;
    [Tooltip("Invokes item action when item enters inventory")]
    [SerializeField]
    bool clickItemOnEnter;
    [Tooltip("Key that toggles the visibility/enabling of the inventory. Must be a character.")]
    [SerializeField]
    private List<char> toggleOnButtonPress;
    [Tooltip("Map between keys and inventory slots for slot highlighting.")]
    [SerializeField]
    List<PressableSlot> highLightSlotOnPress;
    [SerializeField] 
    List<invokeOnExit> itemEntryExitAction;


    Dictionary<string, int> slotPress = new Dictionary<string, int>();//Links the string and the position to highlight on press.

    private Transform UI;

    private RectTransform rectTransform;

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
        SlotImage.regular = slot.GetComponent<Image>().sprite;
        slotSize = slot.GetComponent<RectTransform>().sizeDelta;
        slotGap = slotSize;
        GameObject slotChild = slot.GetComponent<Slot>().GetSlotChildInstance();
        RectTransform slotChildRect = slotChild.GetComponent<RectTransform>();
        ItemImageSizeFactor = new Vector2(slotChildRect.sizeDelta.x/slot.GetComponent<RectTransform>().sizeDelta.x, slotChildRect.sizeDelta.y/slot.GetComponent<RectTransform>().sizeDelta.y);
        textSize = slot.GetComponent<Slot>().GetSlotChildInstance().GetComponent<DragItem>().GetTextSize();
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
            SetBackground();
            UpdateInventory();
            InitSlotEnterExitDict();
            if (background!=null && !activeBackground)
            {
                background.SetActive(false);
            }
            else if(background!=null && activeBackground)
            {
                background.SetActive(true);
            }
            inventory.SetclickItemOnEnter(clickItemOnEnter);

        }
    }
    /// <summary>
    /// Sets all the values to be checked for change in <see cref="CheckEditorChange"/> and resets any values that may cary over after a change
    /// </summary>
    private void InventoryUIReset()
    {
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
    private void SetBackground()
    {
        if (background == null)
        {
            return;
        }
        if (slotsvec.Count <= 0)
        {
            return;
        }
        RectTransform rectTransform = background.GetComponent<RectTransform>();
        Vector2 backGroundArea = slotsvec[0] - slotsvec[slotsvec.Count - 1];
        rectTransform.sizeDelta = new Vector2((Mathf.Abs(backGroundArea.x) + backgroundBoarder.x), Mathf.Abs(backGroundArea.y) + backgroundBoarder.y);
        background.transform.position = new Vector2(transform.position.x + slotOffSetToBackground.x, transform.position.y+slotOffSetToBackground.y);
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
                slotObjectInstance.GetComponent<Image>().sprite = SlotImage.regular;
                slotObjectInstance.GetComponent<Slot>().SetChildImageSize(new Vector2(slotSize.x * ItemImageSizeFactor.x, slotSize.y * ItemImageSizeFactor.y));
                slotObjectInstance.GetComponent<Slot>().SetTextSize(textSize);
                slotObjectInstance.GetComponent<Slot>().SetTextOffset(textPosition);

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
    public void SetSelected(GameObject slot)
    {
        Slot slotInstance = slot.GetComponent<Slot>();
        InventoryItem item = slotInstance.GetItem();
        if (item.GetHighlightable() && clickable || item.GetIsNull() && clickable)
        {
            if (previouslyHighlighted != null)
            {
                Slot prevSlotInstance = previouslyHighlighted.GetComponent<Slot>();

                if (previouslyHighlighted == slot)
                {
                    slotInstance.GetItem().Selected();
                    return;
                }
                previouslyHighlighted.GetComponent<Image>().sprite = SlotImage.regular;
                prevSlotInstance.GetSlotImage().color = prevSlotInstance.GetColor();
            }
            if (SlotImage.selected == null)
            {
                slotInstance.GetSlotImage().color = Color.grey;

            }
            else
            {
                slotInstance.GetComponent<Image>().sprite = SlotImage.selected;

            }
            slotInstance.GetItem().Selected();
            previouslyHighlighted = slot;
        }
    }
    /// <summary>
    /// Uses <see cref="SetSelected"/>  to highlight a user defined slot on the press of a user defined button.
    /// </summary>
    private void HighLightOnButtonPress()
    {
        if (Input.anyKeyDown)
        {
            string input = Input.inputString;
            if (slotPress.ContainsKey(input))
            {
                int position = slotPress[input];
                SetSelected(slotPos[position]);
            }
        }
    }
    /// <summary>
    /// Defines the <see cref="slotPress"/> dictionary, allowing for <see cref="HighLightOnButtonPress"/> to efficiently detect a button click
    /// </summary>
    private void InitSlotPressDict()
    {
        foreach (PressableSlot press in highLightSlotOnPress)
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
    private void InitSlotEnterExitDict()
    {

        if(itemEntryExitAction.Count != 0)
        {
            Dictionary<int, UnityEvent> enter = new Dictionary<int, UnityEvent>();
            Dictionary<int, UnityEvent> exit = new Dictionary<int, UnityEvent>();
            Dictionary<int, bool>actItem = new Dictionary<int, bool>(); 
            foreach(invokeOnExit enterExit in itemEntryExitAction)
            {
                if(enterExit.enterExit == EnterExit.Exit)
                {
                    if(!exit.ContainsKey(enterExit.slotpos))
                    {
                        exit.Add(enterExit.slotpos, enterExit.action);
                       // actItem.Add(enterExit.slotpos, enterExit.ItemActOnEnter);
                        inventory.SetExitEntranceDict(enter, exit, actItem);
                    }
                    else
                    {
                        Debug.LogError("Each slot can only have one Exit action, ignoring: " + enterExit.slotpos);
                    }
                }
                else
                {
                    if (!enter.ContainsKey(enterExit.slotpos))
                    {
                        enter.Add(enterExit.slotpos, enterExit.action);
                        actItem.Add(enterExit.slotpos, enterExit.ItemActOnEnter);
                        inventory.SetExitEntranceDict(enter, exit, actItem);

                    }
                    else
                    {
                        Debug.LogError("Each slot can only have one Enter action, ignoring: "+ enterExit.slotpos);
                    }
                }
            }

        }
    }
    /// <summary>
    /// Tests that InventoryController is Setup to support the InventoryUIManager, destroying the InventoryUIManager if not
    /// </summary>
    public bool TestSetup()
    {
        if (!InventoryController.instance.checkUI(gameObject))
        {
            Debug.LogError("inventoryUIDict does not contain object, this can be caused by not initializing through inventory controller. Destroying: " + gameObject.name);
            Destroy(gameObject);
            return false;
        }
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
    [System.Serializable]
    private struct slotImages
    {
        [Tooltip("Displays regular slot image")]
        public Sprite regular;
        [Tooltip("OPTIONAL: Displays slot Image when selected")]
        public Sprite selected;
    }
    [System.Serializable]
    private struct invokeOnExit
    {
        public EnterExit enterExit;

        public int slotpos;

        public UnityEvent action;

        public bool ItemActOnEnter;
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
        saveInventory = save;
    }
    public void SetSaveInventory()
    {
        inventory.SetSave(saveInventory);
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
        this.clickable = highlightable;
    }
    public bool GetDraggable()
    {
        return draggable;
    }
    private void OnDestroy()
    {
        inventory = null;
    }

    public void SetInvToggle(List<char> EnableDisableOnPress)
    {
        this.toggleOnButtonPress = EnableDisableOnPress;
    }
    public List<char> GetEnableDisable()
    {
        return toggleOnButtonPress;
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
    private enum EnterExit
    {
        Enter,
        Exit
    }

}
