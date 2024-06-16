using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Audune.Utils.UnityEditor.Editor
{
  // Class that defines extension methods for the EditorGUILayout class
  public static partial class EditorGUILayoutExtensions
  { 
    #region Drawing an item popup
    // Draw a popup containing the items in the list
    public static T ItemPopup<T>(List<T> items, Predicate<T> selected, Func<T, GUIContent> displayedItemSelector, params GUILayoutOption[] options)
    {
      return ItemPopup(null, items, selected, displayedItemSelector, options);
    }

    // Draw a popup containing the items in the list with the specified selected value
    public static T ItemPopup<T>(List<T> items, T selected, Func<T, GUIContent> displayedItemSelector, params GUILayoutOption[] options)
    {
      return ItemPopup(null, items, selected, displayedItemSelector, options);
    }

    // Draw a popup containing the items in the list with a prefix label
    public static T ItemPopup<T>(GUIContent label, List<T> items, Predicate<T> selected, Func<T, GUIContent> displayedItemSelector, params GUILayoutOption[] options)
    {
      var rect = EditorGUILayout.GetControlRect(label != null, EditorGUIUtility.singleLineHeight, options);
      return EditorGUIExtensions.ItemPopup(rect, label, items, selected, displayedItemSelector);
    }

    // Draw a popup containing the items in the list with the specified selected value with a prefix label
    public static T ItemPopup<T>(GUIContent dropdownLabel, List<T> items, T selected, Func<T, GUIContent> displayedItemSelector, params GUILayoutOption[] options)
    {
      return ItemPopup(dropdownLabel, items, t => t.Equals(selected), displayedItemSelector, options);
    }
    #endregion
  }
}