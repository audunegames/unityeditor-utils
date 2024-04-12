using UnityEditor;
using UnityEngine;

namespace Audune.Utils.UnityEditor.Editor
{
  // Class that defines Styles for an item tree view
  public static class ItemTreeViewStyles
  {
    // Default styles
    public static readonly GUIStyle label;
    public static readonly GUIStyle labelRightAligned;
    public static readonly GUIStyle boldLabel;
    public static readonly GUIStyle boldLabelRightAligned;


    // Static constructor
    static ItemTreeViewStyles()
    {
      label = new GUIStyle(EditorStyles.label);
      label.alignment = TextAnchor.MiddleLeft;
      label.richText = true;
      label.padding.left = 2;
      label.padding.right = 2;

      labelRightAligned = new GUIStyle(label);
      labelRightAligned.alignment = TextAnchor.MiddleRight;

      boldLabel = new GUIStyle(label);
      boldLabel.fontStyle = FontStyle.Bold;

      boldLabelRightAligned = new GUIStyle(label);
      boldLabelRightAligned.fontStyle = FontStyle.Bold;
      boldLabelRightAligned.alignment = TextAnchor.MiddleRight;
    }
  }
}