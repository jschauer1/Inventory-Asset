using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomPropertyDrawer(typeof(InventoryInitializer))]
public class InventoryInitializerPropertyManager : PropertyDrawer
{
    int count = 0;
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        count = 0;
        
        EditorGUI.BeginProperty(position, label, property);
        SerializedProperty endProperty = property.GetEndProperty();
        SerializedProperty currentProperty = property.Copy();
        bool enterChildren = true;

        while (currentProperty.NextVisible(enterChildren) && !SerializedProperty.EqualContents(currentProperty, endProperty))
        {

            if (property.FindPropertyRelative("initialized").boolValue)
            {
                GUIContent label2 = new GUIContent(label);
                EditorGUI.LabelField(position, label2, EditorGUIUtility.TrTextContent("Initialized, Make Edits In Inventory"));
                break;
            }
            EditorGUI.PropertyField(position, currentProperty, true);
            position.y += EditorGUI.GetPropertyHeight(currentProperty) + EditorGUIUtility.standardVerticalSpacing;
            count++;
        }
        EditorGUI.EndProperty();
    }


    private void makeUIComp(ref Rect position, SerializedProperty property, string comp, string name, int size)
    {
        Rect isCheckRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        EditorGUI.PropertyField(isCheckRect, property.FindPropertyRelative(comp), new GUIContent(name));
        for (int i = 0; i < size; i++)
            position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing; // Move position down for next control
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float height = 0;
        for (int i = 0; i < count + 1; i++)
        {
            height += EditorGUIUtility.singleLineHeight;
        }
        return height;
    }
}

