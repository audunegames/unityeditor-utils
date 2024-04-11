using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Audune.Utils.UnityEditor.Editor
{
  // Class that defines extensions for the EditorGUI class
  public static class EditorGUIExtensions
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

    #region Drawing the array elements of a property
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

    #region Drawing an item popup
    // Draw a popup containing the items in the list
    public static T ItemPopup<T>(Rect rect, List<T> items, Predicate<T> selected, Func<T, GUIContent> displayedItemSelector)
    {
      return ItemPopup(rect, null, items, selected, displayedItemSelector);
    }
    public static T ItemPopup<T>(Rect rect, List<T> items, T selected, Func<T, GUIContent> displayedItemSelector)
    {
      return ItemPopup(rect, null, items, selected, displayedItemSelector);
    }

    // Draw a popup containing the items in the list with a prefix label
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
    #endregion

    #region Drawing a generic menu dropdown
    // Draw a generic menu dropdown
    public static void GenericMenuDropdown(Rect rect, GUIContent dropdownLabel, GenericMenu menu)
    {
      if (EditorGUI.DropdownButton(rect, dropdownLabel, FocusType.Keyboard))
        menu.DropDown(rect);
    }

    // Draw a generic menu dropdown with a prefix label
    public static void GenericMenuDropdown(Rect rect, GUIContent label, GUIContent dropdownLabel, GenericMenu menu)
    {
      if (label != null)
        rect = EditorGUI.PrefixLabel(rect, label);
      GenericMenuDropdown(rect, dropdownLabel, menu);
    }

    // Return the height of a generic menu dropdown
    public static float GetGenericMenuDropdownHeight(GenericMenu menu)
    {
      return EditorGUIUtility.singleLineHeight;
    }
    #endregion

    #region Drawing an enum flags dropdown
    // Draw an enum flags dropdown
    public static void EnumFlagsDropdown<TEnum>(Rect rect, SerializedProperty serializedProperty) where TEnum : Enum
    {
      GenericMenuDropdown(rect, new GUIContent(ObjectNames.NicifyVariableName(((TEnum)Enum.ToObject(typeof(TEnum), serializedProperty.enumValueFlag)).ToString())), CreateEnumFlagsMenu<TEnum>(serializedProperty));
    }

    // Draw an enum flags dropdown with a prefix label
    public static void EnumFlagsDropdown<TEnum>(Rect rect, GUIContent label, SerializedProperty serializedProperty) where TEnum : Enum
    {
      if (label != null)
        rect = EditorGUI.PrefixLabel(rect, label);
      EnumFlagsDropdown<TEnum>(rect, serializedProperty);
    }

    // Return the height an enum flags dropdown
    public static float GetEnumFlagsDropdownHeight<TEnum>(SerializedProperty serializedProperty) where TEnum : Enum
    {
      return GetGenericMenuDropdownHeight(CreateEnumFlagsMenu<TEnum>(serializedProperty));
    }

    // Create an enum flag menu for a property
    private static GenericMenu CreateEnumFlagsMenu<TEnum>(SerializedProperty serializedProperty) where TEnum : Enum
    {
      var menu = new GenericMenu();
      foreach (var flag in (TEnum[])Enum.GetValues(typeof(TEnum)))
        menu.AddItem(new GUIContent(ObjectNames.NicifyVariableName(Enum.GetName(typeof(TEnum), flag))), HasEnumFlag(serializedProperty, Convert.ToInt32(flag)), () => SetEnumFlag(serializedProperty, Convert.ToInt32(flag)));
      return menu;
    }

    // Return if a property has an enum flag
    private static bool HasEnumFlag(SerializedProperty serializedProperty, int flag)
    {
      if (flag == 0)
        return serializedProperty.enumValueFlag == 0;
      else
        return (serializedProperty.enumValueFlag & flag) == flag;
    }

    // Set the enum flag of a property
    private static void SetEnumFlag(SerializedProperty serializedProperty, int flag)
    {
      serializedProperty.serializedObject.Update();

      if (flag == 0)
        serializedProperty.enumValueFlag = 0;
      if ((serializedProperty.enumValueFlag & flag) == flag)
        serializedProperty.enumValueFlag &= ~flag;
      else
        serializedProperty.enumValueFlag |= flag;

      serializedProperty.serializedObject.ApplyModifiedProperties();
    }
    #endregion

    #region Drawing a search tree view dropdown
    // Draw a search dropdown
    public static void SearchDropdown<T, TWindow>(Rect rect, GUIContent dropdownLabel, SerializedProperty serializedProperty) where TWindow : SearchWindow<T>
    {
      if (EditorGUI.DropdownButton(rect, dropdownLabel, FocusType.Keyboard))
        SearchWindow<T>.ShowAsDropDown<TWindow>(rect, serializedProperty);
    }

    // Draw a search dropdown with a prefix label
    public static void SearchDropdown<T, TWindow>(Rect rect, GUIContent label, GUIContent dropdownLabel, SerializedProperty serializedProperty) where TWindow : SearchWindow<T>
    {
      if (label != null)
        rect = EditorGUI.PrefixLabel(rect, label);
      SearchDropdown<T, TWindow>(rect, dropdownLabel, serializedProperty);
    }

    // Return the height of a search dropdown
    public static float GetSearchDropdownHeight(SerializedProperty serializedProperty)
    {
      return EditorGUIUtility.singleLineHeight;
    }
    #endregion
  }
}