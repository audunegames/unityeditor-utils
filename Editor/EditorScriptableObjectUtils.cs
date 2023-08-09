using System;
using UnityEditor;
using UnityEngine;

namespace Audune.Utils.Editor
{
  // Class that defines utility methods for scriptable objects in the editor
  public static class EditorScriptableObjectUtils
  {
    // Create a scriptable object with its name set to the nice type name
    public static ScriptableObject CreateInstanceWithNiceName(Type type)
    {
      var obj = ScriptableObject.CreateInstance(type);
      obj.name = type.ToDisplayString(TypeDisplayOptions.DontShowNamespace);
      return obj;
    }

    // Create a scriptable object with its name set to the nice type name
    public static TObject CreateInstanceWithNiceName<TObject>() where TObject : ScriptableObject
    {
      return CreateInstanceWithNiceName(typeof(TObject)) as TObject;
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