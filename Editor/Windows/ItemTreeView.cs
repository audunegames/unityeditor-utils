using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Audune.Utils.UnityEditor.Editor
{
  // Class that defines a tree view for displaying items
  public abstract class ItemTreeView<TItem> : TreeView
  {
    // Class that defines a column in an item tree view
    public class Column
    {
      // Delegate for drawing an item in the column
      public delegate void Drawer(Rect rect, DataItem item);


      // Function to draw an item in the column
      private readonly Drawer _drawer;

      // Header of the column
      public readonly GUIContent header;

      // Width of the column
      public readonly float width;

      // Text alignment of the column
      public readonly TextAlignment alignment;

      // Toggle to set if the column can be sorted
      public readonly bool isSortable;

      // Toggle to set if the column can be hidden from the context menu
      public readonly bool isHideable;


      // Constructor
      public Column(GUIContent header, Drawer drawer, float width = 50, TextAlignment alignment = TextAlignment.Left, bool isSortable = false, bool isHideable = false)
      {
        _drawer = drawer;

        this.header = header;
        this.width = width;
        this.alignment = alignment;
        this.isSortable = isSortable;
        this.isHideable = isHideable;
      }


      // Draw an item in the column
      internal void Draw(Rect rect, ItemTreeView<TItem>.DataItem item)
      {
        _drawer(rect, item);
      }
    }

    // Class that defines a tree view item containing data
    public class DataItem : TreeViewItem
    {
      // The data held by the tree view item
      public TItem data;


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
    private IReadOnlyList<TItem> _items = new List<TItem>();

    // Columns of the tree view
    private IReadOnlyList<Column> _columns = new List<Column>();

    // Item matcher of the tree view
    private ItemMatcher<TItem> _matcher = ItemMatcher.String<TItem>(data => data.ToString());


    // Return and set the items of the tree view
    public virtual IReadOnlyList<TItem> items {
      get => _items;
      protected set => _items = value;
    }

    // Return and set the columns of the tree view
    public virtual IReadOnlyList<Column> columns {
      get => _columns;
      protected set => _columns = value;
    }

    // Return and set the item matcher of the tree view
    public virtual ItemMatcher<TItem> matcher {
      get => _matcher;
      protected set => _matcher = value;
    }


    // Return if the tree view is searching
    public bool isSearching => !string.IsNullOrEmpty(searchString);


    // Event that is triggered when an item has been selected
    public event Action<TItem> onItemSelected;

    // Event that is triggered when an item has been single clicked
    public event Action<TItem> onItemSingleClicked;

    // Event that is triggered when an item has been double clicked
    public event Action<TItem> onItemDoubleClicked;

    // Event that is triggered when an item has been context clicked
    public event Action<TItem> onItemContextClicked;


    // Constructor
    public ItemTreeView(TreeViewState treeViewState) : base(treeViewState)
    {
    }


    // Build the items of the tree view
    protected virtual void Build(ref TreeViewItem rootItem, ref int id)
    {
      // Iterate over the items and create data items for them
      var pathItems = new Dictionary<string, GroupItem>();
      foreach (var item in items)
      {
        // Create the data item
        var dataItem = CreateDataItem(ref id, item, out var path);

        // Create the path items
        var pathItem = rootItem;
        for (int i = 0; i < path.Length - 1; i++)
        {
          var joinedPath = path[..(i + 1)];
          var joinedPathString = string.Join("/", path[..(i + 1)]);
          if (!pathItems.TryGetValue(joinedPathString, out var newPathItem))
          {
            newPathItem = CreateGroupItem(ref id, joinedPath, GetExpanded().Contains(id));
            pathItem.AddChild(newPathItem);
            pathItems.Add(joinedPathString, newPathItem);
          }
          pathItem = newPathItem;
        }

        // Add the data item to the correct path item
        pathItem.AddChild(dataItem);
      }

      // Set the tree view options
      multiColumnHeader = CreateMultiColumnHeader(_columns);
      rowHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
      showAlternatingRowBackgrounds = true;
    }

    // Create a data item
    protected DataItem CreateDataItem(ref int id, TItem data, out string[] path)
    {
      path = SelectDataPath(data);
      return new DataItem() { id = id++, data = data, displayName = SelectDataDisplayName(data), icon = SelectDataIcon(data) };
    }

    // Create a group item
    protected GroupItem CreateGroupItem(ref int id, string[] path, bool expanded)
    {
      return new GroupItem() { id = id++, displayName = SelectGroupDisplayName(path, expanded), icon = SelectGroupIcom(path, expanded) };
    }

    // Return the path for a data item
    protected virtual string[] SelectDataPath(TItem data)
    {
      return new[] { data.ToString() };
    }

    // Return the display string for a data item
    protected virtual string SelectDataDisplayName(TItem data)
    {
      return data.ToString();
    }

    // Return the icon for a data item
    protected virtual Texture2D SelectDataIcon(TItem data)
    {
      return null;
    }

    // Return the display string for a group item
    protected virtual string SelectGroupDisplayName(string[] path, bool expanded)
    {
      return path[^1];
    }

    // Return the icon for a group item
    protected virtual Texture2D SelectGroupIcom(string[] path, bool expanded)
    {
      return expanded ? EditorIcons.folderOpened : EditorIcons.folder;
    }

    // Highlight a string
    protected virtual string Highlight(string text)
    {
      return $"<b><color=#0e6dcb>{text}</color></b>";
    }


    // Draw the group GUI
    protected virtual void OnGroupGUI(Rect rect, GroupItem item)
    {
      // Draw a label for the group item
      EditorGUI.LabelField(rect, item, ItemTreeViewStyles.boldLabel);
    }

    // Handle when a data item has been single clicked
    protected virtual void OnSingleClicked(DataItem item) { }

    // Handle when a data item has been double clicked
    protected virtual void OnDoubleClicked(DataItem item) { }

    // Fill the context menu for the specified data item
    protected virtual void FillDataItemContextMenu(DataItem item, GenericMenu menu) 
    {
      // Items for expanding and collapsing the whole tree
      menu.AddItem(new GUIContent("Expand All"), false, () => ExpandAll());
      menu.AddItem(new GUIContent("Collapse All"), false, () => CollapseAll());
    }

    // Fill the context menu for the specified group item
    protected virtual void FillGroupItemContextMenu(GroupItem item, GenericMenu menu, bool expanded)
    {
      // Items for expanding and collapsing the group item
      menu.AddItem(new GUIContent(expanded ? "Collapse" : "Expand"), false, () => SetExpanded(item.id, !expanded));

      // Items for expanding the children of the group item
      menu.AddItem(new GUIContent("Expand Recursively"), false, () => SetExpandedRecursive(item.id, true));

      menu.AddSeparator("");

      // Items for expanding and collapsing the whole tree
      menu.AddItem(new GUIContent("Expand All"), false, () => ExpandAll());
      menu.AddItem(new GUIContent("Collapse All"), false, () => CollapseAll());
    }

    // Highlight the search string in the specified string
    protected string HighlightSearchString(string text, string searchString)
    {
      return isSearching ? Regex.Replace(text, Regex.Escape(searchString), m => Highlight(m.Value), RegexOptions.IgnoreCase) : text;
    }

    // Highlight the search string in the specified string with the specified required key
    protected string HighlightSearchString(string text, string searchString, string[] keys, string requiredKey)
    {
      if (!isSearching)
        return text;

      searchString = ItemMatcher.TrimKeys(searchString, keys, out var key);
      return string.IsNullOrEmpty(key) || key == requiredKey ? HighlightSearchString(text, searchString) : text;
    }

    // Highlight the search string in the specified GUI content
    protected GUIContent HighlightSearchString(GUIContent content, string searchString)
    {
      return isSearching ? new GUIContent(HighlightSearchString(content.text, searchString), content.image, content.tooltip) : content;
    }

    // Highlight the search string in the specified GUI content with the specified required key
    protected GUIContent HighlightSearchString(GUIContent content, string searchString, string[] keys, string requiredKey)
    {
      return isSearching ? new GUIContent(HighlightSearchString(content.text, searchString, keys, requiredKey), content.image, content.tooltip) : content;
    }


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
    public TItem GetSelectionData()
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
    public void SetSelectionData(TItem data)
    {
      var item = GetItemsRecursive(item => item is DataItem dataItem && Equals(dataItem.data, data)).FirstOrDefault();
      if (item != null)
        SetSelection(new[] { item.id }, TreeViewSelectionOptions.RevealAndFrame);
    }


    // Convert an enumerable of columns to a multi column header
    private static MultiColumnHeader CreateMultiColumnHeader(IEnumerable<Column> columns)
    {
      if (columns == null || columns.Count() == 0)
        return null;

      return new MultiColumnHeader(new MultiColumnHeaderState(columns.Select(column => new MultiColumnHeaderState.Column() {
        headerContent = column.header,
        width = column.width,
        minWidth = column.width,
        headerTextAlignment = column.alignment,
        canSort = column.isSortable,
        allowToggleVisibility = column.isHideable,
        autoResize = true,
      }).ToArray()));
    }


    #region Overridden tree view methods
    // Build the tree data and return the root item of the tree
    protected override TreeViewItem BuildRoot()
    {
      // Create the roo item
      var id = 0;
      var rootItem = new TreeViewItem(id++, -1);

      // Build the root item
      Build(ref rootItem, ref id);

      // Setup the depths of the root item
      SetupDepthsFromParentsAndChildren(rootItem);

      // Return the root item
      return rootItem;
    }

    // Draw the rows
    protected override void RowGUI(RowGUIArgs args)
    {
      // Iterate over the visible columns
      for (var i = 0; i < args.GetNumVisibleColumns(); i++)
      {
        // Get the index and rect of the column
        var columnIndex = args.GetColumn(i);
        var columnRect = args.GetCellRect(i);

        // Check the type of the item
        if (args.item is DataItem dataItem)
        {
          // Indent the rect in the first column
          if (columnIndex == 0 && !isSearching)
            columnRect = columnRect.ContractLeft(GetContentIndent(dataItem));

          // Draw the column
          columns[columnIndex].Draw(columnRect, dataItem);
        }
        else if (args.item is GroupItem groupItem && columnIndex == 0 && !isSearching)
        {
          // Indent the rect
          columnRect = columnRect.ContractLeft(GetContentIndent(groupItem));

          // Draw the group
          OnGroupGUI(columnRect, groupItem);
        }
      }
    }

    // Handler for when an item is selected
    protected override void SelectionChanged(IList<int> ids)
    {
      // Check if any item is selected
      if (ids.Count == 0)
        return;

      // Find the selected item
      var itemId = ids.First();
      var item = FindItem(itemId, rootItem);

      // Check the type of the item
      if (item is DataItem dataItem)
      {
        // Invoke the item selected event
        onItemSelected?.Invoke(dataItem.data);
      }
    }

    // Handler for when an item is single clicked
    protected override void SingleClickedItem(int id)
    {
      // Find the clicked item
      var item = FindItem(id, rootItem);

      // Check the type of the item
      if (item is DataItem dataItem)
      {
        // Invoke the item single clicked event
        onItemSingleClicked?.Invoke(dataItem.data);

        // Handle the single click
        OnSingleClicked(dataItem);
      }
      else if (item is GroupItem groupItem)
      {
        // Toggle the expanded state of the group item
        var expanded = GetExpanded().Contains(groupItem.id);
        SetExpanded(groupItem.id, !expanded);
      }
    }

    // Handler for when an item is double clicked
    protected override void DoubleClickedItem(int id)
    {
      // Find the clicked item
      var item = FindItem(id, rootItem);

      // Check the type of the item
      if (item is DataItem dataItem)
      {
        // Invoke the item double clicked event
        onItemDoubleClicked?.Invoke(dataItem.data);

        // Handle the double click
        OnDoubleClicked(dataItem);
      }
    }

    // Handler for when an item is context clicked
    protected override void ContextClickedItem(int id)
    {
      // Find the clicked item
      var item = FindItem(id, rootItem);

      // Check the type of the item
      if (item is DataItem dataItem)
      {
        // Invoke the item context clicked event
        onItemContextClicked?.Invoke(dataItem.data);

        // Show the context menu for the data item
        GetDataItemContextMenu(dataItem)?.ShowAsContext();
      }
      else if (item is GroupItem groupItem)
      {
        // Show the context menu for the group item
        GetGroupItemContextMenu(groupItem)?.ShowAsContext();
      }
    }

    // Return if an item matches a search query
    protected override bool DoesItemMatchSearch(TreeViewItem item, string searchString)
    {
      if (string.IsNullOrEmpty(searchString))
        return true;

      return _matcher == null || (item is DataItem dataItem && _matcher(dataItem.data, searchString));
    }

    // Return a context menu for a data item
    private GenericMenu GetDataItemContextMenu(DataItem item)
    {
      var menu = new GenericMenu();
      FillDataItemContextMenu(item, menu);
      return menu;
    }

    // Return a context menu for a group item
    private GenericMenu GetGroupItemContextMenu(GroupItem item)
    {
      var menu = new GenericMenu();
      FillGroupItemContextMenu(item, menu, GetExpanded().Contains(item.id));
      return menu;
    }
    #endregion
  }
}