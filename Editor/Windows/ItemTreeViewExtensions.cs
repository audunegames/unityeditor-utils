using System.Collections.Generic;
using System.Linq;
using UnityEditor.IMGUI.Controls;

namespace Audune.Utils.UnityEditor.Editor
{
  // Class that defines extension methods for item tree views
  internal static class ItemTreeViewExtensions
  {
    // Convert an item tree view column to a tree view column
    public static MultiColumnHeaderState.Column ToTreeViewColumn<TItem>(this ItemTreeView<TItem>.Column column)
    {
      return new MultiColumnHeaderState.Column() {
        headerContent = column.header,
        width = column.width,
        headerTextAlignment = column.alignment,
        canSort = column.isSortable,
        allowToggleVisibility = column.isHideable,
      };
    }

    // Convert an enumerable of item tree view columns to a tree view header
    public static MultiColumnHeader ToTreeViewHeader<TItem>(this IEnumerable<ItemTreeView<TItem>.Column> columns)
    {
      if (columns == null || columns.Count() == 0)
        return null;

      return new MultiColumnHeader(new MultiColumnHeaderState(columns.Select(ToTreeViewColumn).ToArray()));
    }
  }
}