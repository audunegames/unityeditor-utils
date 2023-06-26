using UnityEngine;

namespace Audune.Utils
{
  // Class that defines utility methods for execution-related tasks
  public static class ExecutionUtils
  {
    // Return if the application is playing in a standalone player with a release build
    public static bool IsReleaseBuild()
    {
      return !Application.isEditor && !Debug.isDebugBuild;
    }

    // Return if the application is playing in a standalone player with a development build
    public static bool IsDevelopmentBuild()
    {
      return !Application.isEditor && Debug.isDebugBuild;
    }

    // Return if the application is playing in the editor
    public static bool IsEditor()
    {
      return Application.isEditor;
    }


    // Return if an execution mode should execute based on the current Unity environment
    public static bool ShouldExecute(this ExecutionMode mode)
    {
      if (mode.HasFlag(ExecutionMode.InReleaseBuild) && IsReleaseBuild())
        return true;
      else if (mode.HasFlag(ExecutionMode.InDevelopmentBuild) && IsDevelopmentBuild())
        return true;
      else if (mode.HasFlag(ExecutionMode.InEditor) && IsEditor())
        return true;
      else
        return false;
    }
  }
}