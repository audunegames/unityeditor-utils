using UnityEditor;
using UnityEngine;

namespace Audune.Utils.UnityEditor.Editor
{
  // Class that defines common editor icons
  public static class EditorIcons
  {
    // Editor icons
    public static readonly Texture2D prefab;
    public static readonly Texture2D prefabModel;
    public static readonly Texture2D prefabVariant;
    public static readonly Texture2D scriptableObject;
    public static readonly Texture2D scene;
    public static readonly Texture2D gameObject;
    public static readonly Texture2D assembly;
    public static readonly Texture2D script;
    public static readonly Texture2D folder;
    public static readonly Texture2D folderOpened;
    public static readonly Texture2D folderEmpty;
    public static readonly Texture2D defaultAsset;
    public static readonly Texture2D textAsset;
    public static readonly Texture2D assemblyDefinitionAsset;
    public static readonly Texture2D assemblyDefinitionReferenceAsset;
    public static readonly Texture2D text;
    public static readonly Texture2D font;
    public static readonly Texture2D settings;
    public static readonly Texture2D refresh;
    public static readonly Texture2D checkMark;
    public static readonly Texture2D warningMark;
    public static readonly Texture2D errorMark;

    // Editor colors
    public static readonly Color warningColor = new Color(1.0f, 0.94f, 0.76f);
    public static readonly Color errorColor = new Color(1.0f, 0.86f, 0.81f);


    // Static constructor
    static EditorIcons()
    {
      prefab = EditorGUIUtility.IconContent("Prefab Icon").image as Texture2D;
      prefabModel = EditorGUIUtility.IconContent("PrefabModel Icon").image as Texture2D;
      prefabVariant = EditorGUIUtility.IconContent("PrefabVariant Icon").image as Texture2D;
      scriptableObject = EditorGUIUtility.IconContent("ScriptableObject Icon").image as Texture2D;
      scene = EditorGUIUtility.IconContent("SceneAsset Icon").image as Texture2D;
      gameObject = EditorGUIUtility.IconContent("GameObject Icon").image as Texture2D;
      assembly = EditorGUIUtility.IconContent("Assembly Icon").image as Texture2D;
      script = EditorGUIUtility.IconContent("cs Script Icon").image as Texture2D;
      folder = EditorGUIUtility.IconContent("Folder Icon").image as Texture2D;
      folderOpened = EditorGUIUtility.IconContent("FolderOpened Icon").image as Texture2D;
      folderEmpty = EditorGUIUtility.IconContent("FolderEmpty Icon").image as Texture2D;
      defaultAsset = EditorGUIUtility.IconContent("DefaultAsset Icon").image as Texture2D;
      textAsset = EditorGUIUtility.IconContent("TextAsset Icon").image as Texture2D;
      assemblyDefinitionAsset = EditorGUIUtility.IconContent("AssemblyDefinitionAsset Icon").image as Texture2D;
      assemblyDefinitionReferenceAsset = EditorGUIUtility.IconContent("AssemblyDefinitionReferenceAsset Icon").image as Texture2D;
      text = EditorGUIUtility.IconContent("Text Icon").image as Texture2D;
      font = EditorGUIUtility.IconContent("Font Icon").image as Texture2D;
      settings = EditorGUIUtility.IconContent("SettingsIcon").image as Texture2D;
      refresh = EditorGUIUtility.IconContent("Refresh").image as Texture2D;
      checkMark = EditorGUIUtility.IconContent("FilterSelectedOnly").image as Texture2D;
      warningMark = EditorGUIUtility.IconContent("Warning").image as Texture2D;
      errorMark = EditorGUIUtility.IconContent("Error").image as Texture2D;
    }
  }
}