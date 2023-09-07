using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class InventoryItemEvent : UnityEvent<InventoryItem> { }
[System.Serializable]
public class InventoryItemPosEvent : UnityEvent<Vector3,InventoryItem> { }

