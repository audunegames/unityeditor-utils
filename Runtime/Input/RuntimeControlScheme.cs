using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;

namespace Audune.Utils.Input
{
  // Struct that defines a control scheme with its associated devices at runtime
  public class RuntimeControlScheme : IEquatable<RuntimeControlScheme>
  {
    // Control scheme values
    public readonly InputControlScheme controlScheme;
    public readonly InputDevice[] devices;


    // Constructor
    public RuntimeControlScheme(InputControlScheme controlScheme, InputDevice[] devices)
    {
      this.controlScheme = controlScheme;
      this.devices = devices;
    }


    // Return if the control scheme equals another object
    public override bool Equals(object obj)
    {
      return obj is RuntimeControlScheme scheme && Equals(scheme);
    }

    // Return if the control scheme equals another control scheme
    public bool Equals(RuntimeControlScheme other)
    {
      return controlScheme.Equals(other.controlScheme) &&
        EqualityComparer<InputDevice[]>.Default.Equals(devices, other.devices);
    }

    // Return the hash code of the control scheme
    public override int GetHashCode()
    {
      return HashCode.Combine(controlScheme, devices);
    }


    #region Equality operators
    public static bool operator ==(RuntimeControlScheme left, object right) => left.Equals(right);
    public static bool operator !=(RuntimeControlScheme left, object right) => !(left == right);
    #endregion
  }
}
