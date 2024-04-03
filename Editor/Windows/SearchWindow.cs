using UnityEditor;
using UnityEngine;

namespace Audune.Utils.UnityEditor.Editor
{
  // Class that defines a search editor window
  public abstract class SearchWindow<T> : EditorWindow
  {
    // Show the window as a dropdown at the specified button position
    public static void ShowAsDropDown<TWindow>(Rect buttonRect, SerializedProperty property) where TWindow : SearchWindow<T>
    {
      // Create the window
      var window = CreateInstance<TWindow>();
      window.serializedProperty = property;

      // Show the window at the mouse position
      var screenButtonRect = GUIUtility.GUIToScreenRect(buttonRect);
      window.ShowAsDropDown(screenButtonRect, new Vector2(600, 300));
    }


    // Reference to the serialized property
    public SerializedProperty serializedProperty;

    // Tree view for displaying the search results
    private SearchTreeView<T> _treeView;


    // Initialize the tree view
    private void InitializeTreeView()
    {
      // Create the tree view
      _treeView = CreateTreeView();

      // Reload the tree view
      _treeView.Reload();

      // Select the initially selected item
      _treeView.SetSelectionData(GetPropertyValue());

      // Set the property value of the window when an item is selected
      _treeView.OnItemSelected += item => SetPropertyValue(item);

      // Close the window when an item is double clicked
      _treeView.OnItemDoubleClicked += item => Close();
    }

    // OnGUI is called when the editor is drawn
    private void OnGUI()
    {
      if (_treeView == null)
        InitializeTreeView();

      // Draw the search box
      GUILayout.BeginHorizontal(EditorStyles.toolbar);
      OnToolbarGUI();
      GUILayout.EndHorizontal();

      // Draw the tree view
      OnTreeViewGUI();
    }


    // OnToolbarGUI is called when the toolbar is drawn
    protected virtual void OnToolbarGUI()
    {
      GUI.SetNextControlName("searchField");
      _treeView.searchString = EditorGUILayout.TextField(_treeView.searchString, EditorStyles.toolbarSearchField, GUILayout.ExpandWidth(true));
      GUI.FocusControl("searchField");
    }

    // OnTreeViewGUI is called when the tree view is drawn
    protected virtual void OnTreeViewGUI()
    {
      var rect = EditorGUILayout.GetControlRect(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
      _treeView.Reload();
      _treeView.OnGUI(rect);
    }


    // Create the tree view
    public abstract SearchTreeView<T> CreateTreeView();

    // Get the property value
    public abstract T GetPropertyValue();

    // Set the property value
    public abstract void SetPropertyValue(T value);
  }
}