using UnityEditor;
using UnityEngine;

namespace Audune.Utils.Sprites
{
  [CustomEditor(typeof(SpriteSheet))]
  public class SpriteSheetEditor : Editor
  {
    // Draw the inspector
    public override void OnInspectorGUI()
    {
      var sprites = serializedObject.FindProperty("sprites");

      // Draw the amount of sprites
      EditorGUILayout.LabelField(new GUIContent($"Sprite sheet with {sprites.arraySize} sprites"), EditorStyles.boldLabel);

      // Draw the sprites
      EditorGUI.indentLevel++;
      for (var i = 0; i < sprites.arraySize; i++)
      {
        var sprite = sprites.GetArrayElementAtIndex(i);

        sprite.isExpanded = EditorGUILayout.Foldout(sprite.isExpanded, new GUIContent(sprite.FindPropertyRelative("name").stringValue));
        if (sprite.isExpanded)
          EditorGUILayout.PropertyField(sprite);
      }
      EditorGUI.indentLevel--;
    }
  }
}