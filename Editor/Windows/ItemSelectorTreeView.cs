using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Audune.Utils.UnityEditor.Editor
{
  // Class that defines a tree view to select an item
  public abstract class ItemSelectorTreeView<TItem> : ItemTreeView<TItem>
  {
    // Return if a default item should be added
    protected virtual bool _addDefaultItem => false;


    // Constructor
    public ItemSelectorTreeView(TreeViewState treeViewState) : base(treeViewState)
    {
    }


    // Build the items of the tree view
    protected override void Build(ref TreeViewItem rootItem, ref int id)
    {
      // Create the default item
      if (_addDefaultItem)
      {
        var defaultItem = CreateDefaultDataItem(ref id);
        rootItem.AddChild(defaultItem);
      }

      // Build the item tree view
      base.Build(ref rootItem, ref id);
    }

    // Create a data item from the default object
    private DataItem CreateDefaultDataItem(ref int id)
    {
      return new DataItem() { id = id++, data = SelectDefaultData(), displayName = SelectDefaultDisplayName(), icon = SelectDefaultIcon() };
    }

    // Return the data for the default item
    protected virtual TItem SelectDefaultData()
    {
      return default;
    }

    // Return the display name for the default item
    protected virtual string SelectDefaultDisplayName()
    {
      return "<None>";
    }

    // Return the icon for the default item
    protected virtual Texture2D SelectDefaultIcon()
    {
      return null;
    }
  }
}