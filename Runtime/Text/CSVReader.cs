using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Audune.Utils.Text
{
  // Class that defines a CSV reader for record data
  public sealed class CSVReader<TRecord> : RecordReader<TRecord>
  {
    // Patterns for parsing
    private static readonly string splitPattern = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    private static readonly char[] trimChars = { '\"' };


    // The deserializer for the reader
    private readonly IRecordDeserializer<TRecord> _deserializer;


    // Constructor
    public CSVReader(IRecordDeserializer<TRecord> deserializer)
    {
      _deserializer = deserializer;
    }


    // Read CSV data to a list of records
    public override IEnumerable<TRecord> Read(TextReader textReader)
    {
      var headerLine = textReader.ReadLine() ?? throw new FormatException("The CSV file does not contain a header");
      var header = Regex.Split(headerLine, splitPattern);

      string line;
      while ((line = textReader.ReadLine()) != null)
      {
        var values = Regex.Split(line, splitPattern);
        if (values.Length == 0 || values[0] == "")
          continue;

        var data = new Dictionary<string, object>();
        for (var valueIndex = 0; valueIndex < header.Length && valueIndex < values.Length; valueIndex++)
        {
          var key = header[valueIndex];
          var value = values[valueIndex].TrimStart(trimChars).TrimEnd(trimChars).Replace("\\", "");

          if (bool.TryParse(value, out var boolValue))
            data[key] = boolValue;
          else if (int.TryParse(value, out var intValue))
            data[key] = intValue;
          else if (float.TryParse(value, out var floatValue))
            data[key] = floatValue;
          else
            data[key] = value;
        }

        yield return _deserializer.Deserialize(data);
      }
    }
  }
}