using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;

namespace Audune.Utils.Input
{
  // Struct that defines a binding with its index at runtime
  public class Binding : IEquatable<Binding>
  {
    // Binding values
    public readonly InputAction action;
    public readonly InputControlScheme controlScheme;
    public readonly InputBinding binding;
    public readonly int bindingIndex;


    // Return the name of the binding
    public string Name {
      get {
        var actionName = action.actionMap != null && !string.IsNullOrEmpty(action.actionMap.name) ? $"{action.actionMap.name}/{action.name}" : action.name;
        return !string.IsNullOrEmpty(binding.name) ? $"{actionName}/{binding.name}" : actionName;
      }
    }


    // Constructor
    public Binding(InputAction action, InputControlScheme controlScheme, InputBinding binding, int bindingIndex)
    {
      this.action = action;
      this.controlScheme = controlScheme;
      this.binding = binding;
      this.bindingIndex = bindingIndex;
    }


    // Return if the binding equals another object
    public override bool Equals(object obj)
    {
      return obj is Binding binding && Equals(binding);
    }

    // Return if the binding equals another binding
    public bool Equals(Binding other)
    {
      return EqualityComparer<InputAction>.Default.Equals(action, other.action) &&
        controlScheme.Equals(other.controlScheme) &&
        EqualityComparer<InputBinding>.Default.Equals(binding, other.binding) &&
        bindingIndex == other.bindingIndex;
    }

    // Return the hash code of the binding
    public override int GetHashCode()
    {
      return HashCode.Combine(action, controlScheme, binding, bindingIndex);
    }

    // Return the string representation of the binding
    public override string ToString()
    {
      return Name;
    }


    #region Equality operators
    public static bool operator ==(Binding left, object right) => left.Equals(right);
    public static bool operator !=(Binding left, object right) => !(left == right);
    #endregion
  }
}
