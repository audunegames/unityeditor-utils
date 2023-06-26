using System;
using System.Collections.Generic;
using UnityEngine;

namespace Audune.Utils.Types
{
  // Class that defines a serializable reference to a type
  [Serializable]
  public sealed class TypeReference : IEquatable<TypeReference>
  {
    // The fully qualified name of the type for serialization
    [SerializeField]
    private string _typeName;


    // Return the referenced type of the reference
    public Type Type => Type.GetType(_typeName);


    // Constructor
    public TypeReference(Type type)
    {
      _typeName = type.AssemblyQualifiedName;
    }


    #region Equatable implementation
    // Return if the reference equals another object
    public override bool Equals(object obj)
    {
      return Equals(obj as TypeReference);
    }

    // Return if the reference equals another reference
    public bool Equals(TypeReference other)
    {
      return other is not null && _typeName == other._typeName;
    }

    // Return the hash code of the reference
    public override int GetHashCode()
    {
      return HashCode.Combine(_typeName);
    }
    #endregion

    #region Equality operators
    // Return if the reference equals another reference
    public static bool operator ==(TypeReference left, TypeReference right)
    {
      return EqualityComparer<TypeReference>.Default.Equals(left, right);
    }

    // Return if the reference does not equal another reference
    public static bool operator !=(TypeReference left, TypeReference right)
    {
      return !(left == right);
    }
    #endregion

    #region Implicit operators
    // Convert a type to a reference
    public static implicit operator TypeReference(Type type)
    {
      return new TypeReference(type);
    }

    // Convert a reference to a type
    public static implicit operator Type(TypeReference reference)
    {
      return reference.Type;
    }
    #endregion
  }
}