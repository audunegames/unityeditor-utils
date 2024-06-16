using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Audune.Utils.UnityEditor.Editor
{
  // Class that defines extensions for the EditorGUI class
  public static partial class EditorGUIExtensions
  {
    #region Drawing an array of properties
    // Draw the fields of the specified properties
    public static void MultiplePropertyFields(Rect rect, SerializedProperty[] properties, Func<SerializedProperty, int, GUIContent> labelSelector)
    {
      for (var i = 0; i < properties.Length; i++)
      {
        var property = properties[i];
        var height = EditorGUI.GetPropertyHeight(property);
        EditorGUI.PropertyField(rect.AlignTop(height, EditorGUIUtility.standardVerticalSpacing, out rect), property, labelSelector(property, i));
      }
    }

    // Draw the fields of the specified properties with a default label selector
    public static void MultiplePropertyFields(Rect rect, SerializedProperty[] properties)
    {
      MultiplePropertyFields(rect, properties, (property, index) => new GUIContent(property.displayName, property.tooltip));
    }

    // Return the height of the fields of the specified properties
    public static float GetMultiplePropertyFieldsHeight(SerializedProperty[] properties, Func<SerializedProperty, int, GUIContent> labelSelector)
    {
      return properties.Select((property, index) => EditorGUI.GetPropertyHeight(property, labelSelector(property, index))).Aggregate(-EditorGUIUtility.standardVerticalSpacing, (a, b) => a + EditorGUIUtility.standardVerticalSpacing + b);
    }

    // Return the height of the fields of the specified properties with a default label selector
    public static float GetMultiplePropertyFieldsHeight(SerializedProperty[] properties)
    {
      return GetMultiplePropertyFieldsHeight(properties, (property, index) => new GUIContent(property.displayName, property.tooltip));
    }
    #endregion

    #region Drawing the array elements of a property
    // Draw the fields of the array element properties in the serialized property
    public static void ArrayElementPropertyFields(Rect rect, SerializedProperty serializedProperty, Func<SerializedProperty, int, GUIContent> labelSelector)
    {
      MultiplePropertyFields(rect, serializedProperty.GetArrayElements().ToArray(), labelSelector);
    }

    // Draw the fields of the array element properties in the serialized property with a default label selector
    public static void ArrayElementPropertyFields(Rect rect, SerializedProperty serializedProperty)
    {
      MultiplePropertyFields(rect, serializedProperty.GetArrayElements().ToArray());
    }

    // Return the height of the fields of the array element properties in the serialized property
    public static float GetArrayElementPropertyFieldsHeight(SerializedProperty serializedProperty, Func<SerializedProperty, int, GUIContent> labelSelector)
    {
      return GetMultiplePropertyFieldsHeight(serializedProperty.GetArrayElements().ToArray(), labelSelector);
    }

    // Return the height of the fields of the array element properties in the serialized property with a default label selector
    public static float GetArrayElementPropertyFieldsHeight(SerializedProperty serializedProperty)
    {
      return GetMultiplePropertyFieldsHeight(serializedProperty.GetArrayElements().ToArray());
    }
    #endregion

    #region Drawing the children of a property
    // Draw the fields of the child properties in the serialized property
    public static void ChildPropertyFields(Rect rect, SerializedProperty serializedProperty, bool enterChildren, Predicate<SerializedProperty> predicate)
    {
      MultiplePropertyFields(rect, serializedProperty.GetChildren(enterChildren, predicate).ToArray());
    }

    // Draw the fields of the child properties in the serialized object
    public static void ChildPropertyFields(Rect rect, SerializedObject serializedObject, bool enterChildren, Predicate<SerializedProperty> predicate)
    {
      MultiplePropertyFields(rect, serializedObject.GetChildren(enterChildren, predicate).ToArray());
    }

    // Return the height of the fields of the child properties in the serialized property
    public static float GetChildPropertyFieldsHeight(SerializedProperty serializedProperty, bool enterChildren, Predicate<SerializedProperty> predicate)
    {
      return GetMultiplePropertyFieldsHeight(serializedProperty.GetChildren(enterChildren, predicate).ToArray());
    }

    // Return the height of the fields of the child properties in the serialized object
    public static float GetChildPropertyFieldsHeight(SerializedObject serializedObject, bool enterChildren, Predicate<SerializedProperty> predicate)
    {
      return GetMultiplePropertyFieldsHeight(serializedObject.GetChildren(enterChildren, predicate).ToArray());
    }
    #endregion
  }
}