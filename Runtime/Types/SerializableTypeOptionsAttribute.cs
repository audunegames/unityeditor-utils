using System;
using UnityEngine;

namespace Audune.Utils.UnityEditor
{
  // Attribute that defines options for drawing a serializable type
  [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
  public class SerializableTypeOptionsAttribute : PropertyAttribute
  {
    // The type of which the child types are drawn
    public Type baseType { get; set; }

    // The display string options for the type
    public TypeDisplayOptions displayOptions { get; set; }


    // Constructor
    public SerializableTypeOptionsAttribute(Type baseType, TypeDisplayOptions displayOptions = TypeDisplayOptions.None)
    {
      if (baseType == null) 
        throw new ArgumentNullException(nameof(baseType));

      this.baseType = baseType;
      this.displayOptions = displayOptions;
    }
  }
}