using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(InventoryUIManager))]
internal class InventoryUIReact : Editor
{
    private bool needToUpdate = false;

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();

        // Draw default inspector content.
        DrawDefaultInspector();

        // Check if anything was changed and if there's no active GUI control.
        if (EditorGUI.EndChangeCheck() && GUIUtility.hotControl == 0)
        {
            needToUpdate = true;
        }

        // If any changes in the inspector and no active GUI control.
        if (needToUpdate && GUIUtility.hotControl == 0)
        {
            // Reference to the InventoryUI script.
            InventoryUIManager inventoryUI = (InventoryUIManager)target;

            // Apply changes to inventory display.
            inventoryUI.UpdateInventoryUI();

            // Mark object as dirty.
            EditorUtility.SetDirty(inventoryUI);

            needToUpdate = false;
        }
    }
}
