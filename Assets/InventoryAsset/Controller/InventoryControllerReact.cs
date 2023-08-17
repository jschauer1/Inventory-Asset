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
            EditorApplication.delayCall += script.InitializeInventories;
        }
        else if (GUILayout.Button("Reset"))
        {
            EditorApplication.delayCall += script.ResetInventory;
        }
    }
}
