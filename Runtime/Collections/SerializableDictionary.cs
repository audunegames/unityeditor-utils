using System;
using System.Collections.Generic;
using UnityEngine;

namespace Audune.Utils
{
  // Class that defines a serializable dictionary
  [Serializable]
  public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
  {
    // List of keys of the dictionary
    [SerializeField]
    private List<TKey> _keys = new List<TKey>();

    // List of values in the dictionary
    [SerializeField]
    private List<TValue> _values = new List<TValue>();


    // Constructors
    public SerializableDictionary() : base() { }
    public SerializableDictionary(IEqualityComparer<TKey> comparer) : base(comparer) { }
    public SerializableDictionary(IDictionary<TKey, TValue> dictionary) : base(dictionary) { }
    public SerializableDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer) : base(dictionary, comparer) { }


    // Callback received before Unity serializes the dictionary
    void ISerializationCallbackReceiver.OnBeforeSerialize()
    {
      _keys.Clear();
      _values.Clear();

      foreach (var e in this)
      {
        _keys.Add(e.Key);
        _values.Add(e.Value);
      }  
    }

    // Callback received after Unity deserializes the dictionary
    void ISerializationCallbackReceiver.OnAfterDeserialize()
    {
      Clear();

      if (_keys.Count != _values.Count)
        throw new InvalidOperationException($"The serialized dictionary has an invalid state ({_keys.Count} keys and {_values.Count} values)");

      for (var i = 0; i < _keys.Count; i++)
        Add(_keys[i], _values[i]);
    }
  }
}