using System;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Audune.Utils.UnityEditor.Editor
{
  // Class that defines a tree view for a search editor window
  public abstract class SearchTreeView<T> : ItemsTreeView<T>
  {
    // Class that defines options for a search tree view
    public new class Options : ItemsTreeView<T>.Options
    {
      // Function to select the path of an item
      public Func<T, string[]> pathSelector = data => new[] { data.ToString() };


      // Indicate if a default item should be added
      public bool addDefaultItem = false;

      // Data of the default item
      public T defaultItemData = default;

      // Display name of the default item
      public string defaultItemDisplayName = "<None>";

      // Icon of the default item
      public Texture2D defaultItemIcon = null;
    }


    // Return the options of the tree view
    public new Options options => base.options as Options;



    // Constructor
    public SearchTreeView(IEnumerable<T> items, Options options) : base(items, options)
    {
    }


    // Build the items of the tree view
    protected override void Build(ref TreeViewItem rootItem, ref int id)
    {
      // Create a default item
      if (options.addDefaultItem)
        rootItem.AddChild(CreateDefaultDataItem(id++));

      // Iterate over the items and create data items for them
      var pathItems = new Dictionary<string, GroupItem>();
      foreach (var item in items)
      {
        // Create the data item
        var dataItem = CreateDataItem(id++, item);

        // Create the path items
        var path = options.pathSelector(item);
        var pathItem = rootItem;
        for (int i = 0; i < path.Length - 1; i++)
        {
          var joinedPath = path[..(i + 1)];
          var joinedPathString = string.Join("/", path[..(i + 1)]);
          if (!pathItems.TryGetValue(joinedPathString, out var newPathItem))
          {
            newPathItem = CreateGroupItem(id++, joinedPath, path[i]);
            pathItem.AddChild(newPathItem);
            pathItems.Add(joinedPathString, newPathItem);
          }
          pathItem = newPathItem;
        }

        // Add the data item to the correct path item
        pathItem.AddChild(dataItem);
      }
    }


    #region Creating tree view items
    // Create a data item from the default object
    public DataItem CreateDefaultDataItem(int id)
    {
      return new DataItem { 
        id = id, 
        data = options.defaultItemData, 
        displayName = options.defaultItemDisplayName,
        icon = options.defaultItemIcon,
      };
    }
    #endregion
  }
}