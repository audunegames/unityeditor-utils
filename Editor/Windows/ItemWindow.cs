using System;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Audune.Utils.UnityEditor.Editor
{
  // Class that defines an editor window for displaying items using a tree view
  public abstract class ItemWindow<TItem, TTreeView> : EditorWindow where TTreeView : ItemTreeView<TItem>
  {
    // Open the window
    public static TWindow Open<TWindow>(TItem selected = default, string searchString = null, Action<TWindow> onBeforeRefresh = null) where TWindow : ItemWindow<TItem, TTreeView>
    {
      // Get the window and set its minimum size
      var window = GetWindow<TWindow>();
      window.minSize = new Vector2(800, 600);

      // Refresh the window
      onBeforeRefresh?.Invoke(window);
      window.Refresh(searchString, selected);

      // Return the window
      return window;
    }

    // Open the window as a dropdown at the specified button position
    public static TWindow OpenAsDropDown<TWindow>(Rect buttonRect, TItem selected = default, string searchString = null, Action<TWindow> onBeforeRefresh = null) where TWindow : ItemWindow<TItem, TTreeView>
    {
      // Create the window
      var window = CreateInstance<TWindow>();

      // Refresh the window
      onBeforeRefresh?.Invoke(window);
      window.Refresh(searchString, selected);

      // Show the window at the mouse position
      var screenButtonRect = GUIUtility.GUIToScreenRect(buttonRect);
      window.ShowAsDropDown(screenButtonRect, new Vector2(600, 300));

      // Return the window
      return window;
    }


    // Tree view for displaying the items
    private TTreeView _treeView;

    // State of the tree view
    [SerializeField]
    private TreeViewState _treeViewState;


    // Return the tree view for displaying the items
    public TTreeView treeView => _treeView;

    // Return if the search field should be expanded
    public virtual bool expandSearchField => true;


    // Initialize the tree view
    protected abstract TTreeView Initialize(TreeViewState treeViewState);

    // Refresh the window
    protected void Refresh(string searchString = null, TItem selected = default, bool forceRebuild = false)
    {
      Rebuild(forceRebuild);
      _treeView.Reload();

      if (!string.IsNullOrEmpty(searchString))
        _treeView.searchString = searchString;
      else if (!Equals(selected, default))
        _treeView.SetSelectionData(selected);
    }

    // Rebuild the tree view
    protected void Rebuild(bool forceRebuild = false)
    {
      if (forceRebuild || _treeView == null)
      {
        if (_treeViewState == null)
          _treeViewState = new TreeViewState();
        _treeView = Initialize(_treeViewState);
      }
    }


    // OnGUI is called when the editor is drawn
    private void OnGUI()
    {
      // Rebuild the tree view
      Rebuild();

      // Draw the toolbar
      GUILayout.BeginHorizontal(EditorStyles.toolbar);
      {
        // Toolbar buttons
        OnToolbarButtonsGUI();

        // Flexible space if applicable
        if (!expandSearchField)
          GUILayout.FlexibleSpace();

        // Search field
        GUI.SetNextControlName("searchField");
        _treeView.searchString = EditorGUILayout.TextField(_treeView.searchString, EditorStyles.toolbarSearchField, expandSearchField ? GUILayout.ExpandWidth(true) : GUILayout.MaxWidth(300));
        GUI.FocusControl("searchField");

        // Tree view buttons
        if (GUILayout.Button(new GUIContent("Expand All", "Expand all tree view items"), EditorStyles.toolbarButton, GUILayout.ExpandWidth(false)))
          _treeView.ExpandAll();
        if (GUILayout.Button(new GUIContent("Collapse All", "Collapse all tree view items"), EditorStyles.toolbarButton, GUILayout.ExpandWidth(false)))
          _treeView.CollapseAll();
      }
      GUILayout.EndHorizontal();

      // Draw the tree view
      var rect = EditorGUILayout.GetControlRect(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
      _treeView.Reload();
      _treeView.OnGUI(rect);
    }

    // OnButtonsGUI is called when the buttons on the toolbar are drawn
    protected virtual void OnToolbarButtonsGUI()
    {
    }
  }
}