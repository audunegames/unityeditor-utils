using System;
using UnityEditor;
using UnityEngine;

namespace Audune.Utils.UnityEditor.Editor
{
  // Class that defines extension methods for the EditorGUI class
  public static partial class EditorGUIExtensions
  {
    #region Drawing inline object references
    // Draw a custom GUI for objects if their reference is set
    public static void InlineObject(Rect rect, SerializedProperty serializedProperty, Action<Rect, SerializedObject> customGUI)
    {
      if (serializedProperty.propertyType == SerializedPropertyType.ObjectReference && serializedProperty.objectReferenceValue != null)
      {
        var serializedObject = new SerializedObject(serializedProperty.objectReferenceValue);

        EditorGUI.BeginChangeCheck();
        customGUI(rect, serializedObject);
        if (EditorGUI.EndChangeCheck())
          serializedObject.ApplyModifiedProperties();
      }
      else
      {
        EditorGUI.PropertyField(rect, serializedProperty);
      }
    }

    // Return the height of a custom GUI for objects if their reference is set
    public static float GetInlineObjectHeight(SerializedProperty serializedProperty, Func<SerializedObject, float> customGUIHeight)
    {
      if (serializedProperty.propertyType == SerializedPropertyType.ObjectReference && serializedProperty.objectReferenceValue != null)
      {
        var serializedObject = new SerializedObject(serializedProperty.objectReferenceValue);

        return customGUIHeight(serializedObject);
      }
      else
      {
        return EditorGUI.GetPropertyHeight(serializedProperty);
      }
    }
    #endregion
  }
}