using System;
using UnityEngine;

namespace Audune.Utils
{
  // Attribute that defines options for drawing a type reference
  public class TypeReferenceAttribute : PropertyAttribute
  {
    // The type of which the child types are drawn
    public Type baseType { get; set; }

    // The display string options for the type
    public TypeDisplayOptions displayOptions { get; set; }


    // Constructor
    public TypeReferenceAttribute(Type baseType, TypeDisplayOptions displayOptions = TypeDisplayOptions.None)
    {
      if (baseType == null) 
        throw new ArgumentNullException(nameof(baseType));

      this.baseType = baseType;
      this.displayOptions = displayOptions;
    }
  }
}