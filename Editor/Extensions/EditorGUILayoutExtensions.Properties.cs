using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Audune.Utils.UnityEditor.Editor
{
  // Class that defines extensions for the EditorGUILayout class
  public static partial class EditorGUILayoutExtensions
  { 
    #region Drawing an array of properties
    // Draw the fields of the specified properties
    public static void MultiplePropertyFields(SerializedProperty[] properties, Func<SerializedProperty, int, GUIContent> labelSelector, params GUILayoutOption[] options)
    {
      var rect = EditorGUILayout.GetControlRect(false, EditorGUIExtensions.GetMultiplePropertyFieldsHeight(properties, labelSelector), options);
      EditorGUIExtensions.MultiplePropertyFields(rect, properties, labelSelector);
    }

    // Draw the fields of the specified properties with a default label selector
    public static void MultiplePropertyFields(SerializedProperty[] properties, params GUILayoutOption[] options)
    {
      MultiplePropertyFields(properties, (property, index) => new GUIContent(property.displayName, property.tooltip), options);
    }
    #endregion

    #region Drawing the array elements of a property
    // Draw the fields of the array element properties in the serialized property
    public static void ArrayElementPropertyFields(SerializedProperty serializedProperty, Func<SerializedProperty, int, GUIContent> labelSelector, params GUILayoutOption[] options)
    {
      MultiplePropertyFields(serializedProperty.GetArrayElements().ToArray(), labelSelector, options);
    }

    // Draw the fields of the array element properties in the serialized property with a default label selector
    public static void ArrayElementPropertyFields(SerializedProperty serializedProperty, params GUILayoutOption[] options)
    {
      MultiplePropertyFields(serializedProperty.GetArrayElements().ToArray(), options);
    }
    #endregion

    #region Drawing the children of a property
    // Draw the fields of the child properties in the serialized property
    public static void ChildPropertyFields(SerializedProperty serializedProperty, bool enterChildren, Predicate<SerializedProperty> predicate, params GUILayoutOption[] options)
    {
      MultiplePropertyFields(serializedProperty.GetChildren(enterChildren, predicate).ToArray(), options);
    }

    // Draw the fields of the child properties in the serialized object
    public static void ChildPropertyFields(SerializedObject serializedObject, bool enterChildren, Predicate<SerializedProperty> predicate, params GUILayoutOption[] options)
    {
      MultiplePropertyFields(serializedObject.GetChildren(enterChildren, predicate).ToArray(), options);
    }
    #endregion
  }
}