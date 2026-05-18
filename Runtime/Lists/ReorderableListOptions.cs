using System;

namespace Audune.Utils.UnityEditor
{
  // Enum that defines options for drawing a reorderable list
  [Flags]
  public enum ReorderableListOptions
  {
    None = 0,
    DrawFoldout = 1 << 0,
    DrawInfoField = 1 << 1,
  }
}