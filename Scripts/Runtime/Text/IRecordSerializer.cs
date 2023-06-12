using System.Collections.Generic;

namespace Audune.Utils.Text
{
  // Interface that defines a serializer for record data
  public interface IRecordSerializer<TRecord>
  {
    // Deserialize the record
    Dictionary<string, object> Serialize(TRecord record);
  }
}