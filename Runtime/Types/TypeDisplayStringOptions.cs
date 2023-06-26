using System;

namespace Audune.Utils.Types
{
  // Enum that defines flags for type display name options
  [Flags]
  public enum TypeDisplayStringOptions
  {
    None = 0,
    DontUseNicifiedNames = 1 << 0,
    DontShowNamespace = 1 << 1,
  }
}
