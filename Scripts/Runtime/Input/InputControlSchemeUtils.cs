using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine.InputSystem;

namespace Audune.Utils.Input
{
  // Class that defines utility methods for control schemes and controls
  public static class InputControlSchemeUtils
  {
    #region Getting avaiable control schemes
    // Return all control schemes of an input action asset that are available based on the specified devices
    public static IEnumerable<RuntimeControlScheme> GetAvailableControlSchemes<TDevices>(this InputActionAsset actionAsset, TDevices devices, bool includeMostSpecificOnly = false) where TDevices : IReadOnlyList<InputDevice>
    {
      // Get the control schemes that are available based on the devices
      var availableControlSchemes = new List<(RuntimeControlScheme scheme, float score)>();
      foreach (var controlScheme in actionAsset.controlSchemes)
      {
        using var result = controlScheme.PickDevicesFrom(devices);
        if (result.isSuccessfulMatch)
          availableControlSchemes.Add((new RuntimeControlScheme(controlScheme, result.devices.ToArray()), result.score));
      }

      // Check if we want to return the most specific control schemes only
      if (includeMostSpecificOnly)
      {
        // Create a dictionary of most specific control schemes per set of devices
        var mostSpecificControlSchemes = new List<(RuntimeControlScheme scheme, float score)>();
        foreach (var tuple in availableControlSchemes)
        {
          mostSpecificControlSchemes.RemoveAll(t => tuple.scheme.devices.All(device => t.scheme.devices.Contains(device)) && t.score < tuple.score);
          mostSpecificControlSchemes.Add(tuple);
        }

        // Return the most specific control schemes
        return mostSpecificControlSchemes.Select(t => t.scheme);
      }
      else
      {
        // Return all available control schemes
        return availableControlSchemes.Select(t => t.scheme);
      }
    }

    // Return all control schemes of an input action asset that are available based on the devices connected to the input system
    public static IEnumerable<RuntimeControlScheme> GetAvailableControlSchemes(this InputActionAsset actionAsset, bool includeMostSpecificOnly = false)
    {
      return GetAvailableControlSchemes(actionAsset, InputSystem.devices, includeMostSpecificOnly);
    }
    #endregion

    #region Matching a control against available control schemes
    // Return if the specified control matches one of the available control schemes based on the specified devices and store the control scheme and a normalized control path
    public static bool MatchesAvailableControlScheme<TDevices>(this InputActionAsset actionAsset, TDevices devices, InputControl control, out RuntimeControlScheme controlScheme, out string controlPath, bool includeMostSpecificOnly = false) where TDevices : IReadOnlyList<InputDevice>
    {
      controlScheme = null;
      controlPath = null;

      // Iterate over the available control schemes
      foreach (var scheme in GetAvailableControlSchemes(actionAsset, devices, includeMostSpecificOnly))
      {
        // Iterate over the required paths of the control scheme
        foreach (var requiredPath in scheme.controlScheme.deviceRequirements.Select(requirement => requirement.controlPath))
        {
          // Check if the required path matches the control
          if (InputControlPath.MatchesPrefix(requiredPath, control))
          {
            // Set the scheme and path
            controlScheme = scheme;
            controlPath = Regex.Replace(control.path, $"/?{control.device.name}", requiredPath);
            return true;
          }
        }
      }

      return false;
    }

    // Return if the specified control matches one of the available control schemes based on the devices connected to the input system and store the control scheme and a normalized control path
    public static bool MatchesAvailableControlScheme(this InputActionAsset actionAsset, InputControl control, out RuntimeControlScheme controlScheme, out string controlPath, bool includeMostSpecificOnly = false)
    {
      return MatchesAvailableControlScheme(actionAsset, InputSystem.devices, control, out controlScheme, out controlPath, includeMostSpecificOnly);
    }
    #endregion
  }
}