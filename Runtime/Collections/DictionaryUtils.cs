using System;
using System.Collections.Generic;
using System.Linq;

namespace Audune.Utils
{
  // Class that defines utility methods for dictionaries
  public static class DictionaryUtils
  {
    // Project each key of an enumerable of key-value pairs into a new form
    public static IEnumerable<KeyValuePair<TKeyResult, TValue>> SelectKey<TKey, TValue, TKeyResult>(this IEnumerable<KeyValuePair<TKey, TValue>> enumerable, Func<TKey, TKeyResult> keySelector)
    {
      if (enumerable == null)
        throw new ArgumentNullException(nameof(enumerable));
      if (keySelector == null)
        throw new ArgumentNullException(nameof(keySelector));

      return enumerable.ToDictionary(e => keySelector(e.Key), e => e.Value);
    }

    // Project each value of an enumerable of key-value pairs into a new form
    public static IEnumerable<KeyValuePair<TKey, TValueResult>> SelectValue<TKey, TValue, TValueResult>(this IEnumerable<KeyValuePair<TKey, TValue>> enumerable, Func<TValue, TValueResult> valueSelector)
    {
      if (enumerable == null)
        throw new ArgumentNullException(nameof(enumerable));
      if (valueSelector == null)
        throw new ArgumentNullException(nameof(valueSelector));

      return enumerable.ToDictionary(e => e.Key, e => valueSelector(e.Value));
    }


    // Convert an enumerable of key-value pairs to a dictionary
    public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> enumerable)
    {
      if (enumerable == null)
        throw new ArgumentNullException(nameof(enumerable));

      return enumerable.ToDictionary(e => e.Key, e => e.Value);
    }
  }
}