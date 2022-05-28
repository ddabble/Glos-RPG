using UnityEditor;
using UnityEngine;


// Code based on https://github.com/hackerspace-ntnu/Temple-Trashers/commit/556b0329d2b8432e261879d3e87bf5638170f4e7
[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        GUI.enabled = false;
        DrawGUIField(position, property, label);
        GUI.enabled = true;
    }

    protected virtual void DrawGUIField(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.PropertyField(position, property, label, true);
    }
}
