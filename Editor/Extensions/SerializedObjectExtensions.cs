using System;
using System.Collections.Generic;
using UnityEditor;

namespace Audune.Utils.UnityEditor.Editor
{
  // Class that defines extension methods for serialized objects
  public static class SerializedObjectExtensions
  {
    #region Getting children
    // Return an enumerable over the children of the object
    public static IEnumerable<SerializedProperty> GetChildren(this SerializedObject serializedObject, bool enterChildren = false, Predicate<SerializedProperty> predicate = null)
    {
      if (serializedObject == null)
        throw new ArgumentNullException(nameof(serializedObject));

      return SerializedPropertyExtensions.GetChildren(serializedObject.GetIterator(), enterChildren, predicate);
    }
    #endregion
  }
}