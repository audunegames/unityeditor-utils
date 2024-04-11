using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Audune.Utils.UnityEditor.Editor
{
  // Class that defines a result in a property search
  public sealed class SerializedPropertySearchResult
  {
    // Asset of the result
    public EditorAsset asset;

    // Component of the result
    public EditorComponent component;

    // Property path of the result
    public readonly string propertyPath;

    // Key of the property when the result is a dictionary
    public readonly int propertyDictionaryKeyIndex;


    // Return the target object of the component
    public Object targetObject => component.targetObject;

    // Return the target serialized object of the component
    public SerializedObject targetSerializedObject => component.targetSerializedObject;

    // Return the target serialized property of the result
    public SerializedProperty targetSerializedProperty {
      get {
        var targetSerializedObject = this.targetSerializedObject;
        return targetSerializedObject != null ? targetSerializedObject.FindProperty(propertyPath) : null;
      }
    }

    // Return the property value of the result
    public object propertyValue {
      get {
        var targetSerializedProperty = this.targetSerializedProperty;
        if (targetSerializedProperty != null)
        {
          return targetSerializedProperty.boxedValue;
        }
        else
        {
          var propertyField = component.componentType.GetField(propertyPath);
          var targetObject = this.targetObject;
          dynamic value = targetObject != null && propertyField != null ? propertyField.GetValue(targetObject) : null;
          if (value != null && propertyDictionaryKeyIndex >= 0 && propertyDictionaryKeyIndex < value.keys.Count)
            return value.Get(value.keys[propertyDictionaryKeyIndex]);
          else
            return value;
        }
      }
    }


    // Constructor
    public SerializedPropertySearchResult(EditorAsset asset, EditorComponent component, string propertyPath)
    {
      this.asset = asset;
      this.component = component;
      this.propertyPath = propertyPath;
    }

    // Return the string representation of the search result
    public override string ToString()
    {
      return string.Join(" › ", Regex.Replace(propertyPath, @".Array.data\[(\d+)\]", m => $"[{m.Groups[1].Value}]")
        .Split('.')
        .Select(s => ObjectNames.NicifyVariableName(s)));
    }
  }
}