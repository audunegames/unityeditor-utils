using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Audune.Utils.UnityEditor.Editor
{
  // Class that defines a drawer for serializable types
  [CustomPropertyDrawer(typeof(SerializableType))]
  public class SerializableTypeDrawer : PropertyDrawer
  {
    // The options attribute of the attribute drawer
    private SerializableTypeOptionsAttribute _attribute;

    // The types of the attribute drawer
    private List<Type> _types;

    
    // Draw the property GUI
    public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
    {
      using var scope = new EditorGUI.PropertyScope(rect, label, property);

      _attribute ??= fieldInfo.GetCustomAttribute<SerializableTypeOptionsAttribute>();
      if (_attribute == null)
      {
        EditorGUI.HelpBox(rect, $"{typeof(SerializableType)} must have a {typeof(SerializableTypeOptionsAttribute)} attribute", MessageType.None);
        return;
      }

      _types ??= TypeExtensions.GetChildTypes(_attribute.baseType).ToList();

      var typeName = property.FindPropertyRelative("_typeName");
      if (string.IsNullOrEmpty(typeName.stringValue))
        typeName.stringValue = _types[0].AssemblyQualifiedName;

      label = EditorGUI.BeginProperty(rect, label, property);

      var newType = EditorGUIExtensions.ItemPopup(rect, label, _types, type => type.AssemblyQualifiedName == typeName.stringValue, type => new GUIContent(type.ToDisplayString(_attribute?.displayOptions ?? TypeDisplayOptions.None)));
      typeName.stringValue = newType.AssemblyQualifiedName;
    }

    // Return the property height
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
      return EditorGUIUtility.singleLineHeight;
    }
  }
}