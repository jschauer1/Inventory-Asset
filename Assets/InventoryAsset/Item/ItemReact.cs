using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

using Unity.VisualScripting.FullSerializer;
using static UnityEditor.Progress;

[CustomEditor(typeof(ItemInitializer))]
public class ItemReact : Editor
{
    SerializedProperty Checked;
    SerializedProperty SomeField;


    public override void OnInspectorGUI()
    {
       // DrawDefaultInspector(); // for other non-HideInInspector fields
/*
        InventoryController script = (InventoryController)target;
        if (Checked.boolValue)
            EditorGUILayout.PropertyField(SomeField);

        serializedObject.ApplyModifiedProperties();*/
    }
}