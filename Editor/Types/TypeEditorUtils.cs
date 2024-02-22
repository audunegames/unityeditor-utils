using System;
using UnityEditor;
using UnityEngine;
using System.Text;

namespace Audune.Utils.UnityEditor.Editor
{
  // Class that defines utility methods for types in the editor
  public static class TypeEditorUtils
  {
    #region Getting the display name of types
    // Return a display string for a type
    public static string ToDisplayString(this Type type, TypeDisplayOptions displayOptions = TypeDisplayOptions.None)
    {
      if (type == null)
        throw new ArgumentNullException(nameof(type));
      
      if (!displayOptions.HasFlag(TypeDisplayOptions.DontUseNicifiedNames))
      {
        var builder = new StringBuilder();
        builder.Append(ObjectNames.NicifyVariableName(type.Name));
        if (!displayOptions.HasFlag(TypeDisplayOptions.DontShowNamespace))
          builder.Append($" ({type.Namespace})");
        return builder.ToString();
      }
      else
      {
        if (!displayOptions.HasFlag(TypeDisplayOptions.DontShowNamespace))
          return type.FullName;
        else
          return type.Name;
      }
      
    }
    #endregion

    #region Creating generic menus for child types
    // Return a generic menu of child types of the specified base type
    public static GenericMenu CreateGenericMenuForChildTypes(this Type baseType, TypeDisplayOptions displayOptions, Type selectedType, Action<Type> onClicked)
    {
      if (baseType == null)
        throw new ArgumentNullException(nameof(baseType));

      var menu = new GenericMenu();
      foreach (var type in TypeExtensions.GetChildTypes(baseType))
        menu.AddItem(new GUIContent(ToDisplayString(type, displayOptions)), type == selectedType, () => onClicked(type));
      return menu;
    }

    // Return a generic menu of child types based on the specified attribute
    public static GenericMenu CreateGenericMenuForChildTypes(this SerializableTypeOptionsAttribute attribute, Type selectedType, Action<Type> onClicked)
    {
      if (attribute == null)
        throw new ArgumentNullException(nameof(attribute));

      var menu = new GenericMenu();
      foreach (var type in TypeExtensions.GetChildTypes(attribute.baseType))
        menu.AddItem(new GUIContent(ToDisplayString(type, attribute.displayOptions)), type == selectedType, () => onClicked(type));
      return menu;
    }
    #endregion
  }
}