using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SecondaryAudioClip))] 
[CustomPropertyDrawer(typeof(Suite))]
public class SuiteStruct_Drawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Find the 'name' property within the struct
        SerializedProperty nameProperty = property.FindPropertyRelative("id");

        // If 'name' is not empty, use it as the label
        if (!string.IsNullOrEmpty(nameProperty.stringValue))
        {
            label = new GUIContent(nameProperty.stringValue);
        }

        // Draw the property with the new label
        EditorGUI.PropertyField(position, property, label, true);
    }

    // Ensure the property height is correctly calculated
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }
}