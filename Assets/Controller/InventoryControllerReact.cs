using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(InventoryController))]
public class InventoryControllerReact : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // for other non-HideInInspector fields

        InventoryController script = (InventoryController)target;

        if (GUILayout.Button("Initialize Inventories"))
        {
            script.InitializeInventories();
        }
        if (GUILayout.Button("Reset"))
        {
            script.ResetInventory();
        }

    }
}