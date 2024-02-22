using System;
using UnityEditor;
using UnityEngine;

namespace Audune.Utils.UnityEditor.Editor
{
  // Class that defines extension methods for scriptable objects
  public static class ScriptableObjectExtensions
  {
    // Create a scriptable object with its name set to the nice type name
    public static ScriptableObject CreateInstance(Type type)
    {
      var obj = ScriptableObject.CreateInstance(type);
      obj.name = type.ToDisplayString(TypeDisplayOptions.DontShowNamespace);
      return obj;
    }

    // Create a scriptable object with its name set to the nice type name
    public static TObject CreateInstance<TObject>() where TObject : ScriptableObject
    {
      return CreateInstance(typeof(TObject)) as TObject;
    }


    // Add an asset to a scriptable object
    public static void AddChildAsset(this ScriptableObject target, ScriptableObject asset)
    {
      AssetDatabase.AddObjectToAsset(asset, target);
      AssetDatabase.SaveAssetIfDirty(target);
    }

    // Add an asset to a scriptable object from a serialized property
    public static void AddChildAsset(this ScriptableObject target, SerializedProperty assetProperty)
    {
      var objectReference = assetProperty.objectReferenceValue;
      if (objectReference is ScriptableObject asset)
        AddChildAsset(target, asset);
    }


    // Remove an asset from a scriptable object
    public static void RemoveChildAsset(this ScriptableObject target, ScriptableObject asset)
    {
      AssetDatabase.RemoveObjectFromAsset(asset);
      AssetDatabase.SaveAssetIfDirty(target);
    }

    // Remove an asset from a scriptable object from a serialized property
    public static void RemoveChildAsset(this ScriptableObject target, SerializedProperty assetProperty)
    {
      var objectReference = assetProperty.objectReferenceValue;
      if (objectReference is ScriptableObject asset)
        RemoveChildAsset(target, asset);
    }
  }
}