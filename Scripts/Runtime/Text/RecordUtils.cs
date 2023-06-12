using System;
using System.Collections.Generic;

namespace Audune.Utils.Text
{
  // Class that defines utility methods for records
  public static class RecordUtils
  {
    // Try to get the value from an object dictionary as a bool
    public static bool TryGetValueAsBool(this Dictionary<string, object> data, string key, out bool boolValue)
    {
      var result = data.TryGetValue(key, out var value) && value is bool;
      boolValue = result ? Convert.ToBoolean(value) : default;
      return result;
    }

    // Try to get the value from an object dictionary as an int
    public static bool TryGetValueAsInt(this Dictionary<string, object> data, string key, out int intValue)
    {
      var result = data.TryGetValue(key, out var value) && value is int;
      intValue = result ? Convert.ToInt32(value) : default;
      return result;
    }

    // Try to get the value from an object dictionary as a float
    public static bool TryGetValueAsFloat(this Dictionary<string, object> data, string key, out float floatValue)
    {
      var result = data.TryGetValue(key, out var value) && (value is float || value is int);
      floatValue = result ? Convert.ToSingle(value) : default;
      return result;
    }

    // Try to get the value from an object dictionary as a string
    public static bool TryGetValueAsString(this Dictionary<string, object> data, string key, out string stringValue)
    {
      var result = data.TryGetValue(key, out var value) && value is string;
      stringValue = result ? Convert.ToString(value) : default;
      return result;
    }
  }
}