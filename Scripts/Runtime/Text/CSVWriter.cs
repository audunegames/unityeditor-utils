using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Audune.Utils.Text
{
  // Class that defines a CSV writer for record data
  public sealed class CSVWriter<TRecord> : RecordWriter<TRecord>
  {
    // Patterns for parsing
    private static readonly string splitPattern = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    private static readonly char[] trimChars = { '\"' };


    // The serializer for the reader
    private readonly IRecordSerializer<TRecord> _serializer;


    // Constructor
    public CSVWriter(IRecordSerializer<TRecord> serializer)
    {
      _serializer = serializer;
    }


    // Write CSV data from a list of records
    public override void Write(IEnumerable<TRecord> records, TextWriter textWriter)
    {
      var datas = records.Select(record => _serializer.Serialize(record));
      var keys = datas.SelectMany(entry => entry.Keys).Distinct().ToList();

      textWriter.WriteLine(string.Join(",", keys));

      foreach (var data in datas)
      {
        var values = new List<string>();
        foreach (var key in keys)
        {
          if (data.TryGetValue(key, out var value))
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

        textWriter.WriteLine(string.Join(",", values));
      }
    }
  }
}