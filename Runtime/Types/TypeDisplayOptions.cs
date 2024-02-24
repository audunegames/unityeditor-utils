using System;

namespace Audune.Utils.UnityEditor
{
  // Enum that defines options for displaying the name of a type
  [Flags]
  public enum TypeDisplayOptions
  {
    None = 0,
    DontUseNicifiedNames = 1 << 0,
    DontShowNamespace = 1 << 1,
    IgnoreDisplayNames = 1 << 2,
  }
}
