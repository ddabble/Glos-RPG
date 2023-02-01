using UnityEditor;
using UnityEngine;


// Code based on https://github.com/hackerspace-ntnu/Temple-Trashers/commit/556b0329d2b8432e261879d3e87bf5638170f4e7
[CustomPropertyDrawer(typeof(TextLabelAttribute))]
public class TextLabelDrawer : ReadOnlyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float baseHeight = EditorGUI.GetPropertyHeight(property, label, true);
        int numLines = GetFieldStringRepr(property).Split('\n').Length;
        return numLines * baseHeight;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (attribute is TextLabelAttribute textLabelAttribute && textLabelAttribute.greyedOut)
            base.OnGUI(position, property, label);
        else
        {
            // Skip the greying-out logic in the base class (through disabling and re-enabling `GUI`)
            DrawGUIField(position, property, label);
        }
    }

    protected override void DrawGUIField(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.LabelField(position, label.text, GetFieldStringRepr(property));
    }

    private string GetFieldStringRepr(SerializedProperty property)
    {
        object attributeValue = fieldInfo.GetValue(property.serializedObject.targetObject);
        return attributeValue.ToString();
    }
}
