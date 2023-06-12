using System;
using UnityEngine;

namespace Audune.Utils.Unity
{
  // Attribute that defines options for drawing a type reference
  public class TypeReferenceAttribute : PropertyAttribute
  {
    // The type of which the child types are drawn
    public Type baseType { get; set; }

    // The display string options for the type
    public TypeDisplayStringOptions displayStringOptions { get; set; }


    // Constructor
    public TypeReferenceAttribute(Type baseType)
    {
      if (baseType == null) 
        throw new ArgumentNullException(nameof(baseType));

      this.baseType = baseType;
    }
  }
}