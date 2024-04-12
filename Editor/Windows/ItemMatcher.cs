using System;
using System.Collections.Generic;
using System.Linq;

namespace Audune.Utils.UnityEditor.Editor
{
  // Delegate that returns if a data item matches a search string
  public delegate bool ItemMatcher<TItem>(TItem data, string searchString);


  // Class that defines utility methods for item matcher
  public static class ItemMatcher
  {
    // Class that defines a key for matching items
    public sealed class Key<TItem>
    {
      // The key of the type
      public readonly string key;

      // The matcher of the type
      public readonly ItemMatcher<TItem> matcher;


      // Constructor
      public Key(string key, ItemMatcher<TItem> matcher)
      {
        this.key = key;
        this.matcher = matcher;
      }
    }


    #region Matching strings
    // Return a matcher that checks if any of the selected strings of an item matches the search string
    public static ItemMatcher<TItem> String<TItem>(Func<TItem, IEnumerable<string>> selector)
    {
      return (data, searchString) => {
        // Iterate over the selected strings
        foreach (var dataString in selector(data))
        {
          // Check if the selected string is empty
          if (string.IsNullOrEmpty(dataString))
            continue;

          // Check which comparison to use based on the start of the search string
          if (searchString.StartsWith("=") && dataString.Equals(searchString[1..]))
            return true;
          else if (dataString.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        }

        // No selected string matched
        return false;
      };
    }

    // Return a matcher that checks if any of the selected strings of an item matches the search string in the params array
    public static ItemMatcher<TItem> String<TItem>(params Func<TItem, string>[] selectors)
    {
      return String<TItem>(item => selectors.Select(selector => selector(item)));
    }

    // Return a matcher that checks if the string item matches the search string in the params array
    public static ItemMatcher<string> String()
    {
      return String<string>(data => data);
    }
    #endregion

    #region Matching compound matchers
    // Return a matcher that checks if the item matches any of the specified matchers
    public static ItemMatcher<TItem> Any<TItem>(IEnumerable<ItemMatcher<TItem>> matchers)
    {
      return (data, searchString) => matchers.Any(matcher => matcher(data, searchString));
    }

    // Return a matcher that checks if the item matches any of the specified matchers in the params array
    public static ItemMatcher<TItem> Any<TItem>(params ItemMatcher<TItem>[] matchers)
    {
      return Any((IEnumerable<ItemMatcher<TItem>>)matchers);
    }

    // Return a matcher that checks if the item matches all of the specified matchers
    public static ItemMatcher<TItem> All<TItem>(IEnumerable<ItemMatcher<TItem>> matchers)
    {
      return (data, searchString) => matchers.All(matcher => matcher(data, searchString));
    }

    // Return a matcher that checks if the item matches all of the specified matchers in the params array
    public static ItemMatcher<TItem> All<TItem>(params ItemMatcher<TItem>[] matchers)
    {
      return All((IEnumerable<ItemMatcher<TItem>>)matchers);
    }

    // Return a matcher that checks if the item matches the specified keys
    public static ItemMatcher<TItem> Keys<TItem>(IEnumerable<(string key, ItemMatcher<TItem> matcher)> keys)
    {
      return (data, searchString) => {
        // Check if there is a key that matches
        foreach (var keys in keys)
        {
          // Check if the key matches th search string
          if (!searchString.StartsWith($"{keys.key}:"))
            continue;

          // Chect if the type matches for the data item
          return keys.matcher(data, searchString[(keys.key.Length + 1)..].TrimStart());
        }

        // No types matched, so check if any of them match
        return keys.Any(type => type.matcher(data, searchString));
      };
    }

    // Return a matcher that checks if the item matches the specified keys in the params array
    public static ItemMatcher<TItem> Keys<TItem>(params (string key, ItemMatcher<TItem> matcher)[] keys)
    {
      return Keys((IEnumerable<(string key, ItemMatcher<TItem> matcher)>)keys);
    }
    #endregion

    #region Trimming keys from search strings
    // Return a string which has the specified matcher keys trimmed from the start
    public static string TrimKeys(string input, string[] keys, out string trimmedKey)
    {
      // Iterate over the keys
      foreach (var key in keys)
      {
        // Check if the input starts with the key
        if (input.StartsWith($"{key}:"))
        {
          // Set the trimmed key and return the input
          trimmedKey = key;
          return input[(key.Length + 1)..].TrimStart();
        }
      }

      // No keys were trimmed
      trimmedKey = null;
      return input;
    }
    #endregion
  }
}