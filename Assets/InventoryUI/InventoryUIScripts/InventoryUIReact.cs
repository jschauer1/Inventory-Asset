using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(InventoryUI))]
public class InventoryUIReact : Editor
{
    public override void OnInspectorGUI()
    {
        // Reference to the InventoryUI script.
        InventoryUI inventoryUI = (InventoryUI)target;

        // Draw default inspector content.
        DrawDefaultInspector();

        // If any changes in the inspector.
        if (GUI.changed)
        {
            // Apply changes to inventory display.
            inventoryUI.UpdateInventoryDisplay();

            // Mark object as dirty.
            EditorUtility.SetDirty(inventoryUI);
        }
    }
}