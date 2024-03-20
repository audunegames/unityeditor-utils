using UnityEngine;

namespace Audune.Utils.UnityEditor.Editor
{
  // Class that defines extensions for the EditorGUIUtility class and related GUI classes
  public static class EditorGUIUtilityExtensions
  {
    // Class that defines a color scope
    public class ColorScope : GUI.Scope
    {
      // The original color before the scope was applied
      private Color _originalColor;


      // Create the scope
      public ColorScope(Color color, bool condition = true)
      {
        _originalColor = GUI.color;

        if (condition)
          GUI.color = color;
      }

      // Close the scope
      protected override void CloseScope()
      {
        GUI.color = _originalColor;
      }
    }


    // Class that defines a background color scope
    public class BackgroundColorScope : GUI.Scope
    {
      // The original color before the scope was applied
      private Color _originalColor;


      // Create the scope
      public BackgroundColorScope(Color color, bool condition = true)
      {
        _originalColor = GUI.backgroundColor;

        if (condition)
          GUI.backgroundColor = color;
      }

      // Close the scope
      protected override void CloseScope()
      {
        GUI.backgroundColor = _originalColor;
      }
    }


    // Class that defines a content color scope
    public class ContentColorScope : GUI.Scope
    {
      // The original color before the scope was applied
      private Color _originalColor;


      // Create the scope
      public ContentColorScope(Color color, bool condition = true)
      {
        _originalColor = GUI.contentColor;

        if (condition)
          GUI.contentColor = color;
      }

      // Close the scope
      protected override void CloseScope()
      {
        GUI.contentColor = _originalColor;
      }
    }
  }
}