using Audune.Utils.Unity.Editor;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Audune.Utils.Types.Editor
{
  // Class that defines a drawer for type references
  [CustomPropertyDrawer(typeof(TypeReference))]
  public class TypeReferenceDrawer : PropertyDrawer
  {
    // The options attribute of the attribute drawer
    private TypeReferenceAttribute _attribute;

    // The types of the attribute drawer
    private List<Type> _types;

    
    // Draw the property GUI
    public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
    {
      using var scope = new EditorGUI.PropertyScope(rect, label, property);

      _attribute ??= fieldInfo.GetCustomAttribute<TypeReferenceAttribute>();
      if (_attribute == null)
      {
        EditorGUI.HelpBox(rect, "TypeReference must have a TypeReferenceAttribute attribute.", MessageType.Error);
        return;
      }

      _types ??= TypeUtils.GetChildTypes(_attribute);

      var typeName = property.FindPropertyRelative("_typeName");
      if (string.IsNullOrEmpty(typeName.stringValue))
        typeName.stringValue = _types[0].AssemblyQualifiedName;

      label = EditorGUI.BeginProperty(rect, label, property);

      var newType = EditorGUIExtensions.ItemPopup(rect, label, _types, type => type.AssemblyQualifiedName == typeName.stringValue, type => new GUIContent(type.ToDisplayString(_attribute?.displayStringOptions ?? TypeDisplayStringOptions.None)));
      typeName.stringValue = newType.AssemblyQualifiedName;

      EditorGUI.EndProperty();
    }

    // Return the property height
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
      return EditorGUIUtility.singleLineHeight;
    }
  }
}