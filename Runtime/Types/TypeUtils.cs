using System.Collections.Generic;
using System;
using System.Linq;

namespace Audune.Utils.Types
{
  // Class that defines utility methods for types
  public static class TypeUtils
  {
    // Return a list of child types of the specified base type
    public static List<Type> GetChildTypes(this Type baseType)
    {
      if (baseType == null)
        throw new ArgumentNullException(nameof(baseType));

      return AppDomain.CurrentDomain.GetAssemblies()
       .SelectMany(assembly => assembly.GetTypes())
       .Where(type => type.IsSubclassOf(baseType))
       .ToList();
    }

    // Return a list of child types based on the specified options
    public static List<Type> GetChildTypes(this TypeReferenceAttribute options)
    {
      if (options == null)
        throw new ArgumentNullException(nameof(options));

      return GetChildTypes(options.baseType);
    }
  }
}