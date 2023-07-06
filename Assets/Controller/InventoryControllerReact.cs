using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(InventoryController))]
public class InventoryControllerReact : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // for other non-HideInInspector fields

        InventoryController script = (InventoryController)target;
        if (GUILayout.Button("Initialize Inventories")) // Button
        {
            script.InitializeInventories(); // Call the method in CustomEditorButton
        }
        if (GUILayout.Button("Reset")) // Button
        {
            script.ResetInventory(); // Call the method in CustomEditorButton
        }
    }
}