using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Audune.Utils.UnityEditor.Editor
{
  // Class that defines a search for serialized properties in the project
  public static class SerializedPropertySearch
  {
    // Delegate that defines a predicate for a serialized property search
    public delegate bool Predicate(SerializedProperty property);

    
    // Get the results in the specified target object
    private static IEnumerable<SerializedPropertySearchResult> SearchInObject(EditorAsset asset, EditorComponent component, Object target, Predicate predicate)
    {
      // Check if the target object is defined
      if (target == null)
        yield break;

      // Iterate over the instance fields in the target object
      var serializedObject = new SerializedObject(target);
      foreach (var property in serializedObject.GetChildren(true, p => predicate(p)))
      {
        // Yield a new result for the field
        yield return new SerializedPropertySearchResult(asset, component, property.propertyPath);
      }
    }


    #region Searching in the project
    // Search in the project
    public static IEnumerable<SerializedPropertySearchResult> SearchInProject(Predicate predicate, params string[] searchInFolders)
    {
      return SearchInPrefabs(predicate, searchInFolders)
        .Concat(SearchInScriptableObjects(predicate, searchInFolders))
        .Concat(SearchInScenes(predicate, searchInFolders));
    }
    #endregion

    #region Searching in prefabs in the project
    // Search in prefab assets in the project
    public static IEnumerable<SerializedPropertySearchResult> SearchInPrefabs(Predicate predicate, params string[] searchInFolders)
    {
      return SearchInPrefabs(EditorAsset.FindPrefabs(searchInFolders), predicate);
    }

    // Search in the specified prefab assets
    private static IEnumerable<SerializedPropertySearchResult> SearchInPrefabs(IReadOnlyList<EditorAsset> assets, Predicate predicate)
    {
      // Iterate over the prefab assets
      for (var assetIndex = 0; assetIndex < assets.Count; assetIndex++)
      {
        var asset = assets[assetIndex];

        // Increment the progress
        EditorUtility.DisplayProgressBar("Searching", $"Searching for localized strings in prefabs: {asset.assetPath}", assetIndex / (float)assets.Count);

        // Return the results found in the prefab in the asset
        var prefab = asset.GetAsset<GameObject>();
        foreach (var result in SearchInGameObject(asset, prefab, prefab, predicate))
          yield return result;
      }

      // Clear the progress
      EditorUtility.ClearProgressBar();
    }
    #endregion

    #region Searching in scriptable objects in the project
    // Search in scriptable object assets in the project
    public static IEnumerable<SerializedPropertySearchResult> SearchInScriptableObjects(Predicate predicate, params string[] searchInFolders)
    {
      return SearchInScriptableObjects(EditorAsset.FindScriptableObjects(searchInFolders), predicate);
    }

    // Search in the specified scriptable object assets
    private static IEnumerable<SerializedPropertySearchResult> SearchInScriptableObjects(IReadOnlyList<EditorAsset> assets, Predicate predicate)
    {
      // Iterate over the scriptable object assets
      for (var assetIndex = 0; assetIndex < assets.Count; assetIndex++)
      {
        var asset = assets[assetIndex];

        // Increment the progress
        EditorUtility.DisplayProgressBar("Searching", $"Searching for localized strings in scriptable objects: {asset.assetPath}", assetIndex / (float)assets.Count);

        // Iterate over the scriptable objects in the asset
        var scriptableObjects = asset.GetAllAssets<ScriptableObject>();
        foreach (var scriptableObject in scriptableObjects)
        {
          // Return the results found in the scriptable object
          foreach (var result in SearchInObject(asset, EditorComponent.FromScriptableObject(asset, scriptableObject), scriptableObject, predicate))
            yield return result;
        }
      }

      // Clear the progress
      EditorUtility.ClearProgressBar();
    }
    #endregion

    #region Searching in scenes in the project
    // Search in scene assets in the project
    public static IEnumerable<SerializedPropertySearchResult> SearchInScenes(Predicate predicate, params string[] searchInFolders)
    {
      return SearchInScenes(EditorAsset.FindScenes(searchInFolders), predicate);
    }

    // Search in the specified scene assets
    private static IEnumerable<SerializedPropertySearchResult> SearchInScenes(IReadOnlyList<EditorAsset> assets, Predicate predicate)
    {
      // Save the scene manager setup
      var sceneManagerSetup = EditorSceneManager.GetSceneManagerSetup();

      // Iterate over the scene assets
      for (var assetIndex = 0; assetIndex < assets.Count; assetIndex++)
      {
        var asset = assets[assetIndex];

        // Increment the progress
        EditorUtility.DisplayProgressBar("Searching", $"Searching for localized strings in scenes: {asset.assetPath}", assetIndex / (float)assets.Count);

        // Open the scene if not done already
        var scene = asset.GetScene();
        if (!scene.IsValid())
          scene = EditorSceneManager.OpenScene(asset.assetPath, OpenSceneMode.Single);

        // Iterate over the root game objects in the scene
        var rootGameObjects = scene.GetRootGameObjects();
        foreach (var rootGameObject in rootGameObjects)
        {
          // Return the results found in the root game object
          foreach (var result in SearchInGameObject(asset, rootGameObject, null, predicate))
            yield return result;
        }
      }

      // Clear the progress
      EditorUtility.ClearProgressBar();

      // Restore the scene manager setup
      if (assets.Count > 0 && sceneManagerSetup.Length > 0)
        EditorSceneManager.RestoreSceneManagerSetup(sceneManagerSetup);
    }
    #endregion

    #region Searching in game objects
    // Search in the specified game object
    private static IEnumerable<SerializedPropertySearchResult> SearchInGameObject(EditorAsset asset, GameObject gameObject, GameObject rootGameObject, Predicate predicate)
    {
      // Check if the game object is defined
      if (gameObject == null)
        yield break;

      // Iterate over the mono behaviours in the game object
      var monoBehaviours = gameObject.GetComponentsInChildren<MonoBehaviour>(true);
      foreach (var monoBehaviour in monoBehaviours)
      {
        // Return the results found in the mono behaviour
        foreach (var result in SearchInObject(asset, EditorComponent.FromMonoBehaviour(asset, monoBehaviour, rootGameObject), monoBehaviour, predicate))
          yield return result;
      }
    }
    #endregion
  }
}