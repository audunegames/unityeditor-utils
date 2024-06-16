using System;
using UnityEditor;
using UnityEngine;

namespace Audune.Utils.UnityEditor.Editor
{
  // Class that defines extensions for the EditorGUI class
  public static partial class EditorGUIExtensions
  {
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

    #region Drawing an item selector dropdown
    // Draw an item selector dropdown
    public static void ItemSelectorDropdown<TItem, TTreeView, TWindow>(Rect rect, GUIContent dropdownLabel, SerializedProperty serializedProperty) where TTreeView : ItemSelectorTreeView<TItem> where TWindow : ItemSelectorWindow<TItem, TTreeView>
    {
      if (EditorGUI.DropdownButton(rect, dropdownLabel, FocusType.Keyboard))
        ItemSelectorWindow<TItem, TTreeView>.OpenAsDropDown<TWindow>(rect, serializedProperty);
    }

    // Draw an item selector dropdown with a prefix label
    public static void ItemSelectorDropdown<TItem, TTreeView, TWindow>(Rect rect, GUIContent label, GUIContent dropdownLabel, SerializedProperty serializedProperty) where TTreeView : ItemSelectorTreeView<TItem> where TWindow : ItemSelectorWindow<TItem, TTreeView>
    {
      if (label != null)
        rect = EditorGUI.PrefixLabel(rect, label);
      ItemSelectorDropdown<TItem, TTreeView, TWindow>(rect, dropdownLabel, serializedProperty);
    }

    // Return the height of an item selector dropdown
    public static float GetItemSelectorDropdownHeight(SerializedProperty serializedProperty)
    {
      return EditorGUIUtility.singleLineHeight;
    }
    #endregion
  }
}