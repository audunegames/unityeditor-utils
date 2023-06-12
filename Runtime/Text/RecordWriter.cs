using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Audune.Utils.Text
{
  // Class that defines a writer for record data
  public abstract class RecordWriter<TRecord>
  {
    // Write to a text writer
    public abstract void Write(IEnumerable<TRecord> records, TextWriter textWriter);


    // Write to a file with the specified encoding
    public void Write(IEnumerable<TRecord> records, string path, Encoding encoding)
    {
      using var textWriter = new StreamWriter(path, false, encoding);
      Write(records, textWriter);
    }

    // Write to a file
    public void Write(IEnumerable<TRecord> records, string path)
    {
      using var textWriter = new StreamWriter(path, false);
      Write(records, textWriter);
    }
  }
}