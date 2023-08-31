using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DropDownList))]
public class DropDownListReact : Editor
{
    public override void OnInspectorGUI()
    {
        DropDownList script = (DropDownList)target;


        // Draw the default inspector
        DrawDefaultInspector();

        // If there are items in the list, show the dropdown
        if (script.items.Count > 0)
        {
            string[] itemNames = new string[script.items.Count];
            for (int i = 0; i < script.items.Count; i++)
            {
                itemNames[i] = script.items[i].itemName;
            }

            EditorGUI.BeginChangeCheck();  // Begin checking for changes

            script.selectedItemIndex = EditorGUILayout.Popup("Select Item", script.selectedItemIndex, itemNames);

            if (EditorGUI.EndChangeCheck())  // If any changes occurred
            {
                Debug.Log("Item changed to: " + itemNames[script.selectedItemIndex]);
                EditorUtility.SetDirty(target);  // Mark the object as "dirty" to ensure changes are saved
            }
        }
    }
}
