using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Audune.Utils.Editor
{
  // Class that defines extensions for the EditorGUI class
  public static class EditorGUIExtensions
  {
    #region Drawer methods for an array of properties
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
    public static void MultiplePropertyFields(Rect rect, SerializedProperty[] properties)
    {
      MultiplePropertyFields(rect, properties, (property, index) => new GUIContent(property.displayName, property.tooltip));
    }

    // Return the height of the fields of the specified properties
    public static float GetMultiplePropertyFieldsHeight(SerializedProperty[] properties, Func<SerializedProperty, int, GUIContent> labelSelector)
    {
      return properties.Select((property, index) => EditorGUI.GetPropertyHeight(property, labelSelector(property, index))).Aggregate(-EditorGUIUtility.standardVerticalSpacing, (a, b) => a + EditorGUIUtility.standardVerticalSpacing + b);
    }
    public static float GetMultiplePropertyFieldsHeight(SerializedProperty[] properties)
    {
      return GetMultiplePropertyFieldsHeight(properties, (property, index) => new GUIContent(property.displayName, property.tooltip));
    }
    #endregion

    #region Drawer methods for the array elements of a property
    // Draw the fields of the array element properties in the serialized property
    public static void ArrayElementPropertyFields(Rect rect, SerializedProperty serializedProperty, Func<SerializedProperty, int, GUIContent> labelSelector)
    {
      MultiplePropertyFields(rect, serializedProperty.GetArrayElements().ToArray(), labelSelector);
    }
    public static void ArrayElementPropertyFields(Rect rect, SerializedProperty serializedProperty)
    {
      MultiplePropertyFields(rect, serializedProperty.GetArrayElements().ToArray());
    }

    // Return the height of the fields of the array element properties in the serialized property
    public static float GetArrayElementPropertyFieldsHeight(SerializedProperty serializedProperty, Func<SerializedProperty, int, GUIContent> labelSelector)
    {
      return GetMultiplePropertyFieldsHeight(serializedProperty.GetArrayElements().ToArray(), labelSelector);
    }
    public static float GetArrayElementPropertyFieldsHeight(SerializedProperty serializedProperty)
    {
      return GetMultiplePropertyFieldsHeight(serializedProperty.GetArrayElements().ToArray());
    }
    #endregion

    #region Drawer methods for the children of a property
    // Draw the fields of the child properties in the serialized property
    public static void ChildPropertyFields(Rect rect, SerializedProperty serializedProperty, Predicate<SerializedProperty> predicate)
    {
      MultiplePropertyFields(rect, serializedProperty.GetChildren(predicate).ToArray());
    }

    // Draw the fields of the child properties in the serialized object
    public static void ChildPropertyFields(Rect rect, SerializedObject serializedObject, Predicate<SerializedProperty> predicate)
    {
      MultiplePropertyFields(rect, serializedObject.GetChildren(predicate).ToArray());
    }

    // Return the height of the fields of the child properties in the serialized property
    public static float GetChildPropertyFieldsHeight(SerializedProperty serializedProperty, Predicate<SerializedProperty> predicate)
    {
      return GetMultiplePropertyFieldsHeight(serializedProperty.GetChildren(predicate).ToArray());
    }

    // Return the height of the fields of the child properties in the serialized object
    public static float GetChildPropertyFieldsHeight(SerializedObject serializedObject, Predicate<SerializedProperty> predicate)
    {
      return GetMultiplePropertyFieldsHeight(serializedObject.GetChildren(predicate).ToArray());
    }
    #endregion

    #region Drawer methods for inline object references
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

    #region Drawer methods for an item popup
    // Draw a popup containing the items in the list with the specified label
    public static T ItemPopup<T>(Rect rect, GUIContent label, List<T> items, Predicate<T> selected, Func<T, GUIContent> displayedItemSelector)
    {
      var currentIndex = Mathf.Max(0, items.FindIndex(selected));
      var newIndex = EditorGUI.Popup(rect, label, currentIndex, items.Select(displayedItemSelector).ToArray());
      return items[newIndex];
    }
    public static T ItemPopup<T>(Rect rect, GUIContent label, List<T> items, T selected, Func<T, GUIContent> displayedItemSelector)
    {
      return ItemPopup(rect, label, items, t => t.Equals(selected), displayedItemSelector);
    }

    // Draw a popup containing the items in the list
    public static T ItemPopup<T>(Rect rect, List<T> items, Predicate<T> selected, Func<T, GUIContent> displayedItemSelector)
    {
      return ItemPopup(rect, null, items, selected, displayedItemSelector);
    }
    public static T ItemPopup<T>(Rect rect, List<T> items, T selected, Func<T, GUIContent> displayedItemSelector)
    {
      return ItemPopup(rect, null, items, selected, displayedItemSelector);
    }
    #endregion

    #region Drawer methods for a search tree view dropdown
    public static void SearchDropdown<T, TWindow>(Rect rect, GUIContent label) where TWindow : SearchWindow<T>
    {
      //if (EditorGUI.DropdownButton(rect, label, FocusType.Keyboard))
      //  SearchWindow<T>.ShowAsDropDown(rect, path, new GUIContent("Select localized key"));
    }

    #endregion
  }
}