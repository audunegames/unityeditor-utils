using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Audune.Utils.Sprites
{
  // Class that defines methods to read and write CSV files
  public static class CSVFile
  {
    // Delegates for (de)serializing records
    public delegate TRecord RecordDeserializer<TRecord>(Dictionary<string, object> entry);
    public delegate Dictionary<string, object> RecordSerializer<TRecord>(TRecord record);


    // Patterns for parsing
    private static readonly string splitPattern = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    private static readonly char[] trimChars = { '\"' };


    // Read a CSV file to a list of records
    public static IEnumerable<TRecord> Read<TRecord>(string path, RecordDeserializer<TRecord> deserializer)
    {
      var lines = File.ReadAllLines(path, Encoding.UTF8);
      if (lines.Length <= 1)
        yield break;

      var header = Regex.Split(lines[0], splitPattern);

      for (var lineIndex = 1; lineIndex < lines.Length; lineIndex++)
      {
        var line = lines[lineIndex];

        var values = Regex.Split(line, splitPattern);
        if (values.Length == 0 || values[0] == "")
          continue;

        var entry = new Dictionary<string, object>();
        for (var valueIndex = 0; valueIndex < header.Length && valueIndex < values.Length; valueIndex++)
        {
          var key = header[valueIndex];
          var value = values[valueIndex].TrimStart(trimChars).TrimEnd(trimChars).Replace("\\", "");

          if (bool.TryParse(value, out var boolValue))
            entry[key] = boolValue;
          else if (int.TryParse(value, out var intValue))
            entry[key] = intValue;
          else if (float.TryParse(value, out var floatValue))
            entry[key] = floatValue;
          else
            entry[key] = value;
        }

        yield return deserializer(entry);
      }
    }

    // Write a list of records to a CSV file
    public static void Write<TRecord>(string path, IEnumerable<TRecord> records, RecordSerializer<TRecord> serializer)
    {
      var entries = records.Select(record => serializer(record));
      var keys = entries.SelectMany(entry => entry.Keys).Distinct().ToList();

      var contents = new StringBuilder();

      contents.AppendJoin(",", keys);
      contents.AppendLine();

      foreach (var entry in entries)
      {
        var values = new List<string>();
        foreach (var key in keys)
        {
          if (entry.TryGetValue(key, out var value))
          {
            var stringValue = value switch {
              bool boolValue => boolValue ? "true" : "false",
              int intValue => intValue.ToString(CultureInfo.InvariantCulture),
              float floatValue => floatValue.ToString("G", CultureInfo.InvariantCulture),
              _ => value.ToString(),
            };

            values.Add(stringValue.Contains("\"") ? $"\"{stringValue.Replace("\"", "\"\"")}\"" : stringValue);
          }
          else
          {
            values.Add("");
          }
        }

        contents.AppendJoin(",", values);
        contents.AppendLine();
      }

      File.WriteAllText(path, contents.ToString());
    }


    // Try to get the value from an object dictionary as a bool
    public static bool TryGetValueAsBool(this Dictionary<string, object> entry, string key, out bool boolValue)
    {
      var result = entry.TryGetValue(key, out var value) && value is bool;
      boolValue = result ? Convert.ToBoolean(value) : default;
      return result;
    }

    // Try to get the value from an object dictionary as an int
    public static bool TryGetValueAsInt(this Dictionary<string, object> entry, string key, out int intValue)
    {
      var result = entry.TryGetValue(key, out var value) && value is int;
      intValue = result ? Convert.ToInt32(value) : default;
      return result;
    }

    // Try to get the value from an object dictionary as a float
    public static bool TryGetValueAsFloat(this Dictionary<string, object> entry, string key, out float floatValue)
    {
      var result = entry.TryGetValue(key, out var value) && (value is float || value is int);
      floatValue = result ? Convert.ToSingle(value) : default;
      return result;
    }

    // Try to get the value from an object dictionary as a string
    public static bool TryGetValueAsString(this Dictionary<string, object> entry, string key, out string stringValue)
    {
      var result = entry.TryGetValue(key, out var value) && value is string;
      stringValue = result ? Convert.ToString(value) : default;
      return result;
    }
  }
}