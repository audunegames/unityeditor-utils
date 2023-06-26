using System;
using UnityEngine;

namespace Audune.Utils
{
  // Enum that defines when a component should execute
  [Flags]
  public enum ExecutionMode
  {
    [Tooltip("Never execute")]
    Never = 0,
    [Tooltip("Execute when playing in a standalone player with a development build")]
    InDevelopmentBuild = 1 << 0,
    [Tooltip("Execute when playing in a standalone player with a release build")]
    InReleaseBuild = 1 << 1,
    [Tooltip("Execute when playing in a standalone player")]
    InBuild = InReleaseBuild | InDevelopmentBuild,
    [Tooltip("Execute when playing in the editor")]
    InEditor = 1 << 2,
    [Tooltip("Always execute")]
    Always = InReleaseBuild | InDevelopmentBuild | InEditor,
  }
}