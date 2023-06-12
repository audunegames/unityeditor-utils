using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Audune.Utils.Text
{
  // Class that defines a reader for record data
  public abstract class RecordReader<TRecord>
  {
    // Read from a text reader
    public abstract IEnumerable<TRecord> Read(TextReader textReader);


    // Read from a file with the specified encoding
    public IEnumerable<TRecord> Read(string path, Encoding encoding)
    {
      using var textReader = new StreamReader(path, encoding);
      foreach (var record in Read(textReader))
        yield return record;
    }

    // Read from a file
    public IEnumerable<TRecord> Read(string path)
    {
      using var textReader = new StreamReader(path);
      foreach (var record in Read(textReader))
        yield return record;
    }
  }
}