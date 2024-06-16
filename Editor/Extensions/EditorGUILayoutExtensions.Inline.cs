using System;
using UnityEditor;

namespace Audune.Utils.UnityEditor.Editor
{
  // Class that defines extension methods for the EditorGUILayout class
  public static partial class EditorGUILayoutExtensions
  { 
    #region Drawing inline object references
    // Draw a custom GUI for objects if their reference is set
    public static void InlineObject(SerializedProperty serializedProperty, Action<SerializedObject> customGUI)
    {
      if (serializedProperty.propertyType == SerializedPropertyType.ObjectReference && serializedProperty.objectReferenceValue != null)
      {
        var serializedObject = new SerializedObject(serializedProperty.objectReferenceValue);

        EditorGUI.BeginChangeCheck();
        customGUI(serializedObject);
        if (EditorGUI.EndChangeCheck())
          serializedObject.ApplyModifiedProperties();
      }
      else
      {
        EditorGUILayout.PropertyField(serializedProperty);
      }
    }
    #endregion
  }
}