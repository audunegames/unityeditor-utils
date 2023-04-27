using UnityEditor;
using UnityEngine;

namespace Audune.Utils.Sprites
{
  [CustomPropertyDrawer(typeof(SpriteData))]
  public class SpriteDataDrawer : PropertyDrawer
  {
    // Draw the property
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
      var name = property.FindPropertyRelative("name");
      var rect = property.FindPropertyRelative("rect");
      var pivot = property.FindPropertyRelative("pivot");
      var border = property.FindPropertyRelative("border");

      Rect linePosition;

      EditorGUI.BeginProperty(position, label, property);

      linePosition = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
      EditorGUI.LabelField(linePosition, new GUIContent("Name", name.tooltip), new GUIContent(name.stringValue));

      linePosition.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
      var rectValue = rect.rectValue;
      EditorGUI.LabelField(linePosition, new GUIContent("Position", rect.tooltip), new GUIContent($"X:{rectValue.x}, Y:{rectValue.y}, W:{rectValue.width}, H:{rectValue.height}"));

      linePosition.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
      var pivotValue = pivot.vector2Value;
      EditorGUI.LabelField(linePosition, new GUIContent("Pivot", pivot.tooltip), new GUIContent($"X:{pivotValue.x}, Y:{pivotValue.y}"));

      linePosition.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
      var borderValue = border.vector4Value;
      EditorGUI.LabelField(linePosition, new GUIContent("Border", border.tooltip), new GUIContent($"L:{borderValue.x}, R:{borderValue.z}, T:{borderValue.w}, B:{borderValue.y}"));

      EditorGUI.EndProperty();
    }

    // Return the height of the property
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
      return (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * 3 + EditorGUIUtility.singleLineHeight;
    }
  }
}