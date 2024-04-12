using System;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Audune.Utils.UnityEditor.Editor
{
  // Class that defines a tree view for displaying serialized property search results
  public class SerializedPropertySearchTreeView : ItemTreeView<SerializedPropertySearchResult>
  {
    // Default strings
    private const string _prefabsDisplayName = "Prefabs";
    private const string _scriptableObjectsDisplayName = "Scriptable objects";
    private const string _scenesDisplayName = "Scene objects";

    // Default keys
    private const string _objectKey = "object";
    private const string _componentKey = "component";
    private const string _nameKey = "name";

    private static readonly string[] _keys = new[] { _objectKey, _componentKey, _nameKey };


    // Constructor
    public SerializedPropertySearchTreeView(TreeViewState treeViewState, SerializedPropertySearch.Predicate predicate, params string[] searchInFolders) : base(treeViewState)
    {
      // Set the tree view items
      items = SerializedPropertySearch.SearchInProject(predicate, searchInFolders).ToList();

      // Set the tree view columns
      columns = new[] {
        new Column(new GUIContent("Object"), OnObjectColumnGUI, width: 250),
        new Column(new GUIContent("Component"), OnComponentColumnGUI, width: 150, isHideable: true),
        new Column(new GUIContent("Property Name"), OnPropertyNameColumnGUI, width: 150, isHideable: true),
        new Column(new GUIContent("Property Value"), OnPropertyValueColumnGUI, width: 200),
      };

      // Set the tree view item matcher
      matcher = ItemMatcher.Keys(
        (_objectKey, ItemMatcher.String<SerializedPropertySearchResult>(data => data.asset.assetName, data => data.component.componentPath)),
        (_componentKey, ItemMatcher.String<SerializedPropertySearchResult>(data => data.component.componentScript?.name)),
        (_nameKey, ItemMatcher.String<SerializedPropertySearchResult>(data => data.ToString())));
    }


    // Return the path for a data item
    protected override string[] SelectDataPath(SerializedPropertySearchResult data)
    {
      var root = data.asset.type switch {
        EditorAssetType.Prefab => _prefabsDisplayName,
        EditorAssetType.PrefabModel => _prefabsDisplayName,
        EditorAssetType.PrefabVariant => _prefabsDisplayName,
        EditorAssetType.ScriptableObject => _scriptableObjectsDisplayName,
        EditorAssetType.Scene => _scenesDisplayName,
        _ => throw new ArgumentException($"Undefined asset type {data.asset.type}", nameof(data)),
      };

      return new[] { root, data.asset.assetDirectoryName, data.ToString() };
    }

    // Return the display string for a data item
    protected override string SelectDataDisplayName(SerializedPropertySearchResult data)
    {
      return data.component.ToString();
    }

    // Return the icon for a data item
    protected override Texture2D SelectDataIcon(SerializedPropertySearchResult data)
    {
      return data.asset.type.GetComponentIcon();
    }

    // Return the display string for a group item
    protected override string SelectGroupDisplayName(string[] path, bool expanded)
    {
      return path[^1];
    }

    // Return the icon for a group item
    protected override Texture2D SelectGroupIcom(string[] path, bool expanded)
    {
      if (path.Length > 1)
        return expanded ? EditorIcons.folderOpened : EditorIcons.folder;
      else if (path[0] == _prefabsDisplayName)
        return EditorIcons.prefab;
      else if (path[0] == _scriptableObjectsDisplayName)
        return EditorIcons.scriptableObject;
      else if (path[0] == _scenesDisplayName)
        return EditorIcons.gameObject;
      else
        return null;
    }


    // Draw the object column GUI
    protected virtual void OnObjectColumnGUI(Rect rect, DataItem item)
    {
      // Draw a label for the object
      EditorGUI.LabelField(rect, HighlightSearchString(item, searchString, _keys, _objectKey), ItemTreeViewStyles.label);
    }

    // Draw the component column GUI
    protected virtual void OnComponentColumnGUI(Rect rect, DataItem item)
    {
      // Draw a label for the component
      EditorGUI.LabelField(rect, HighlightSearchString(new GUIContent(ObjectNames.NicifyVariableName(item.data.component.componentScript.name), item.data.component.componentIcon), searchString, _keys, _componentKey), ItemTreeViewStyles.label);
    }

    // Draw the property name column GUI
    protected virtual void OnPropertyNameColumnGUI(Rect rect, DataItem item)
    {
      // Get the serialized property
      var serializedProperty = item.data.targetSerializedProperty;

      // Draw a label for the property name
      EditorGUI.LabelField(rect, HighlightSearchString(item.data.ToString(), searchString, _keys, _nameKey), serializedProperty != null && serializedProperty.prefabOverride ? ItemTreeViewStyles.boldLabel : ItemTreeViewStyles.label);
    }

    // Draw the property value column GUI
    protected virtual void OnPropertyValueColumnGUI(Rect rect, DataItem item)
    {
      // Get the serialized property
      var serializedProperty = item.data.targetSerializedProperty;
      if (serializedProperty != null)
      {
        // Get the rect for drawing the property field
        var propertyRect = rect.ContractTop(EditorGUIUtility.standardVerticalSpacing * 0.5f).ContractBottom(EditorGUIUtility.standardVerticalSpacing * 0.5f);

        // Draw the property field
        serializedProperty.serializedObject.Update();
        OnPropertyValueFieldGUI(propertyRect, serializedProperty);
        serializedProperty.serializedObject.ApplyModifiedProperties();
      }
      else
      {
        // Draw a label field to indicate an error
        EditorGUI.LabelField(rect, "<color=#bf5130>Scene is not loaded</color>", ItemTreeViewStyles.label);
      }
    }

    // Draw the property value field GUI
    protected virtual void OnPropertyValueFieldGUI(Rect rect, SerializedProperty serializedProperty)
    {
      // Draw the property field
      EditorGUI.PropertyField(rect, serializedProperty);
    }

    // Handler for when an item is double clicked
    protected override void OnDoubleClicked(DataItem item)
    {
      // Set the item as the selection in the editor
      SetAsSelection(item.data);
    }

    // Fill a context menu for the specified data item
    protected override void FillDataItemContextMenu(DataItem item, GenericMenu menu)
    {
      // Items for showing the asset and component
      menu.AddItem(new GUIContent("Show Component"), false, () => SetAsSelection(item.data));
      menu.AddItem(new GUIContent("Show Asset"), false, () => item.data.asset.SetAsSelection());
      menu.AddItem(new GUIContent("Copy Path"), false, () => GUIUtility.systemCopyBuffer = item.data.asset.assetPath);

      // Items depending on the type of the asset
      if (item.data.asset.type.IsPrefab())
      {
        menu.AddSeparator("");

        // Items for opening the prefab asset
        menu.AddItem(new GUIContent("Open Containing Prefab"), false, () => AssetDatabase.OpenAsset(item.data.asset.GetAsset<GameObject>()));
      }
      else if (item.data.asset.type.IsScene())
      {
        menu.AddSeparator("");

        // Items for opening the scene asset
        menu.AddItem(new GUIContent("Open Containing Scene"), false, () => EditorSceneManager.OpenScene(item.data.asset.assetPath, OpenSceneMode.Single));
        menu.AddItem(new GUIContent("Open Containing Scene Additive"), false, () => EditorSceneManager.OpenScene(item.data.asset.assetPath, OpenSceneMode.Additive));
      }

      // Items depending on the component script of the asset
      if (item.data.component.componentScript != null)
      {
        menu.AddSeparator("");

        // Items for editing the script
        menu.AddItem(new GUIContent("Edit Script"), false, () => AssetDatabase.OpenAsset(item.data.component.componentScript));
      }

      // Fill the base menu items
      menu.AddSeparator("");
      base.FillDataItemContextMenu(item, menu);
    }


    // Set the data item as the selectiom in the editor
    private void SetAsSelection(SerializedPropertySearchResult data)
    {
      // Check the type of the asset
      if (data.asset.type.IsPrefab())
      {
        // Open the prefab
        AssetDatabase.OpenAsset(data.asset.GetAsset<GameObject>());
      }
      else if (data.asset.type.IsScene())
      {
        // Open the scene if not done already
        var scene = data.asset.GetScene();
        if (!scene.IsValid())
          EditorSceneManager.OpenScene(data.asset.assetPath, OpenSceneMode.Single);
      }

      // Select the component
      data.component.SetAsSelection();
    }
  }
}