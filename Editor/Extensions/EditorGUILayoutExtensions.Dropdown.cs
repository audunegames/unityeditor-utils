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
    #region Drawing a generic menu dropdown
    // Draw a generic menu dropdown
    public static void GenericMenuDropdown(GUIContent dropdownLabel, GenericMenu menu, params GUILayoutOption[] options)
    {
      var rect = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight, options);
      EditorGUIExtensions.GenericMenuDropdown(rect, dropdownLabel, menu);
    }

    // Draw a generic menu dropdown with a prefix label
    public static void GenericMenuDropdown(GUIContent label, GUIContent dropdownLabel, GenericMenu menu, params GUILayoutOption[] options)
    {
      var rect = EditorGUILayout.GetControlRect(label != null, EditorGUIUtility.singleLineHeight, options);
      EditorGUIExtensions.GenericMenuDropdown(rect, label, dropdownLabel, menu);
    }
    #endregion

    #region Drawing an enum flags dropdown
    // Draw an enum flags dropdown
    public static void EnumFlagsDropdown<TEnum>(SerializedProperty serializedProperty, params GUILayoutOption[] options) where TEnum : Enum
    {
      var rect = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight, options);
      EditorGUIExtensions.EnumFlagsDropdown<TEnum>(rect, serializedProperty);
    }
    // Draw an enum flags dropdown with a prefix label
    public static void EnumFlagsDropdown<TEnum>(GUIContent label, SerializedProperty serializedProperty, params GUILayoutOption[] options) where TEnum : Enum
    {
      var rect = EditorGUILayout.GetControlRect(label != null, EditorGUIUtility.singleLineHeight, options);
      EditorGUIExtensions.EnumFlagsDropdown<TEnum>(rect, label, serializedProperty);
    }
    #endregion

    #region Drawing an item selector dropdown
    // Draw an item selector dropdown
    public static void ItemSelectorDropdown<TItem, TTreeView, TWindow>(GUIContent dropdownLabel, SerializedProperty serializedProperty, params GUILayoutOption[] options) where TTreeView : ItemSelectorTreeView<TItem> where TWindow : ItemSelectorWindow<TItem, TTreeView>
    {
      var rect = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight, options);
      EditorGUIExtensions.ItemSelectorDropdown<TItem, TTreeView, TWindow>(rect, dropdownLabel, serializedProperty);
    }
    // Draw an item selector with a prefix label
    public static void ItemSelectorDropdown<TItem, TTreeView, TWindow>(GUIContent label, GUIContent dropdownLabel, SerializedProperty serializedProperty, params GUILayoutOption[] options) where TTreeView : ItemSelectorTreeView<TItem> where TWindow : ItemSelectorWindow<TItem, TTreeView>
    {
      var rect = EditorGUILayout.GetControlRect(label != null, EditorGUIUtility.singleLineHeight, options);
      EditorGUIExtensions.ItemSelectorDropdown<TItem, TTreeView, TWindow>(rect, label, dropdownLabel, serializedProperty);
    }
    #endregion
  }
}