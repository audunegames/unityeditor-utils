using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Audune.Utils.UnityEditor.Editor
{
  // Class that defines a tree view for items
  public abstract class ItemsTreeView<T> : TreeView
  {
    // Default styles
    public static readonly GUIStyle label;
    public static readonly GUIStyle labelRightAligned;
    public static readonly GUIStyle boldLabel;
    public static readonly GUIStyle boldLabelRightAligned;


    // Class that defines options for an extended tree view
    public class Options
    {
      // Function to select the display name of an item
      public Func<T, string> displayNameSelector = data => data.ToString();

      // Function to select the icon of an item
      public Func<T, Texture2D> iconSelector = data => null;

      // Function to select the icon of a group item
      public Func<string[], bool, Texture2D> groupIconSelector = (path, expanded) => null;
    }


    // Class that defines a tree view item containing data
    public class DataItem : TreeViewItem
    {
      // The data held by the tree view item
      public T data;


      // Convert the item to a GUI content
      public static implicit operator GUIContent(DataItem item)
      {
        return new GUIContent(item.displayName, item.icon);
      }
    }

    // Class that defines a tree view item containing other items
    public class GroupItem : TreeViewItem
    {
      // Convert the item to a GUI content
      public static implicit operator GUIContent(GroupItem item)
      {
        return new GUIContent(item.displayName, item.icon);
      }
    }


    // Items of the tree view
    private List<T> _items;

    // Options of the tree view
    private Options _options;


    // Return the items of the tree view
    public IReadOnlyList<T> items => _items;

    // Return the options of the tree view
    public Options options => _options;

    // Return if the tree view is searching
    public bool isSearching => !string.IsNullOrEmpty(searchString);


    // Event that is triggered when an item has been selected
    public event Action<T> OnItemSelected;

    // Event that is triggered when an item has been single clicked
    public event Action<T> OnItemSingleClicked;

    // Event that is triggered when an item has been double clicked
    public event Action<T> OnItemDoubleClicked;

    // Event that is triggered when an item has been context clicked
    public event Action<T> OnItemContextClicked;


    // Constructor
    public ItemsTreeView(IEnumerable<T> items, Options options) : base(new TreeViewState())
    {
      _items = items.ToList();
      _options = options;

      rowHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
      showAlternatingRowBackgrounds = true;
    }

    // Static constructor
    static ItemsTreeView()
    {
      label = new GUIStyle(EditorStyles.label);
      label.alignment = TextAnchor.MiddleLeft;
      label.richText = true;
      label.padding.left = 2;
      label.padding.right = 2;

      labelRightAligned = new GUIStyle(label);
      labelRightAligned.alignment = TextAnchor.MiddleRight;

      boldLabel = new GUIStyle(label);
      boldLabel.fontStyle = FontStyle.Bold;

      boldLabelRightAligned = new GUIStyle(label);
      boldLabelRightAligned.fontStyle = FontStyle.Bold;
      boldLabelRightAligned.alignment = TextAnchor.MiddleRight;
    }


    // Build the items of the tree view
    protected virtual void Build(ref TreeViewItem rootItem, ref int id)
    {
      // Iterate over the items and create data items for them
      foreach (var item in items)
      {
        // Create the data item
        var dataItem = CreateDataItem(id++, item);

        // Add the data item to the root item
        rootItem.AddChild(dataItem);
      }
    }

    // Draw a cell that represents a data item in the tree view
    protected abstract void OnDataCellGUI(DataItem item, int columnIndex, Rect columnRect);

    // Draw a cell that represents a group item in the tree view
    protected abstract void OnGroupCellGUI(GroupItem item, int columnIndex, Rect columnRect);

    // Handle when a data item has been single clicked
    protected virtual void OnSingleClicked(DataItem item) { }

    // Handle when a data item has been double clicked
    protected virtual void OnDoubleClicked(DataItem item) { }

    // Return a context menu for a data item
    protected virtual GenericMenu GetDataItemContextMenu(DataItem item)
    {
      var menu = new GenericMenu();
      menu.AddItem(new GUIContent("Expand All"), false, () => ExpandAll());
      menu.AddItem(new GUIContent("Collapse All"), false, () => CollapseAll());
      return menu;
    }

    // Return a context menu for a group item
    protected virtual GenericMenu GetGroupItemContextMenu(GroupItem item)
    {
      var expanded = GetExpanded().Contains(item.id);

      var menu = new GenericMenu();
      menu.AddItem(new GUIContent(expanded ? "Collapse" : "Expand"), false, () => SetExpanded(item.id, !expanded));
      menu.AddItem(new GUIContent("Expand Children"), false, () => SetExpandedRecursive(item.id, true));
      menu.AddSeparator("");
      menu.AddItem(new GUIContent("Expand All"), false, () => ExpandAll());
      menu.AddItem(new GUIContent("Collapse All"), false, () => CollapseAll());
      return menu;
    }

    // Return if the data of an item matches the specified search query
    protected abstract bool Matches(T data, string search);


    // Enumerate over all tree view items in an item recursively
    public IEnumerable<TreeViewItem> GetItemsRecursive(TreeViewItem item, Func<TreeViewItem, bool> predicate = null)
    {
      if (predicate == null || predicate(item))
        yield return item;

      if (item.hasChildren)
      {
        foreach (var childItem in item.children.SelectMany(child => GetItemsRecursive(child, predicate)))
          yield return childItem;
      }
    }

    // Enumerate over all tree view items recursively
    public IEnumerable<TreeViewItem> GetItemsRecursive(Func<TreeViewItem, bool> predicate = null)
    {
      return GetItemsRecursive(rootItem, predicate);
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
      var item = GetItemsRecursive()
        .Where(item => item is DataItem dataItem && Equals(dataItem.data, data))
        .FirstOrDefault();

      if (item != default)
        SetSelection(new[] { item.id }, TreeViewSelectionOptions.RevealAndFrame);
    }

    // Highlight the search string in the specified string
    protected virtual string HighlightSearchString(string text)
    {
      if (isSearching)
        return Regex.Replace(text, Regex.Escape(searchString), m => $"<b><color=blue>{m.Value}</color></b>", RegexOptions.IgnoreCase);
      else
        return text;
    }

    // Highlight the search string in the specified GUI content
    protected GUIContent HighlightSearchString(GUIContent content)
    {
      if (isSearching)
        return new GUIContent(HighlightSearchString(content.text), content.image, content.tooltip);
      else
        return content;
    }


    #region Tree view method overrides
    // Build the tree data and return the root item of the tree
    protected override TreeViewItem BuildRoot()
    {
      var id = 0;
      var rootItem = new TreeViewItem(id++, -1);
      Build(ref rootItem, ref id);
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

    // Handler for when an item is selected
    protected override void SelectionChanged(IList<int> ids)
    {
      if (ids.Count == 0)
        return;

      var itemId = ids.First();
      var item = FindItem(itemId, rootItem);

      if (item is DataItem dataItem)
        OnItemSelected?.Invoke(dataItem.data);
    }

    // Handler for when an item is single clicked
    protected override void SingleClickedItem(int id)
    {
      var item = FindItem(id, rootItem);
      if (item is DataItem dataItem)
      {
        OnItemSingleClicked?.Invoke(dataItem.data);
        OnSingleClicked(dataItem);
      }
      else if (item is GroupItem groupItem)
      {
        var expanded = GetExpanded().Contains(groupItem.id);
        SetExpanded(groupItem.id, !expanded);
      }
    }

    // Handler for when an item is double clicked
    protected override void DoubleClickedItem(int id)
    {
      var item = FindItem(id, rootItem);
      if (item is DataItem dataItem)
      {
        OnItemDoubleClicked?.Invoke(dataItem.data);
        OnDoubleClicked(dataItem);
      }
    }

    // Handler for when an item is context clicked
    protected override void ContextClickedItem(int id)
    {
      var item = FindItem(id, rootItem);
      if (item is DataItem dataItem)
      {
        OnItemContextClicked?.Invoke(dataItem.data);
        GetDataItemContextMenu(dataItem)?.ShowAsContext();
      }
      else if (item is GroupItem groupItem)
      {
        GetGroupItemContextMenu(groupItem)?.ShowAsContext();
      }
    }

    // Return if an item matches a search query
    protected override bool DoesItemMatchSearch(TreeViewItem item, string search)
    {
      return item is DataItem dataItem && Matches(dataItem.data, search);
    }
    #endregion

    #region Creating tree view items
    // Create a data item from a data object
    public DataItem CreateDataItem(int id, T data, string displayName = null, Texture2D icon = null)
    {
      return new DataItem { 
        id = id, 
        data = data, 
        displayName = displayName ?? options.displayNameSelector(data), 
        icon = icon != null ? icon : options.iconSelector(data),
      };
    }

    // Create a group item with the specified path and display name
    public GroupItem CreateGroupItem(int id, string[] path, string displayName = null, Texture2D icon = null)
    {
      return new GroupItem { 
        id = id,
        displayName = displayName ?? path[^1], 
        icon = icon != null ? icon : options.groupIconSelector(path, GetExpanded().Contains(id)),
      };
    }
    #endregion
  }
}