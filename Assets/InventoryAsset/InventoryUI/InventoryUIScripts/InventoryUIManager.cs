using System.Collections.Generic;
using UnityEngine;


public class InventoryUIManager : MonoBehaviour
{
    private GameObject previouslyHighlighted;

    [SerializeField] private string inventoryName;
    [SerializeField] private int col, row;
    [SerializeField] private GameObject slot;
    [SerializeField] private GameObject inventoryPosition;

    [SerializeField] private GameObject backGround;
    [SerializeField] private Vector2 slotGap;
    [SerializeField] private Vector2 slotOffSet;
    [SerializeField] private Vector2 backGroundBoarder;
    [SerializeField] private bool draggable;
    [SerializeField] private bool highlightable;
    [SerializeField] private char EnableDisableOnPress;
    [SerializeField] List<PressableSlot> highLightOnPress;
    Dictionary<string, int> slotPress = new Dictionary<string, int>();

    [System.Serializable]
    private struct PressableSlot
    {
        public int position;
        public char buttonPress;
    }


    Transform UI;

    [SerializeField, HideInInspector]
    private Inventory inventory;



    private int previousRow = 0, previousCol = 0;
    private float previousWidth = 0, previousHeight = 0;
    private float previousOffSetx = 0, previousOffSety = 0;
    private float previousBorderx = 0, previousBordery = 0;


    private GameObject slotObjectInstance;

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
        if (!InventoryController.instance.CheckSetup())
        {
            Destroy(gameObject);
            return;
        }
        UI = InventoryController.instance.GetUI();
        UpdateInventoryDisplay();
    }
    private void Update()
    {
        HighLightOnButtonPress();
    }
    public void UpdateInventoryDisplay()
    {
        if (CheckEditorChange())
        {
            inventory.Resize(row*col);

            InventoryUIReset();

            createSlots();

            setBackground();
        }
    }
    private void InventoryUIReset()
    {
        previousCol = col;
        previousRow = row;
        previousHeight = slotGap.y;
        previousWidth = slotGap.x;
        previousOffSetx = slotOffSet.x;
        previousOffSety = slotOffSet.y;
        if (slot == null) return;
        if(Application.isPlaying)
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
    private void setBackground()
    {
        if(backGround == null)
        {
            return;
        }
        if (slotsvec.Count <= 0)
        {
            return;
        }
        RectTransform rectTransform = backGround.GetComponent<RectTransform>();
        Vector2 backGroundArea = slotsvec[0] - slotsvec[slotsvec.Count - 1];
        rectTransform.sizeDelta = new Vector2((Mathf.Abs(backGroundArea.x) +backGroundBoarder.x), Mathf.Abs(backGroundArea.y) + backGroundBoarder.y);
        backGround.transform.position = new Vector2(inventoryPosition.transform.position.x, inventoryPosition.transform.position.y);
    }
    private void createSlots()
    {
        InventoryUIReset();
        InitPressableDict();
        Vector2 inventoryPosVec = inventoryPosition.transform.position;
        Vector2 placeMentPos = new Vector2(inventoryPosVec.x,inventoryPosVec.y);

        placeMentPos.y = inventoryPosition.transform.position.y + slotOffSet.y;
        for (int curRow = 0; curRow < row; curRow++)
        {
            placeMentPos.x = inventoryPosition.transform.position.x + slotOffSet.x;
            for (int curCol = 0; curCol < col; curCol++)
            {
                slotObjectInstance = Instantiate(slot, placeMentPos, Quaternion.identity, transform);
                placeMentPos.x += slotGap.x;
                slotPositionsVec.Add(new Vector2(curRow, curCol), slotObjectInstance);
                slots.Add(slotObjectInstance);
                slotsvec.Add(slotObjectInstance.transform.localPosition);
            }
            placeMentPos.y -= (slotGap.y);
        }
        SetSlotStart(new Vector2(19, 0), "Up", "Right");
    }


    private void SetSlotStart(Vector2 startPosition, string moveVerticle, string moveHorizontal)
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
        Vector2 offSet = new Vector2(row-1, 0);
        int currentPosition = 0;
        for (int curRow = 0; curRow < row; curRow++)
        {
            offSet.y = startPosition.y;
            for (int curCol = 0; curCol < col; curCol++)
            {
                GameObject slot = slotPositionsVec[new Vector2((offSet.x), (offSet.y))];
                slot.GetComponent<Slot>().SetPosition(currentPosition);
                slotPos.Add(currentPosition, slot);
                offSet.y+= colChange;

                currentPosition++;
            }
            offSet.x += rowChange;

        }
    }
    private bool CheckEditorChange()
    {
        if(previousRow != row || previousCol != col || previousWidth != slotGap.x || 
        previousHeight != slotGap.y || previousOffSetx != slotOffSet.x || previousOffSety != slotOffSet.y
        || backGroundBoarder.x!=previousBorderx || backGroundBoarder.y != previousBordery)
        {
            return true;
        }
        else
        {
            return false;
        }

    }
    public void SetRowCol(int row, int col)
    {
        this.row = row;
        this.col = col;
        UpdateInventoryDisplay();
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
        this.inventory= inventory;
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
    public void SetHightlighted(GameObject slot)
    {
        Slot slotInstance = slot.GetComponent<Slot>();
        Item item = slotInstance.GetItem();
        if(item.GetHighlightable() && highlightable)
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
    private void InitPressableDict()
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
    public void SetEnableDisable(string EnableDisableOnPress)
    {
        this.EnableDisableOnPress = EnableDisableOnPress.ToCharArray()[0];
    }
    public string GetEnableDisable()
    {
        return EnableDisableOnPress.ToString();
    }
}
