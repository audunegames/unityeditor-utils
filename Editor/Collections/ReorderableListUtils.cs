using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Audune.Utils.Editor
{
  // Class that defines utility methods for a reorderable list
  public static class ReorderableListUtls
  {
    // Draw the list using the EditorGUI at the specified rect
    public static void DoList(this ReorderableList list, Rect rect, ReorderableListDrawOptions options = ReorderableListDrawOptions.None, GUIContent label = null)
    {
      label ??= new GUIContent(list.serializedProperty.displayName, list.serializedProperty.tooltip);

      if (options.HasFlag(ReorderableListDrawOptions.DrawFoldout))
      {
        var topRect = rect.AlignTop(EditorGUIUtility.singleLineHeight);
        var foldoutRect = options.HasFlag(ReorderableListDrawOptions.DrawInfoField) ? topRect.AlignLeft(EditorGUIUtility.labelWidth) : topRect;
        list.serializedProperty.isExpanded = EditorGUI.Foldout(foldoutRect, list.serializedProperty.isExpanded, label, true);

        if (options.HasFlag(ReorderableListDrawOptions.DrawInfoField))
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
    public static void DoLayoutList(this ReorderableList list, ReorderableListDrawOptions options = ReorderableListDrawOptions.None, GUIContent label = null)
    {
      label ??= new GUIContent(list.serializedProperty.displayName, list.serializedProperty.tooltip);

      if (options.HasFlag(ReorderableListDrawOptions.DrawFoldout))
      {
        var topRect = EditorGUILayout.GetControlRect();
        var foldoutRect = options.HasFlag(ReorderableListDrawOptions.DrawInfoField) ? topRect.AlignLeft(EditorGUIUtility.labelWidth) : topRect;
        list.serializedProperty.isExpanded = EditorGUI.Foldout(foldoutRect, list.serializedProperty.isExpanded, label, true);

        if (options.HasFlag(ReorderableListDrawOptions.DrawInfoField))
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
    public static float GetHeight(this ReorderableList list, ReorderableListDrawOptions options = ReorderableListDrawOptions.None)
    {
      if (options.HasFlag(ReorderableListDrawOptions.DrawFoldout))
        return EditorGUIUtility.singleLineHeight + (list.serializedProperty.isExpanded ? EditorGUIUtility.standardVerticalSpacing + list.GetHeight() : 0.0f);
      else
        return list.GetHeight();
    }
  }
}