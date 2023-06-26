using System;
using UnityEditor;
using UnityEngine;
using System.Text;
using Audune.Utils.Types;

namespace Audune.Utils.Unity.Editor
{
  // Class that defines utility methods for types in the editor
  public static class TypeEditorUtils
  {
    #region Getting the display name of types
    // Return a display string for a type
    public static string ToDisplayString(this Type type, TypeDisplayStringOptions options = TypeDisplayStringOptions.None)
    {
      if (type == null)
        throw new ArgumentNullException(nameof(type));
      
      if (!options.HasFlag(TypeDisplayStringOptions.DontUseNicifiedNames))
      {
        var builder = new StringBuilder();
        builder.Append(ObjectNames.NicifyVariableName(type.Name));
        if (!options.HasFlag(TypeDisplayStringOptions.DontShowNamespace))
          builder.Append($" ({type.Namespace})");
        return builder.ToString();
      }
      else
      {
        if (!options.HasFlag(TypeDisplayStringOptions.DontShowNamespace))
          return type.FullName;
        else
          return type.Name;
      }
      
    }
    #endregion

    #region Getting child types
    // Return a generic menu of child types of the specified base type
    public static GenericMenu CreateGenericMenuForChildTypes(this Type baseType, TypeDisplayStringOptions options, Type selectedType, Action<Type> onClicked)
    {
      if (baseType == null)
        throw new ArgumentNullException(nameof(baseType));

      var menu = new GenericMenu();
      foreach (var type in TypeUtils.GetChildTypes(baseType))
        menu.AddItem(new GUIContent(ToDisplayString(type, options)), type == selectedType, () => onClicked(type));
      return menu;
    }

    // Return a generic menu of child types based on the specified options
    public static GenericMenu CreateGenericMenuForChildTypes(this TypeReferenceAttribute options, Type selectedType, Action<Type> onClicked)
    {
      if (options == null)
        throw new ArgumentNullException(nameof(options));

      var menu = new GenericMenu();
      foreach (var type in TypeUtils.GetChildTypes(options))
        menu.AddItem(new GUIContent(ToDisplayString(type, options.displayStringOptions)), type == selectedType, () => onClicked(type));
      return menu;
    }
    #endregion
  }
}