using System.Reflection.Emit;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(Item))]
public class ItemPropertyDrawer : PropertyDrawer
{
    Vector2 savePos = new Vector2();
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        Rect labelRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        EditorGUI.LabelField(labelRect, label);
        EditorGUI.indentLevel++;
        position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing; // Move position down for next control
        makeUIComp(ref position, property, "itemType", "itemType", 1);

        makeUIComp(ref position, property, "itemImage", "itemImage", 1);
        makeUIComp(ref position, property, "draggable", "Draggable", 1);

        makeUIComp(ref position, property, "highlightable", "Highlightable",1);
        if (property.FindPropertyRelative("highlightable").boolValue)
        {
            makeUIComp(ref position, property, "myEvent", "Choose Function",5);
        }
        EditorGUI.indentLevel = 0;

        EditorGUI.EndProperty();
    }


    private void makeUIComp(ref Rect position, SerializedProperty property, string comp ,string name,int size)
    {
        Rect isCheckRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        EditorGUI.PropertyField(isCheckRect, property.FindPropertyRelative(comp), new GUIContent(name));
        for(int i =0; i < size; i++)
        position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing; // Move position down for next control
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float height = 0;
        height += EditorGUIUtility.singleLineHeight;
        height += EditorGUIUtility.singleLineHeight;
        height += EditorGUIUtility.singleLineHeight;
        height += EditorGUIUtility.singleLineHeight;
        height += EditorGUIUtility.singleLineHeight;
        height += EditorGUIUtility.singleLineHeight;

        if (property.FindPropertyRelative("highlightable").boolValue)
        {
            EditorGUI.indentLevel++;
            height += EditorGUIUtility.singleLineHeight;
            height += EditorGUIUtility.singleLineHeight;
            height += EditorGUIUtility.singleLineHeight;
            height += EditorGUIUtility.singleLineHeight;
            height += EditorGUIUtility.singleLineHeight;
            height += EditorGUIUtility.singleLineHeight;
            height += EditorGUIUtility.standardVerticalSpacing;  // add space between the two properties
            EditorGUI.indentLevel = 0;

        }

        return height;
    }
}
