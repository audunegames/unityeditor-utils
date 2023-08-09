using System;
using System.Collections.Generic;
using UnityEditor;

namespace Audune.Utils.Editor
{
  // Class that defines utility methods for serialized properties in the editor
  public static class EditorSerializedPropertyUtils
  {
    // Return an enumerable over the array elements of the property
    public static IEnumerable<SerializedProperty> GetArrayElements(this SerializedProperty serializedProperty)
    {
       for (var i = 0; i < serializedProperty.arraySize; i++)
         yield return serializedProperty.GetArrayElementAtIndex(i);
    }


    // Return an enumerable over the children of the property
    public static IEnumerable<SerializedProperty> GetChildren(this SerializedProperty serializedProperty, Predicate<SerializedProperty> predicate)
    {
      var childProperty = serializedProperty.Copy();

      if (!childProperty.NextVisible(true))
        yield break;

      while (childProperty.NextVisible(false))
      {
        if (predicate(childProperty))
          yield return childProperty.Copy();
      }
    }

    // Return an enumerable over the children of the object
    public static IEnumerable<SerializedProperty> GetChildren(this SerializedObject serializedObject, Predicate<SerializedProperty> predicate)
    {
      return GetChildren(serializedObject.GetIterator(), predicate);
    }
  }
}