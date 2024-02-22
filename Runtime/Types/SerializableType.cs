using System;
using System.Collections.Generic;
using UnityEngine;

namespace Audune.Utils.UnityEditor
{
  // Class that defines a serializable type
  [Serializable]
  public sealed class SerializableType : IEquatable<SerializableType>
  {
    // The fully qualified name of the type for serialization
    [SerializeField]
    private string _typeName;


    // Return the referenced type of the reference
    public Type type => Type.GetType(_typeName);


    // Constructor
    public SerializableType(Type type)
    {
      _typeName = type.AssemblyQualifiedName;
    }


    #region Equatable implementation
    // Return if the serializable type equals another object
    public override bool Equals(object obj)
    {
      return Equals(obj as SerializableType);
    }

    // Return if the serializable type equals another serializable type
    public bool Equals(SerializableType other)
    {
      return other is not null && _typeName == other._typeName;
    }

    // Return the hash code of the serializable type
    public override int GetHashCode()
    {
      return HashCode.Combine(_typeName);
    }
    #endregion

    #region Equality operators
    // Return if the serializable type equals another serializable type
    public static bool operator ==(SerializableType left, SerializableType right)
    {
      return EqualityComparer<SerializableType>.Default.Equals(left, right);
    }

    // Return if the serializable type does not equal another serializable type
    public static bool operator !=(SerializableType left, SerializableType right)
    {
      return !(left == right);
    }
    #endregion

    #region Implicit operators
    // Convert a type to a serializable type
    public static implicit operator SerializableType(Type type)
    {
      return new SerializableType(type);
    }

    // Convert a serializable type to a type
    public static implicit operator Type(SerializableType reference)
    {
      return reference.type;
    }
    #endregion
  }
}