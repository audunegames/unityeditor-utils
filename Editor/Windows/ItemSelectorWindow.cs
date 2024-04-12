using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Audune.Utils.UnityEditor.Editor
{
  // Class that defines an editor window for selecting an item using a tree view
  public abstract class ItemSelectorWindow<TItem, TTreeView> : ItemWindow<TItem, TTreeView> where TTreeView : ItemSelectorTreeView<TItem>
  {
    // Open the window with the specified serialized property
    public static TWindow Open<TWindow>(SerializedProperty property) where TWindow : ItemSelectorWindow<TItem, TTreeView>
    {
      // Create the window and set the serialized property
      var window = Open<TWindow>();
      window.serializedProperty = property;

      // Return the window
      return window;
    }

    // Open the window as a dropdown at the specified button position with the specified serialized property
    public static TWindow OpenAsDropDown<TWindow>(Rect buttonRect, SerializedProperty property) where TWindow : ItemSelectorWindow<TItem, TTreeView>
    {
      // Create the window and set the serialized property
      var window = OpenAsDropDown<TWindow>(buttonRect);
      window.serializedProperty = property;

      // Return the window
      return window;
    }


    // Reference to the serialized property
    public SerializedProperty serializedProperty;


    // Get the property value
    public abstract TItem GetPropertyValue();

    // Set the property value
    public abstract void SetPropertyValue(TItem value);

    // Create the tree view
    public abstract TTreeView CreateTreeView(TreeViewState treeViewState);

    // Initialize the tree view
    protected override TTreeView Initialize(TreeViewState treeViewState)
    {
      // Create the tree view
      var treeView = CreateTreeView(treeViewState);

      // Reload the tree view
      treeView.Reload();

      // Select the initially selected item
      treeView.SetSelectionData(GetPropertyValue());

      // Set the property value of the window when an item is selected
      treeView.onItemSelected += item => SetPropertyValue(item);

      // Close the window when an item is double clicked
      treeView.onItemDoubleClicked += item => Close();

      return treeView;
    }
  }
}