using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Audune.Utils.Unity.Editor
{
  // Class that defines extensions for the EditorGUILayout class
  public static class EditorGUILayoutExtensions
  { 
    #region Drawer methods for an array of properties
    // Draw the fields of the specified properties
    public static void MultiplePropertyFields(SerializedProperty[] properties, Func<SerializedProperty, int, GUIContent> labelSelector, params GUILayoutOption[] options)
    {
      var rect = EditorGUILayout.GetControlRect(false, EditorGUIExtensions.GetMultiplePropertyFieldsHeight(properties, labelSelector), options);
      EditorGUIExtensions.MultiplePropertyFields(rect, properties, labelSelector);
    }
    public static void MultiplePropertyFields(SerializedProperty[] properties, params GUILayoutOption[] options)
    {
      MultiplePropertyFields(properties, (property, index) => new GUIContent(property.displayName, property.tooltip), options);
    }
    #endregion

    #region Drawer methods for the array elements of a property
    // Draw the fields of the array element properties in the serialized property
    public static void ArrayElementPropertyFields(SerializedProperty serializedProperty, Func<SerializedProperty, int, GUIContent> labelSelector, params GUILayoutOption[] options)
    {
      MultiplePropertyFields(serializedProperty.GetArrayElements().ToArray(), labelSelector, options);
    }
    public static void ArrayElementPropertyFields(SerializedProperty serializedProperty, params GUILayoutOption[] options)
    {
      MultiplePropertyFields(serializedProperty.GetArrayElements().ToArray(), options);
    }
    #endregion

    #region Drawer methods for the children of a property
    // Draw the fields of the child properties in the serialized property
    public static void ChildPropertyFields(SerializedProperty serializedProperty, Predicate<SerializedProperty> predicate, params GUILayoutOption[] options)
    {
      MultiplePropertyFields(serializedProperty.GetChildren(predicate).ToArray(), options);
    }

    // Draw the fields of the child properties in the serialized object
    public static void ChildPropertyFields(SerializedObject serializedObject, Predicate<SerializedProperty> predicate, params GUILayoutOption[] options)
    {
      MultiplePropertyFields(serializedObject.GetChildren(predicate).ToArray(), options);
    }
    #endregion

    #region Drawer methods for inline object references
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

    #region Drawer methods for an item popup
    // Draw a popup containing the items in the list with the specified label
    public static T ItemPopup<T>(GUIContent label, List<T> items, Predicate<T> selected, Func<T, GUIContent> displayedItemSelector, params GUILayoutOption[] options)
    {
      var rect = EditorGUILayout.GetControlRect(label != null, EditorGUIUtility.singleLineHeight, options);
      return EditorGUIExtensions.ItemPopup(rect, label, items, selected, displayedItemSelector);
    }
    public static T ItemPopup<T>(GUIContent label, List<T> items, T selected, Func<T, GUIContent> displayedItemSelector, params GUILayoutOption[] options)
    {
      return ItemPopup(label, items, t => t.Equals(selected), displayedItemSelector, options);
    }

    // Draw a popup containing the items in the list
    public static T ItemPopup<T>(List<T> items, Predicate<T> selected, Func<T, GUIContent> displayedItemSelector, params GUILayoutOption[] options)
    {
      return ItemPopup(null, items, selected, displayedItemSelector, options);
    }
    public static T ItemPopup<T>(List<T> items, T selected, Func<T, GUIContent> displayedItemSelector, params GUILayoutOption[] options)
    {
      return ItemPopup(null, items, selected, displayedItemSelector, options);
    }
    #endregion
  }
}