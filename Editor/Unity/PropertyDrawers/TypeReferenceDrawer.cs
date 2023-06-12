using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Audune.Utils.Unity.Editor
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
      _attribute ??= fieldInfo.GetCustomAttribute<TypeReferenceAttribute>();
      if (_attribute == null)
      {
        EditorGUI.HelpBox(rect, "TypeReference must have a TypeReferenceAttribute attribute.", MessageType.Error);
        return;
      }

      _types ??= TypeUtils.GetChildTypes(_attribute);

      var typeName = property.FindPropertyRelative("_typeName");

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