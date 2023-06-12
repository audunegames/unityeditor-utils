using System.Collections.Generic;

namespace Audune.Utils.Text
{
  // Interface that defines a deserializer for record data
  public interface IRecordDeserializer<TRecord>
  {
    // Deserialize the record
    TRecord Deserialize(Dictionary<string, object> data);
  }
}