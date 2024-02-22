using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Audune.Utils.UnityEditor.Editor
{
  // Class that defines extension methods for reorderable lists
  public static class ReorderableListExtensions
  {
    // Draw the list using the EditorGUI at the specified rect
    public static void DoList(this ReorderableList list, Rect rect, ReorderableListOptions options = ReorderableListOptions.None, GUIContent label = null)
    {
      label ??= new GUIContent(list.serializedProperty.displayName, list.serializedProperty.tooltip);

      if (options.HasFlag(ReorderableListOptions.DrawFoldout))
      {
        var topRect = rect.AlignTop(EditorGUIUtility.singleLineHeight);
        var foldoutRect = options.HasFlag(ReorderableListOptions.DrawInfoField) ? topRect.AlignLeft(EditorGUIUtility.labelWidth) : topRect;
        list.serializedProperty.isExpanded = EditorGUI.Foldout(foldoutRect, list.serializedProperty.isExpanded, label, true);

        if (options.HasFlag(ReorderableListOptions.DrawInfoField))
        {
          using (new EditorGUI.DisabledScope(true))
          {
            var fieldContent = $"{list.serializedProperty.arraySize} element{(list.serializedProperty.arraySize != 1 ? "s" : "")}";
            EditorGUI.LabelField(topRect.ContractLeft(foldoutRect.width + EditorGUIUtility.standardVerticalSpacing), fieldContent);
          }
        }

        if (list.serializedProperty.isExpanded)
          list.DoList(rect.ContractTop(EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing));
      }
      else
      {
        list.DoList(rect);
      }
    }

    // Draw the list using the EditorGUILayout
    public static void DoLayoutList(this ReorderableList list, ReorderableListOptions options = ReorderableListOptions.None, GUIContent label = null)
    {
      label ??= new GUIContent(list.serializedProperty.displayName, list.serializedProperty.tooltip);

      if (options.HasFlag(ReorderableListOptions.DrawFoldout))
      {
        var topRect = EditorGUILayout.GetControlRect();
        var foldoutRect = options.HasFlag(ReorderableListOptions.DrawInfoField) ? topRect.AlignLeft(EditorGUIUtility.labelWidth) : topRect;
        list.serializedProperty.isExpanded = EditorGUI.Foldout(foldoutRect, list.serializedProperty.isExpanded, label, true);

        if (options.HasFlag(ReorderableListOptions.DrawInfoField))
        {
          using (new EditorGUI.DisabledScope(true))
          {
            var fieldContent = $"{list.serializedProperty.arraySize} element{(list.serializedProperty.arraySize != 1 ? "s" : "")}";
            EditorGUI.LabelField(topRect.ContractLeft(foldoutRect.width + EditorGUIUtility.standardVerticalSpacing), fieldContent);
          }
        }

        if (list.serializedProperty.isExpanded)
          list.DoLayoutList();
      }
      else
      {
        list.DoLayoutList();
      }
    }

    // Return the height of the list
    public static float GetHeight(this ReorderableList list, ReorderableListOptions options = ReorderableListOptions.None)
    {
      if (options.HasFlag(ReorderableListOptions.DrawFoldout))
        return EditorGUIUtility.singleLineHeight + (list.serializedProperty.isExpanded ? EditorGUIUtility.standardVerticalSpacing + list.GetHeight() : 0.0f);
      else
        return list.GetHeight();
    }
  }
}