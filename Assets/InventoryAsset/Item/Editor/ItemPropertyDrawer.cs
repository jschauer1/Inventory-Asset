using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ItemInitializer))]
public class ItemPropertyDrawer : PropertyDrawer
{
    int count = 0;
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        count = 0;
        EditorGUI.BeginProperty(position, label, property);
        SerializedProperty endProperty = property.GetEndProperty();
        SerializedProperty currentProperty = property.Copy();
        bool enterChildren = true;

        while (currentProperty.NextVisible(enterChildren)&&!SerializedProperty.EqualContents(currentProperty, endProperty))
        {
            EditorGUI.PropertyField(position, currentProperty, true);
            position.y += EditorGUI.GetPropertyHeight(currentProperty) + EditorGUIUtility.standardVerticalSpacing;
            count++;
        }

        if (property.FindPropertyRelative("highlightable").boolValue)
        {
            makeUIComp(ref position, property, "myEvent", "Choose Function on Highlight", 5);
        }
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
        for(int i = 0; i < count+1; i++)
        {
            height += EditorGUIUtility.singleLineHeight;
        }

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
