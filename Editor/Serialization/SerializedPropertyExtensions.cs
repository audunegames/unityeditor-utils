using System;
using System.Collections.Generic;
using UnityEditor;

namespace Audune.Utils.UnityEditor.Editor
{
  // Class that defines extension methods for serialized properties
  public static class SerializedPropertyExtensions
  {
    #region Getting array elements
    // Return an enumerable over the array elements of the property
    public static IEnumerable<SerializedProperty> GetArrayElements(this SerializedProperty serializedProperty)
    {
      if (serializedProperty == null)
        throw new ArgumentNullException(nameof(serializedProperty));
      if (!serializedProperty.isArray)
        throw new ArgumentException("The serialized property is not an array", nameof(serializedProperty));

      for (var i = 0; i < serializedProperty.arraySize; i++)
        yield return serializedProperty.GetArrayElementAtIndex(i);
    }
    #endregion

    #region Getting children
    // Return an enumerable over the children of the property
    public static IEnumerable<SerializedProperty> GetChildren(this SerializedProperty serializedProperty, bool enterChildren = false, Predicate<SerializedProperty> predicate = null)
    {
      if (serializedProperty == null)
        throw new ArgumentNullException(nameof(serializedProperty));

      var childProperty = serializedProperty.Copy();

      if (!childProperty.NextVisible(true))
        yield break;

      while (childProperty.NextVisible(enterChildren))
      {
        if (predicate == null || predicate(childProperty))
          yield return childProperty.Copy();
      }
    }
    #endregion
  }
}