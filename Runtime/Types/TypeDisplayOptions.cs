using System;

namespace Audune.Utils
{
  // Enum that defines flags for displaying the name of a type
  [Flags]
  public enum TypeDisplayOptions
  {
    None = 0,
    DontUseNicifiedNames = 1 << 0,
    DontShowNamespace = 1 << 1,
  }
}
