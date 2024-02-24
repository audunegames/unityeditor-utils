using System;

namespace Audune.Utils.UnityEditor
{
  // Attribute that defines a display name for the type of a class
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
  public class TypeDisplayNameAttribute : Attribute
  {
    // The name to display for the serialized type
    public string name { get; set; }


    // Constructor
    public TypeDisplayNameAttribute(string name)
    {
      this.name = name;
    }
  }
}