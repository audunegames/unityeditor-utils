using System.Collections.Generic;
using System;
using System.Linq;

namespace Audune.Utils.UnityEditor
{
  // Class that defines extension methods for types
  public static class TypeExtensions
  {
    #region Getting child types
    // Return a list of child types of the specified base type
    public static IEnumerable<Type> GetChildTypes(this Type baseType)
    {
      if (baseType == null)
        throw new ArgumentNullException(nameof(baseType));

      return AppDomain.CurrentDomain.GetAssemblies()
       .SelectMany(assembly => assembly.GetTypes())
       .Where(type => type.IsSubclassOf(baseType));
    }
  }
  #endregion
}