using Audune.Utils.UnityEditor.Editor;
using UnityEditor;
using UnityEngine;

namespace Audune.Localization.Editor
{
  // Class that defines an editor window for displaying serialized property search results
  public abstract class SerializedPropertySearchWindow<TTreeView> : ItemWindow<SerializedPropertySearchResult, TTreeView> where TTreeView : SerializedPropertySearchTreeView
  {
    // OnButtonsGUI is called when the buttons on the toolbar are drawn
    protected override void OnToolbarButtonsGUI()
    {
      // Rescan project button
      if (GUILayout.Button(new GUIContent("Rescan Project", EditorIcons.refresh, "Rescan the project for assets"), EditorStyles.toolbarButton, GUILayout.ExpandWidth(false)))
        Refresh(searchString: treeView.searchString, forceRebuild: true);
    }
  }
}