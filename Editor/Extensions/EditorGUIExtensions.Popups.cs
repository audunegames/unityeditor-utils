using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Audune.Utils.UnityEditor.Editor
{
  // Class that defines extension methods for the EditorGUI class
  public static partial class EditorGUIExtensions
  {
    #region Drawing an item popup
    // Draw a popup containing the items in the list
    public static T ItemPopup<T>(Rect rect, List<T> items, Predicate<T> selected, Func<T, GUIContent> displayedItemSelector)
    {
      return ItemPopup(rect, null, items, selected, displayedItemSelector);
    }

    // Draw a popup containing the items in the list with the specified selected value
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

    // Draw a popup containing the items in the list with the specified selected value with a prefix label
    public static T ItemPopup<T>(Rect rect, GUIContent label, List<T> items, T selected, Func<T, GUIContent> displayedItemSelector)
    {
      return ItemPopup(rect, label, items, t => t.Equals(selected), displayedItemSelector);
    }
    #endregion
  }
}