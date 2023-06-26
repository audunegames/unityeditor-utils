using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Audune.Utils.Editor
{
  // Class that defines a tree view for a search editor window
  public abstract class SearchTreeView<T> : TreeView
  {
    // Class that defines options for a search tree view
    public class Options
    {
      // Function to select the path of an item
      public Func<T, string> pathSelector = data => data.ToString();

      // Function to select the display name of an item
      public Func<T, string> displayNameSelector = data => data.ToString();


      // Indicate if a default item should be added
      public bool addDefaultItem = false;

      // Data of the default item
      public T defaultItemData = default;

      // Display name of the default item
      public string defaultItemDisplayName = "<None>";
    }


    // Class that defines a tree view item containing data
    protected class DataItem : TreeViewItem
    {
      // The data held by the tree view item
      public T data;


      // Create a data item from an object
      public static DataItem CreateFromData(int id, T data, Options options)
      {
        return new DataItem { id = id, data = data, displayName = options.displayNameSelector(data) };
      }

      // Create a data item from a detault object
      public static DataItem CreateFromDefaultData(int id, Options options)
      {
        return new DataItem { id = id, data = options.defaultItemData, displayName = options.defaultItemDisplayName };
      }
    }


    // Class that defines a tree view item containing other items
    protected class GroupItem : TreeViewItem
    {
      // Create a group item with the specified display name
      public static GroupItem Create(int id, string displayName)
      {
        return new GroupItem { id = id, displayName = displayName };
      }
    }


    // Items of the tree view
    protected List<T> _items;

    // Options of the tree view
    protected Options _options;


    // Event that is triggered when an item has been selected
    public event Action<T> onItemSelected;

    // Event that is triggered when an item has been double clicked
    public event Action<T> onItemDoubleClicked;


    // Constructor
    public SearchTreeView(IEnumerable<T> items, Options options) : base(new TreeViewState())
    {
      _items = items.ToList();
      _options = options;
    }

    // Static constructor
    static SearchTreeView()
    {
      DefaultStyles.foldoutLabel.richText = true;
      DefaultStyles.foldoutLabel.fontStyle = FontStyle.Bold;

      DefaultStyles.label.richText = true;
      DefaultStyles.labelRightAligned.richText = true;
      DefaultStyles.boldLabel.richText = true;
      DefaultStyles.boldLabelRightAligned.richText = true;
    }


    // Build the tree data and return the root item of the tree
    protected override TreeViewItem BuildRoot()
    {
      var index = 0;

      // Create a root item
      var rootItem = new TreeViewItem(++index, -1);

      // Create a none item
      if (_options.addDefaultItem)
        rootItem.AddChild(DataItem.CreateFromDefaultData(++index, _options));

      // Iterate over the items and create items for them
      var pathItems = new Dictionary<string, GroupItem>();
      foreach (var item in _items) 
      {
        var dataItem = DataItem.CreateFromData(++index, item, _options);

        var pathComponents = _options.pathSelector(item).Split('/');
        var pathItem = rootItem;
        for (int i = 0; i < pathComponents.Length - 1; i++)
        {
          var path = string.Join("/", pathComponents[..(i + 1)]);
          if (!pathItems.TryGetValue(path, out var newPathItem))
          {
            newPathItem = GroupItem.Create(++index, pathComponents[i]);
            pathItem.AddChild(newPathItem);
            pathItems.Add(path, newPathItem);
          }
          pathItem = newPathItem;
        }

        pathItem.AddChild(dataItem);
      }

      // Set the depths and return the root item
      SetupDepthsFromParentsAndChildren(rootItem);
      return rootItem;
    }

    // Draw the rows
    protected override void RowGUI(RowGUIArgs args)
    {
      for (var i = 0; i < args.GetNumVisibleColumns(); i++)
      {
        if (args.item is DataItem dataItem)
          OnDataCellGUI(dataItem, args.GetColumn(i), args.GetCellRect(i));
        else if (args.item is GroupItem groupItem)
          OnGroupCellGUI(groupItem, args.GetColumn(i), args.GetCellRect(i));
      }
    }

    // Return if an item matches a search query
    protected override bool DoesItemMatchSearch(TreeViewItem item, string search)
    {
      return item is DataItem dataItem && Matches(dataItem.data, search);
    }

    // Handler for when an item is selected
    protected override void SelectionChanged(IList<int> ids)
    {
      if (ids.Count == 0)
        return;

      var itemId = ids.First();
      var item = FindItem(itemId, rootItem);

      if (item is DataItem dataItem)
        onItemSelected?.Invoke(dataItem.data);
      else if (item is not null)
        SetExpanded(itemId, true);
    }

    // Handler for when an item is double clicked
    protected override void DoubleClickedItem(int id)
    {
      SingleClickedItem(id);

      var item = FindItem(id, rootItem);
      if (item is DataItem dataItem)
        onItemDoubleClicked?.Invoke(dataItem.data);
    }


    // Enumerate over all child view items recursively
    protected IEnumerable<TreeViewItem> GetItems(TreeViewItem item)
    {
      return Enumerable.Repeat(item, 1).Concat(item.hasChildren ? item.children.SelectMany(child => GetItems(child)) : Enumerable.Empty<TreeViewItem>());
    }

    // Enumerate over all tree view items recursively
    public IEnumerable<TreeViewItem> GetItems()
    {
     return GetItems(rootItem);
    }

    // Get the data of the selected item
    public T GetSelectionData()
    {
      var ids = GetSelection();
      if (ids.Count == 0)
        return default;

      var item = FindItem(ids.First(), rootItem);
      if (item is DataItem dataItem)
        return dataItem.data;
      else
        return default;
    }

    // Selet the data item with the specified data
    public void SetSelectionData(T data)
    {
      var item = GetItems()
        .Where(item => item is DataItem dataItem && Equals(dataItem.data, data))
        .FirstOrDefault();

      if (item != default)
        SetSelection(new [] { item.id }, TreeViewSelectionOptions.RevealAndFrame);
    }

    // Highlight the search string in an input string
    public string HighlightSearchString(string input)
    {
      return Regex.Replace(input, Regex.Escape(searchString), m => $"<b><color=blue>{m.Value}</color></b>", RegexOptions.IgnoreCase);
    }


    // Return if an item matches the specified search query
    protected abstract bool Matches(T item, string search);

    // Draw a cell that represents a data item in the tree view
    protected abstract void OnDataCellGUI(DataItem item, int columnIndex, Rect columnRect);

    // Draw a cell that represents a group item in the tree view
    protected abstract void OnGroupCellGUI(GroupItem item, int columnIndex, Rect columnRect);
  }
}