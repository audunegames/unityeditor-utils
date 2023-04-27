using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.InputSystem;

namespace Audune.Utils.Input
{
  // Class that defines a group of bindings at runtime
  public class BindingGroup : IGrouping<string, Binding>, IEquatable<BindingGroup>
  {
    // Binding group values
    public readonly InputAction action;
    public readonly InputControlScheme controlScheme;
    public readonly string partOfCompositeName;
    public readonly List<Binding> bindings;


    // Return the key of the grouping
    public string Key => partOfCompositeName;

    // Return the name of the binding group
    public string Name {
      get {
        var actionName = action.actionMap != null && !string.IsNullOrEmpty(action.actionMap.name) ? $"{action.actionMap.name}/{action.name}" : action.name;
        return !string.IsNullOrEmpty(partOfCompositeName) ? $"{actionName}/{partOfCompositeName}" : actionName;
      }
    }


    // Constructor
    public BindingGroup(InputAction action, InputControlScheme controlScheme, string partOfCompositeName, IEnumerable<Binding> bindings)
    {
      this.action = action;
      this.controlScheme = controlScheme;
      this.partOfCompositeName = partOfCompositeName;
      this.bindings = bindings.ToList();
    }


    // Return a generic enumerator of the grouping
    public IEnumerator<Binding> GetEnumerator()
    {
      return bindings.GetEnumerator();
    }

    // Return an enumerator of the grouping
    IEnumerator IEnumerable.GetEnumerator()
    {
      return bindings.GetEnumerator();
    }


    // Return if the binding composite equals another object
    public override bool Equals(object obj)
    {
      return obj is BindingGroup bindingComposite && Equals(bindingComposite);
    }

    // Return if the binding composite equals another binding composite
    public bool Equals(BindingGroup other)
    {
      return EqualityComparer<InputAction>.Default.Equals(action, other.action) &&
        controlScheme.Equals(other.controlScheme) &&
        partOfCompositeName == other.partOfCompositeName &&
        EqualityComparer<List<Binding>>.Default.Equals(bindings, other.bindings);
    }

    // Return the hash code of the binding composite
    public override int GetHashCode()
    {
      return HashCode.Combine(action, controlScheme, partOfCompositeName, bindings);
    }

    // Return the string representation of the binding composite
    public override string ToString()
    {
      var str = action.actionMap != null && !string.IsNullOrEmpty(action.actionMap.name) ? $"{action.actionMap.name}/{action.name}" : action.name;
      if (!string.IsNullOrEmpty(partOfCompositeName))
        str += $"/{partOfCompositeName}";
      return str;
    }


    #region Equality operators
    public static bool operator ==(BindingGroup left, object right) => left.Equals(right);
    public static bool operator !=(BindingGroup left, object right) => !(left == right);
    #endregion
  }
}
